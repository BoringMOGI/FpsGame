using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    public enum BODY
    {
        Head,       // �Ӹ�
        Body,       // ����
        Arm,        // ��
        Leg,        // �ٸ�
    }

    private void Start()
    {
        
    }

    public virtual void OnHit(BODY body)
    {

    }

}
