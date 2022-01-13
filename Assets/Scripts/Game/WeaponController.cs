using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Animator anim;

    [Header("Gun")]
    [SerializeField] float fireRate;            // 연사 속도.
    [SerializeField] int maxBulletCount;        // 최대 장전 탄약 수.

    [Header("Bullet")]
    [SerializeField] Transform bulletHole;      // 총구의 위치.
    [SerializeField] Bullet bulletPrefab;       // 총알의 프리팹.
    [SerializeField] float bulletSpeed;         // 총알의 속도.

    [Header("Sound")]
    [SerializeField] AudioClip fireSE;          // 격발음.
    [SerializeField] AudioClip reloadSE;        // 장전음.

    [Header("Positoin")]
    [SerializeField] Transform aimCameraPivot;  // 조준 카메라 중심점.

    float nextFireTime = 0f;

    int bulletCount = 0;            // 장전 된 탄약 수.
    int hasBulletCount = 5;        // 소지하고 있는 탄약 수.

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

        // 연사 속도 제어, xkrdir wpdj.
        if (bulletCount > 0 && nextFireTime <= Time.time)
        {
            nextFireTime = Time.time + fireRate;
            bulletCount--;

            // 이펙트.

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

        // 리로딩 중이 아니며 장전 탄약이 최대 탄약보다 적을 경우.
        if (bulletCount < maxBulletCount && hasBulletCount > 0)
        {
            isReload = true;

            anim.SetTrigger("onReload");
            AudioManager.Instance.PlayEffect(reloadSE);
        }
    }
    public void Aim(bool _isAim)
    {
        // 새로 눌렸을 경우.
        if (isAim == false && _isAim)
            anim.SetTrigger("onAim");

        isAim = _isAim;
    }

    private void OnEndReload()
    {
        isReload = false;

        int need = maxBulletCount - bulletCount;        // 요구치.
        if(hasBulletCount < need)                       // 내가 필요한 수보다 적게 가지고 있을 경우.
        {
            bulletCount += hasBulletCount;              // 소지 수가 요구치보다 적어서 전부 더하기.
            hasBulletCount = 0;                         // 소지 수는 0.
        }
        else
        {
            bulletCount += need;                        // 필요한 수만큼 장전 수 올림.
            hasBulletCount -= need;                     // 필요한 수만큼 소지 탄약에서 제거.
        }
    }

    private void UpdateUI()
    {
        WeaponUI ui = WeaponUI.Instance;

        ui.SetBulletCount(bulletCount, hasBulletCount);
    }
}
