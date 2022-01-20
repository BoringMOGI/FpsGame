using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeUI : Singleton<RangeUI>
{
    [SerializeField] GameObject simplePanel;
    [SerializeField] GameObject detailPanel;
    
    void Start()
    {
        SwitchDetail(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F3))
        {
            OpenDetail();
        }
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
