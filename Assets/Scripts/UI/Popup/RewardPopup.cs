using UnityEngine;
using YH;

public class RewardPopup : BasePopup
{
    // esc로 창 닫기 여부
    public override bool IsEscapeClosable => true;

    protected override void HandleAction(string key)
    {
        switch (key)
        {
            case "Close":
                Debug.Log("Close 클릭됨");
                break;
        }
    }


}