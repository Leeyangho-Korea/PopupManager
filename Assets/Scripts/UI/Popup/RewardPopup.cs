using UnityEngine;
using YH;

public class RewardPopup : BasePopup
{
    // esc�� â �ݱ� ����
    public override bool IsEscapeClosable => true;

    protected override void HandleAction(string key)
    {
        switch (key)
        {
            case "Close":
                Debug.Log("Close Ŭ����");
                break;
        }
    }


}