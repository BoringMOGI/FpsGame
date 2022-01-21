using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static RangeManager.RangeInfo; // �ش� ���� ����.

public class SelectedButton : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] Color selectedColor;           // ���� ����.
    [SerializeField] Color deselectedColor;         // ���� ����.

    INFO_TYPE infoType;
    public delegate void OnSendValueEvent(INFO_TYPE type, int index);
    private OnSendValueEvent onSendValue;

    private void Start()
    {
        for(int i = 0; i<buttons.Length; i++)
        {
            Button button = buttons[i];
            buttons[i].onClick.AddListener(() => {
                OnSelectButton(button);
            });
        }
    }

    public void Setup(INFO_TYPE infoType, int firstIndex, OnSendValueEvent onSendValue)
    {
        this.infoType = infoType;
        this.onSendValue = onSendValue;

        OnSelectButton(buttons[firstIndex]);
    }
    public void Setup(INFO_TYPE infoType, bool isSelected, OnSendValueEvent onSendValue)
    {
        Setup(infoType, (isSelected ? 0 : 1), onSendValue);
    }

    private void OnSelectButton(Button target)
    {
        // Array.IndexOf(Array<T>, T)
        //  = ���� �ش� �迭���� �� ��°�ΰ�?
        int index = System.Array.IndexOf(buttons, target);

        for(int i = 0; i< buttons.Length; i++)
        {
            // ������ ��ư�� ��� ���� ����. �ƴϸ� ���� �������� ����.
            buttons[i].GetComponent<Image>().color = 
                (buttons[i] == target) ? selectedColor : deselectedColor;
        }

        // ��������Ʈ�� ���� �� ����.
        onSendValue?.Invoke(infoType, index);       
    }
}
