using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace YH
{
    public class PopupManager : MonoBehaviour
    {
        public static PopupManager Instance { get; private set; }

        [SerializeField] private Transform popupParentCanvas;

        private Dictionary<string, Queue<GameObject>> popupPools = new();
        private Dictionary<string, GameObject> loadedPrefabs = new();

        private List<BasePopup> activePopups = new();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TryCloseTopPopup();
            }
        }

        private void TryCloseTopPopup()
        {
            if (activePopups.Count == 0) return;

            var top = activePopups[activePopups.Count - 1];
            if (top != null && top.IsEscapeClosable)
            {
                HidePopup(top.gameObject.name, top.gameObject);
            }
        }

        public async void ShowPopup<T>(string popupName, Action<T> onReady = null) where T : BasePopup
        {
            GameObject popupObj = GetFromPool(popupName);

            if (popupObj == null)
            {
                if (!loadedPrefabs.TryGetValue(popupName, out var prefab))
                {
                    var handle = Addressables.LoadAssetAsync<GameObject>(popupName);
                    await handle.Task;

                    if (handle.Status != AsyncOperationStatus.Succeeded)
                    {
                        Debug.LogError($"[PopupManager] Addressable 로딩 실패: {popupName}");
                        return;
                    }

                    prefab = handle.Result;
                    loadedPrefabs[popupName] = prefab;
                }

                popupObj = Instantiate(prefab, popupParentCanvas);
                popupObj.name = popupName;
            }

            popupObj.SetActive(true);
            popupObj.transform.SetAsLastSibling();

            var popupScript = popupObj.GetComponent<T>();
            if (popupScript == null)
            {
                Debug.LogError($"[PopupManager] {popupName}에 {typeof(T).Name} 컴포넌트가 없습니다.");
                return;
            }

            activePopups.Add(popupScript);
            onReady?.Invoke(popupScript);
        }

        public void HidePopup(string popupName, GameObject popupObj)
        {
            popupObj.SetActive(false);

            var popupScript = popupObj.GetComponent<BasePopup>();
            if (popupScript != null)
            {
                activePopups.Remove(popupScript);
            }

            if (!popupPools.ContainsKey(popupName))
                popupPools[popupName] = new Queue<GameObject>();

            popupPools[popupName].Enqueue(popupObj);
        }

        private GameObject GetFromPool(string name)
        {
            if (popupPools.TryGetValue(name, out var queue) && queue.Count > 0)
                return queue.Dequeue();

            return null;
        }
    }
}