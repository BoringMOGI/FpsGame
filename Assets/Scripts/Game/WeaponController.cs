using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    enum FIRE_TYPE
    {
        Single,     // 단발.
        // Burst,      // 점사.
        Auto,       // 연발.

        Count,      // 개수.
    }

    [SerializeField] Animator anim;

    [Header("Gun")]
    [SerializeField] float fireRate;            // 연사 속도.
    [SerializeField] int maxBulletCount;        // 최대 장전 탄약 수.
    [SerializeField] Vector2 recoil;            // 총기 반동.

    [Header("Bullet")]
    [SerializeField] Transform gunMuzzle;       // 총구의 위치.
    [SerializeField] Bullet bulletPrefab;       // 총알의 프리팹.
    [SerializeField] float bulletSpeed;         // 총알의 속도.

    [Header("Sound")]
    [SerializeField] AudioClip fireSE;          // 격발음.
    [SerializeField] AudioClip reloadSE;        // 장전음.

    [Header("Positoin")]
    [SerializeField] Transform aimCameraPivot;  // 조준 카메라 중심점.

    [Header("Grenade")]
    [SerializeField] Grenade grenadePrefab;
    [SerializeField] Transform grenadePivot;

    float nextFireTime = 0f;

    int bulletCount = 0;            // 장전 된 탄약 수.
    int hasBulletCount = 5;         // 소지하고 있는 탄약 수.

    FIRE_TYPE fireType;
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

    bool isThrowing;        // 폭탄을 던지는 중인가?

    // 애니메이션을 재생하고 있는가? (= 다른 동작을 하지 못하게 해야 한다.)
    bool IsAnimating => isReload || isThrowing;


    private void Start()
    {
        fireType = FIRE_TYPE.Single;

        bulletCount = maxBulletCount;
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        UpdateUI();
    }

    public void Setup(Transform eye)
    {
        this.eye = eye;
    }

    private Vector3 GetBulletDirection()
    {
        // '시선'과 '총구'의 각도 차이를 보상해주기 위해
        // Ray를 이용하여 총알의 목적지 계산.
        Vector3 destination = eye.position + (eye.forward * 1000f);

        // 전체 레이어에서 ignore만 제외한다.
        RaycastHit hit;
        LayerMask eyeLayer = int.MaxValue;
        LayerMask ignore = 1 << LayerMask.NameToLayer("NoEntry");

        eyeLayer ^= ignore; // XOR연산.

        if (Physics.Raycast(eye.position, eye.forward, out hit, 1000f, eyeLayer))
            destination = hit.point;

        // 총알이 나아갈 방향.
        // Normalize() : 값의 정규화. 방향에서 크기를 제거해 정규화를 시킨다.
        Vector3 direction = destination - gunMuzzle.position;
        direction.Normalize();

        return direction;
    }
    private void Recoil()
    {
        // 조준 유무에 따른 최대 반동.
        float maxRecoilX = recoil.x * (isAim ? 0.6f : 1.0f);
        float maxRecoilY = recoil.y * (isAim ? 0.6f : 1.0f);

        // 반동 전달.
        float recoilX = Random.Range(-maxRecoilX, maxRecoilX);
        float recoilY = Random.Range(0, maxRecoilY);
        CameraLook.Instance.AddRecoil(new Vector2(recoilX, recoilY));
    }



    // 외부 조작.
    public void Fire(bool isFirst)
    {        
        if (IsAnimating)
            return;

        // isFirst : 최초 입력 여부.
        // 단발, 점사.
        switch(fireType)
        {
            case FIRE_TYPE.Single:

                if (isFirst)
                    Fire();

                break;

            case FIRE_TYPE.Auto:

                if (nextFireTime <= Time.time)
                {
                    nextFireTime = Time.time + fireRate;
                    Fire();
                }

                break;
        }                    
    }
    private void Fire()
    {
        // 연사 속도 제어
        if (bulletCount > 0)
        {
            bulletCount--;

            // 총알 생성.
            Bullet newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = gunMuzzle.position;
            newBullet.Shoot(5f, bulletSpeed, GetBulletDirection());

            // 애니메이션, 효과음 제어.
            anim.SetTrigger("onFire");
            AudioManager.Instance.PlayEffect(fireSE);

            // 총기 반동.
            Recoil();
        }
    }

    
    public void ThrowGrenade()
    {
        if (IsAnimating)
            return;

        // 애니메이션 제어.
        anim.SetTrigger("onGrenade");
        isThrowing = true;
    }
    private void OnThrow()
    {
        // 실제로 던지는 이벤트.
        // 폭탄 클론 생성.
        Grenade grenade = Instantiate(grenadePrefab);
        grenade.transform.position = grenadePivot.position;
        grenade.transform.rotation = grenadePivot.rotation;

        grenade.Setup();
        grenade.Throw(eye.forward, 20f);
    }

    public void Realod()
    {
        if (IsAnimating)
            return;

        // 리로딩 중이 아니며 장전 탄약이 최대 탄약보다 적을 경우.
        if (bulletCount < maxBulletCount && hasBulletCount > 0)
        {
            isReload = true;

            anim.SetTrigger("onReload");
            AudioManager.Instance.PlayEffect(reloadSE);
        }
    }
    public void ChangeFireType()
    {
        if (isThrowing)
            return;

        fireType += 1;
        if (fireType >= FIRE_TYPE.Count)
            fireType = 0;
    }

    public void Aim(bool _isAim)
    {
        if (IsAnimating)
            return;

        // 새로 눌렸을 경우.
        if (isAim == false && _isAim)
            anim.SetTrigger("onAim");

        isAim = _isAim;
    }



    // 이벤트 함수.
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
    private void OnEndThrow()
    {
        isThrowing = false;
    }

    private void UpdateUI()
    {
        WeaponUI ui = WeaponUI.Instance;

        ui.SetBulletCount(bulletCount, hasBulletCount);
    }

}
