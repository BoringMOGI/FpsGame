using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget
{
    void OnHit();
}
public class Hitbox : MonoBehaviour, ITarget
{
    [SerializeField] HitManager hitManager;
    [SerializeField] HitManager.BODY type;

    public void OnHit()
    {
        hitManager.OnHit(type);
    }
}
