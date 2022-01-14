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
    [SerializeField] Transform gunMuzzle;       // �ѱ��� ��ġ.
    [SerializeField] Bullet bulletPrefab;       // �Ѿ��� ������.
    [SerializeField] float bulletSpeed;         // �Ѿ��� �ӵ�.

    [Header("Sound")]
    [SerializeField] AudioClip fireSE;          // �ݹ���.
    [SerializeField] AudioClip reloadSE;        // ������.

    [Header("Positoin")]
    [SerializeField] Transform aimCameraPivot;  // ���� ī�޶� �߽���.

    float nextFireTime = 0f;

    int bulletCount = 0;            // ���� �� ź�� ��.
    int hasBulletCount = 5;         // �����ϰ� �ִ� ź�� ��.

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

        // ���� �ӵ� ����
        if (bulletCount > 0 && nextFireTime <= Time.time)
        {
            nextFireTime = Time.time + fireRate;
            bulletCount--;

            // ź�� ���� ���.

            // '�ü�'�� '�ѱ�'�� ���� ���̸� �������ֱ� ����
            // Ray�� �̿��Ͽ� �Ѿ��� ������ ���.
            Vector3 destination = eye.position + (eye.forward * 1000f);

            RaycastHit hit;
            if (Physics.Raycast(eye.position, eye.forward, out hit, 1000f))
                destination = hit.point;

            // �Ѿ��� ���ư� ����.
            // Normalize() : ���� ����ȭ. ���⿡�� ũ�⸦ ������ ����ȭ�� ��Ų��.
            Vector3 direction = destination - gunMuzzle.position;
            direction.Normalize();

            // �Ѿ� ����.
            Bullet newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = gunMuzzle.position;
            newBullet.Shoot(5f, bulletSpeed, direction);

            // �ִϸ��̼�, ȿ���� ����.
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
