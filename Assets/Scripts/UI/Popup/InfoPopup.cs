using UnityEngine;
using YH;

public class InfoPopup : BasePopup
{
    protected override void HandleAction(string key)
    {
        switch (key)
        {
            case "Close":
                Debug.Log("Close Å¬¸¯µÊ");
                break;
        }
    }

}