using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RangeSimpleUI : MonoBehaviour
{
    [SerializeField] TMP_Text contentText;

    readonly string[] stringDifficult = new string[]{ "����", "����", "�����", "����50", "����100" };
    readonly string[] stringActivate  = new string[] { "Ȱ��ȭ", "��Ȱ��ȭ" };

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
        // �� ���� ���� �ؽ�Ʈ ����.
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
