using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    enum FIRE_TYPE
    {
        Single,     // �ܹ�.
        // Burst,      // ����.
        Auto,       // ����.

        Count,      // ����.
    }

    [SerializeField] Animator anim;

    [Header("Gun")]
    [SerializeField] float fireRate;            // ���� �ӵ�.
    [SerializeField] int maxBulletCount;        // �ִ� ���� ź�� ��.
    [SerializeField] Vector2 recoil;            // �ѱ� �ݵ�.

    [Header("Bullet")]
    [SerializeField] Transform gunMuzzle;       // �ѱ��� ��ġ.
    [SerializeField] Bullet bulletPrefab;       // �Ѿ��� ������.
    [SerializeField] float bulletSpeed;         // �Ѿ��� �ӵ�.

    [Header("Sound")]
    [SerializeField] AudioClip fireSE;          // �ݹ���.
    [SerializeField] AudioClip reloadSE;        // ������.

    [Header("Positoin")]
    [SerializeField] Transform aimCameraPivot;  // ���� ī�޶� �߽���.

    [Header("Grenade")]
    [SerializeField] Grenade grenadePrefab;
    [SerializeField] Transform grenadePivot;

    float nextFireTime = 0f;

    int bulletCount = 0;            // ���� �� ź�� ��.
    int hasBulletCount = 5;         // �����ϰ� �ִ� ź�� ��.

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

    bool isThrowing;        // ��ź�� ������ ���ΰ�?

    // �ִϸ��̼��� ����ϰ� �ִ°�? (= �ٸ� ������ ���� ���ϰ� �ؾ� �Ѵ�.)
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
        // '�ü�'�� '�ѱ�'�� ���� ���̸� �������ֱ� ����
        // Ray�� �̿��Ͽ� �Ѿ��� ������ ���.
        Vector3 destination = eye.position + (eye.forward * 1000f);

        // ��ü ���̾�� ignore�� �����Ѵ�.
        RaycastHit hit;
        LayerMask eyeLayer = int.MaxValue;
        LayerMask ignore = 1 << LayerMask.NameToLayer("NoEntry");

        eyeLayer ^= ignore; // XOR����.

        if (Physics.Raycast(eye.position, eye.forward, out hit, 1000f, eyeLayer))
            destination = hit.point;

        // �Ѿ��� ���ư� ����.
        // Normalize() : ���� ����ȭ. ���⿡�� ũ�⸦ ������ ����ȭ�� ��Ų��.
        Vector3 direction = destination - gunMuzzle.position;
        direction.Normalize();

        return direction;
    }
    private void Recoil()
    {
        // ���� ������ ���� �ִ� �ݵ�.
        float maxRecoilX = recoil.x * (isAim ? 0.6f : 1.0f);
        float maxRecoilY = recoil.y * (isAim ? 0.6f : 1.0f);

        // �ݵ� ����.
        float recoilX = Random.Range(-maxRecoilX, maxRecoilX);
        float recoilY = Random.Range(0, maxRecoilY);
        CameraLook.Instance.AddRecoil(new Vector2(recoilX, recoilY));
    }



    // �ܺ� ����.
    public void Fire(bool isFirst)
    {        
        if (IsAnimating)
            return;

        // isFirst : ���� �Է� ����.
        // �ܹ�, ����.
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
        // ���� �ӵ� ����
        if (bulletCount > 0)
        {
            bulletCount--;

            // �Ѿ� ����.
            Bullet newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = gunMuzzle.position;
            newBullet.Shoot(5f, bulletSpeed, GetBulletDirection());

            // �ִϸ��̼�, ȿ���� ����.
            anim.SetTrigger("onFire");
            AudioManager.Instance.PlayEffect(fireSE);

            // �ѱ� �ݵ�.
            Recoil();
        }
    }

    
    public void ThrowGrenade()
    {
        if (IsAnimating)
            return;

        // �ִϸ��̼� ����.
        anim.SetTrigger("onGrenade");
        isThrowing = true;
    }
    private void OnThrow()
    {
        // ������ ������ �̺�Ʈ.
        // ��ź Ŭ�� ����.
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

        // ���ε� ���� �ƴϸ� ���� ź���� �ִ� ź�ຸ�� ���� ���.
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

        // ���� ������ ���.
        if (isAim == false && _isAim)
            anim.SetTrigger("onAim");

        isAim = _isAim;
    }



    // �̺�Ʈ �Լ�.
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
