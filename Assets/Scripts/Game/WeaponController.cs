using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] float fireRate;            // ���� �ӵ�.

    [Header("Bullet")]
    [SerializeField] Transform bulletHole;      // �ѱ��� ��ġ.
    [SerializeField] Bullet bulletPrefab;       // �Ѿ��� ������.
    [SerializeField] float bulletSpeed;         // �Ѿ��� �ӵ�.

    float nextFireTime = 0f;

    public void Fire()
    {
        // ���� �ӵ� ����.
        if (nextFireTime <= Time.time)
        {
            nextFireTime = Time.time + fireRate;

            // ����Ʈ.

            Bullet newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = bulletHole.position;
            newBullet.Shoot(5f, bulletSpeed, bulletHole.forward);

            anim.SetTrigger("onFire");
            AudioManager.Instance.PlayEffect("shoot");
        }        
    }
}
