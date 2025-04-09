using UnityEngine;
using YH;

public class RewardPopup : BasePopup
{
    // esc·Î Ã¢ ´Ý±â ¿©ºÎ
    public override bool IsEscapeClosable => true;

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