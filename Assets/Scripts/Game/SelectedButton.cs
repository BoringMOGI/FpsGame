using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static RangeManager.RangeInfo; // 해당 지역 포함.

public class SelectedButton : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] Color selectedColor;           // 선택 색상.
    [SerializeField] Color deselectedColor;         // 비선택 색상.

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
        //  = 값이 해당 배열에서 몇 번째인가?
        int index = System.Array.IndexOf(buttons, target);

        for(int i = 0; i< buttons.Length; i++)
        {
            // 선택한 버튼일 경우 선택 색상. 아니면 비선택 색상으로 변경.
            buttons[i].GetComponent<Image>().color = 
                (buttons[i] == target) ? selectedColor : deselectedColor;
        }

        // 델리게이트를 통해 값 전달.
        onSendValue?.Invoke(infoType, index);       
    }
}
