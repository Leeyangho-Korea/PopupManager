using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupType
{
    Info,
    ReStart,
    Reward,
    Levelup,
}

public static class DEF
{
    #region ====== UIPopup ======
    public static Dictionary <PopupType, string> PopupTypeToName = new Dictionary<PopupType, string>
    {
        { PopupType.Info, "InfoPopup" },
        { PopupType.ReStart, "ReStartPopup" },
        { PopupType.Reward, "RewardPopup" },
        { PopupType.Levelup, "LevelUpPopup" },
    };

    //public const string POPUP_INFO = "InfoPopup";
    //public const string POPUP_RESTART = "ReStartPopup";
    //public const string POPUP_REWARD = "RewardPopup";
    #endregion


}
