using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RangeSimpleUI : MonoBehaviour
{
    [SerializeField] TMP_Text contentText;

    readonly string[] stringDifficult = new string[]{ "쉬움", "보통", "어려움", "연속50", "연속100" };
    readonly string[] stringActivate  = new string[] { "활성화", "비활성화" };

    RangeManager.RangeInfo info;

    private void OnEnable()
    {
        if (info == null)
            return;

        UpdateText();
    }

    public void Setup(RangeManager.RangeInfo info)
    {
        this.info = info;
    }
    public void UpdateText()
    {
        // 각 값에 따른 텍스트 대입.
        string content = string.Empty;
        content += stringDifficult[(int)info.difficulty];
        content += "\n";

        content += stringActivate[info.isBotArmor ? 0 : 1];
        content += "\n";

        content += stringActivate[info.isInfinityAmmo ? 0 : 1];
        content += "\n";

        contentText.text = content;
    }
}
