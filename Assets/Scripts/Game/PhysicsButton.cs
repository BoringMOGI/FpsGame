using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public delegate void CallbackEvent();

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] Image checkImage;

    [Header("Event")]
    [SerializeField] UnityEvent<CallbackEvent> OnClick;
    
    bool isClick = false;

    private void Start()
    {
        ResetButton();
    }

    // ���� �浹�� �Ͼ�� Ŭ�� �̺�Ʈ ȣ��.
    private void OnCollisionEnter(Collision collision)
    {
        if (isClick)
            return;

        checkImage.enabled = true;
        OnClick?.Invoke(ResetButton);
        isClick = true;

        // Invoke("ResetButton", 2.0f);
    }

    void ResetButton()
    {
        checkImage.enabled = false;
        isClick = false;
    }

}
