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
    public struct HitInfo
    {
        public BODY hitbox;
        public int damage;
        public Vector3 hitPoint;
        public Vector3 hitRotation;
    }

    public virtual void OnHit(HitInfo hitInfo)
    {

    }

}
