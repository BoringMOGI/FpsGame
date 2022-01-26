using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    public enum BODY
    {
        Head,       // ¸Ó¸®
        Body,       // ¸öÅë
        Arm,        // ÆÈ
        Leg,        // ´Ù¸®
    }

    private void Start()
    {
        
    }

    public virtual void OnHit(BODY body)
    {

    }

}
