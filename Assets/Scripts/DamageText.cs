using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static DamageManager;

public class DamageText : MonoBehaviour, ObjectPool<DamageText>.IPool
{
    static Camera mainCam;
    [SerializeField] TextMesh damageText;

    public void Appear(Vector3 position, int amount, DAMAGE_TYPE type)
    {
        damageText.text = amount.ToString();
        StartCoroutine(FixPosition(position));
    }
    IEnumerator FixPosition(Vector3 position)
    {
        //Vector2 damagePosition = mainCam.WorldToScreenPoint(position);
        transform.position = position;

        float showTime = 1.0f;
        yield return new WaitForSeconds(showTime);

        OnReturnPool(this);
    }

    // 인터페이스 구현.
    ObjectPool<DamageText>.OnReturnPoolEvent OnReturnPool;
    public void Setup(ObjectPool<DamageText>.OnReturnPoolEvent OnReturnPool)
    {
        this.OnReturnPool = OnReturnPool;

        mainCam = Camera.main;
    }
}