using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget
{
    void OnHit(int damage);
}
public class Hitbox : MonoBehaviour, ITarget
{
    [SerializeField] HitManager hitManager;
    [SerializeField] HitManager.BODY type;

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Hitbox");
    }
    public void OnHit(int damage)
    {
        hitManager.OnHit(type, damage);
    }
}
