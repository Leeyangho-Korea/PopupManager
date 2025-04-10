using UnityEngine;
using YH;

public class Test : MonoBehaviour
{

    public PopupType popupType;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ShowPopup(DEF.PopupTypeToName[popupType]);
        }
    }

    void ShowPopup(string popupName)
    {
        PopupManager.Instance.ShowPopup<BasePopup>(popupName, popup =>
        {
            Debug.Log($"[STACK TEST] {popupName} 팝업 생성됨");
        });
    }
}