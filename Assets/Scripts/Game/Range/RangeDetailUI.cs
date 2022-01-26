using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static RangeManager.RangeInfo;

public class RangeDetailUI : MonoBehaviour
{
    [SerializeField] SelectedButton difficultButton;
    [SerializeField] SelectedButton movementButton;
    [SerializeField] SelectedButton armorButton;
    [SerializeField] SelectedButton ammoButton;

    SelectedButton.OnSendValueEvent onSendValue;

    public void Setup(RangeManager.RangeInfo rangeInfo, SelectedButton.OnSendValueEvent onSendValue)
    {
        this.onSendValue = onSendValue;

        // 버튼 셋업.
        difficultButton.Setup(INFO_TYPE.Difficult, (int)rangeInfo.difficulty, OnClickEvent);
        movementButton.Setup(INFO_TYPE.Movement, (int)rangeInfo.botMovement, OnClickEvent);
        armorButton.Setup(INFO_TYPE.Armor, rangeInfo.isBotArmor, OnClickEvent);
        ammoButton.Setup(INFO_TYPE.Ammo, rangeInfo.isInfinityAmmo, OnClickEvent);
    }

    private void OnClickEvent(INFO_TYPE infoType, int index)
    {   
        onSendValue?.Invoke(infoType, index);
    }
}
