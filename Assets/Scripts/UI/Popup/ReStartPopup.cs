using UnityEngine;
using YH;

public class ReStartPopup : BasePopup
{
    protected override void HandleAction(string key)
    {
        switch (key)
        {
            case "Continue":
                Debug.Log("Continue Å¬¸¯µÊ");
                break;
            case "Restart":
                Debug.Log("Restart Å¬¸¯µÊ");
                break;
        }
    }

}