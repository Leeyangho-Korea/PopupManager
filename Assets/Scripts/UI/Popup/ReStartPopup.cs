using UnityEngine;
using YH;

public class ReStartPopup : BasePopup
{
    protected override void HandleAction(string key)
    {
        switch (key)
        {
            case "Continue":
                Debug.Log("Continue Ŭ����");
                break;
            case "Restart":
                Debug.Log("Restart Ŭ����");
                break;
        }
    }

}