using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

namespace YH
{
    [Serializable]
    public class UIMapEntry
    {
        public Button button;
        public string actionKey;
    }

    public class BasePopup : MonoBehaviour
    {
        [Header("��ư -> Ű ����")]
        [SerializeField] private List<UIMapEntry> buttonMappings;

        private Dictionary<Button, string> buttonToActionKey = new();

        public virtual bool IsEscapeClosable => true;

        protected virtual void Awake()
        {
            buttonToActionKey.Clear();
            foreach (var entry in buttonMappings)
            {
                if (entry.button != null && !string.IsNullOrEmpty(entry.actionKey))
                {
                    buttonToActionKey[entry.button] = entry.actionKey;
                    entry.button.onClick.RemoveAllListeners();
                    entry.button.onClick.AddListener(() => OnButtonClicked(entry.button));
                }
            }
        }

        protected virtual void OnButtonClicked(Button clickedButton)
        {
            if (buttonToActionKey.TryGetValue(clickedButton, out string key))
            {
                HandleAction(key);
            }
        }

        protected virtual void HandleAction(string key)
        {
            Debug.Log($"[BasePopup] HandleAction ȣ���: {key}");
        }

        protected void Close()
        {
            PopupManager.Instance.HidePopup(gameObject.name, gameObject);
        }
    }
}