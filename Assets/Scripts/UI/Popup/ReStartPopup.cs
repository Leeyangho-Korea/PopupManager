using UnityEngine;
using YH;

public class ReStartPopup : BasePopup
{
    protected override void HandleAction(string key)
    {
        switch (key)
        {
            case "Continue":
                Debug.Log("Continue 클릭됨");
                break;
            case "Restart":
                Debug.Log("Restart 클릭됨");
                break;
        }
    }

}