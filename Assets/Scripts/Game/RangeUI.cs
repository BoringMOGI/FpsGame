using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RangeUI : Singleton<RangeUI>
{
    [SerializeField] TMP_Text scoreText;            // ����.
    [SerializeField] TMP_Text remainingText;        // ���� ���� ��.

    [SerializeField] GameObject simplePanel;
    [SerializeField] GameObject detailPanel;


    void Start()
    {
        SwitchDetail(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            OpenDetail();
        }
    }

    public void UpdateUI(int score, int remainingCount)
    {
        scoreText.text = score.ToString();
        remainingText.text = remainingCount.ToString();
    }
    public void OpenDetail()
    {
        SwitchDetail(true);
    }
    public void SwitchDetail(bool isDetail)
    {
        simplePanel.SetActive(!isDetail);
        detailPanel.SetActive(isDetail);

        // ������ ȭ���� �����Ŀ� ���� ���� ���� ���.
        if (isDetail)
            CameraLook.Instance.OnUnlockMouse();
        else
            CameraLook.Instance.OnLockMouse();
    }
}
