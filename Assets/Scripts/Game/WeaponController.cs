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
    [SerializeField] Transform gunMuzzle;       // 총구의 위치.
    [SerializeField] Bullet bulletPrefab;       // 총알의 프리팹.
    [SerializeField] float bulletSpeed;         // 총알의 속도.

    [Header("Sound")]
    [SerializeField] AudioClip fireSE;          // 격발음.
    [SerializeField] AudioClip reloadSE;        // 장전음.

    [Header("Positoin")]
    [SerializeField] Transform aimCameraPivot;  // 조준 카메라 중심점.

    float nextFireTime = 0f;

    int bulletCount = 0;            // 장전 된 탄약 수.
    int hasBulletCount = 5;         // 소지하고 있는 탄약 수.

    Transform eye;
    LineRenderer lineRenderer;

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
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        UpdateUI();

        Vector3 linePoint = gunMuzzle.position + (gunMuzzle.forward * 100f);
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, gunMuzzle.position);
        lineRenderer.SetPosition(1, linePoint);

        //Physics.Raycast()
    }

    public void Setup(Transform eye)
    {
        this.eye = eye;
    }

    public void Fire()
    {
        if (isReload)
            return;

        // 연사 속도 제어
        if (bulletCount > 0 && nextFireTime <= Time.time)
        {
            nextFireTime = Time.time + fireRate;
            bulletCount--;

            // 탄도 방향 계산.

            // '시선'과 '총구'의 각도 차이를 보상해주기 위해
            // Ray를 이용하여 총알의 목적지 계산.
            Vector3 destination = eye.position + (eye.forward * 1000f);

            RaycastHit hit;
            if (Physics.Raycast(eye.position, eye.forward, out hit, 1000f))
                destination = hit.point;

            // 총알이 나아갈 방향.
            // Normalize() : 값의 정규화. 방향에서 크기를 제거해 정규화를 시킨다.
            Vector3 direction = destination - gunMuzzle.position;
            direction.Normalize();

            // 총알 생성.
            Bullet newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = gunMuzzle.position;
            newBullet.Shoot(5f, bulletSpeed, direction);

            // 애니메이션, 효과음 제어.
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
