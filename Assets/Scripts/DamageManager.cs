using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : ObjectPool<DamageText>
{
    static DamageManager instnace;
    public static DamageManager Instance => instnace;

    private new void Awake()
    {
        base.Awake();
        instnace = this;
    }

    public void AppearDamage(Vector3 position, int amount, DAMAGE_TYPE type = DAMAGE_TYPE.Normal)
    {
        DamageText pool = GetPool();            
        pool.Appear(position, amount, type);    
    }
}