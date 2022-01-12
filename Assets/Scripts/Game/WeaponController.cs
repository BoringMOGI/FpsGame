using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] float fireRate;            // 연사 속도.

    [Header("Bullet")]
    [SerializeField] Transform bulletHole;      // 총구의 위치.
    [SerializeField] Bullet bulletPrefab;       // 총알의 프리팹.
    [SerializeField] float bulletSpeed;         // 총알의 속도.

    float nextFireTime = 0f;

    public void Fire()
    {
        // 연사 속도 제어.
        if (nextFireTime <= Time.time)
        {
            nextFireTime = Time.time + fireRate;

            // 이펙트.

            Bullet newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = bulletHole.position;
            newBullet.Shoot(5f, bulletSpeed, bulletHole.forward);

            anim.SetTrigger("onFire");
            AudioManager.Instance.PlayEffect("shoot");
        }        
    }
}
