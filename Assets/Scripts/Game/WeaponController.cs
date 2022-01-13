using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Animator anim;

    [Header("Gun")]
    [SerializeField] float fireRate;            // ���� �ӵ�.
    [SerializeField] int maxBulletCount;        // �ִ� ���� ź�� ��.

    [Header("Bullet")]
    [SerializeField] Transform bulletHole;      // �ѱ��� ��ġ.
    [SerializeField] Bullet bulletPrefab;       // �Ѿ��� ������.
    [SerializeField] float bulletSpeed;         // �Ѿ��� �ӵ�.

    [Header("Sound")]
    [SerializeField] AudioClip fireSE;          // �ݹ���.
    [SerializeField] AudioClip reloadSE;        // ������.

    [Header("Positoin")]
    [SerializeField] Transform aimCameraPivot;  // ���� ī�޶� �߽���.

    float nextFireTime = 0f;

    int bulletCount = 0;            // ���� �� ź�� ��.
    int hasBulletCount = 5;        // �����ϰ� �ִ� ź�� ��.

    public Transform AimCameraPivot => aimCameraPivot;

    bool isReload
    {
        get
        {
            return anim.GetBool("isReload");
        }
        set
        {
            anim.SetBool("isReload", value);
        }
    }
    bool isAim
    {
        get
        {
            return anim.GetBool("isAim");
        }
        set
        {
            anim.SetBool("isAim", value);
        }
    }


    private void Start()
    {
        bulletCount = maxBulletCount;
    }
    private void Update()
    {
        UpdateUI();
    }

    public void Fire()
    {
        if (isReload)
            return;

        // ���� �ӵ� ����, xkrdir wpdj.
        if (bulletCount > 0 && nextFireTime <= Time.time)
        {
            nextFireTime = Time.time + fireRate;
            bulletCount--;

            // ����Ʈ.

            Bullet newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = bulletHole.position;
            newBullet.Shoot(5f, bulletSpeed, bulletHole.forward);

            anim.SetTrigger("onFire");
            AudioManager.Instance.PlayEffect(fireSE);
        }        
    }
    public void Realod()
    {
        if (isReload)
            return;

        // ���ε� ���� �ƴϸ� ���� ź���� �ִ� ź�ຸ�� ���� ���.
        if (bulletCount < maxBulletCount && hasBulletCount > 0)
        {
            isReload = true;

            anim.SetTrigger("onReload");
            AudioManager.Instance.PlayEffect(reloadSE);
        }
    }
    public void Aim(bool _isAim)
    {
        // ���� ������ ���.
        if (isAim == false && _isAim)
            anim.SetTrigger("onAim");

        isAim = _isAim;
    }

    private void OnEndReload()
    {
        isReload = false;

        int need = maxBulletCount - bulletCount;        // �䱸ġ.
        if(hasBulletCount < need)                       // ���� �ʿ��� ������ ���� ������ ���� ���.
        {
            bulletCount += hasBulletCount;              // ���� ���� �䱸ġ���� ��� ���� ���ϱ�.
            hasBulletCount = 0;                         // ���� ���� 0.
        }
        else
        {
            bulletCount += need;                        // �ʿ��� ����ŭ ���� �� �ø�.
            hasBulletCount -= need;                     // �ʿ��� ����ŭ ���� ź�࿡�� ����.
        }
    }

    private void UpdateUI()
    {
        WeaponUI ui = WeaponUI.Instance;

        ui.SetBulletCount(bulletCount, hasBulletCount);
    }
}
