using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RangeUI : Singleton<RangeUI>
{
    [SerializeField] TMP_Text scoreText;            // 점수.
    [SerializeField] TMP_Text remainingText;        // 남은 적의 수.

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

        // 디테일 화면이 열리냐에 따라 시점 변경 잠금.
        if (isDetail)
            CameraLook.Instance.OnUnlockMouse();
        else
            CameraLook.Instance.OnLockMouse();
    }
}
