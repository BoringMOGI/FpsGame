using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTarget : HitManager
{
    [SerializeField] Animation anim;
    [SerializeField] new Collider collider;
    [SerializeField] ParticleSystem hitBlood;


    const string KEY_STAND = "Range_Stand";         // �Ͼ�� �ִϸ��̼� Ű.
    const string KEY_FALL = "Range_Exit";           // �������� �ִϸ��̼� Ű.
    

    // ��������Ʈ : �Լ��� ���� �� �ִ� ����.
    // event�� ��������Ʈ : �ܺο��� ȣ��(x) �̺�Ʈ�� ���(O)
    public delegate void HitEvent();
    public event HitEvent OnHitEvent;               // ���� �¾��� �� ��ϵ� �̺�Ʈ ȣ��.

    bool isAlive = false;       // ���� ����.
    float showTime = 0;         // ���� �ð�.


    int hp;         // ü��.
    int armor;      // �� ��ȣ��.
    int helmet;     // �Ӹ� ��ȣ��.

    // �ʱ�ȭ �Լ�. (������ �� �ҷ��� �Ѵ�.)
    public void Setup(float showTime, bool isArmor)
    {
        isAlive = true;

        this.showTime = showTime;           // ���� ���� �ð�. 

        hp = 100;
        armor = isArmor ? 100 : 0;          // �� ���� �� ��ź���� �������� 100���� ����.
        helmet= isArmor ? 100 : 0;          // �� ���� �� ����� �������� 100���� ����.

        LookRotation();


        // armorObject.SetActive(isArmor);  // isArmor�� ���� ���� �� ������Ʈ Ȱ��ȭ.
        anim.Play(KEY_STAND);               // �Ͼ�� �ִϸ��̼� ���.
    }

    void LookRotation()
    {
        // �� ���� ��븦 ���ϴ� ���� ���.
        Vector3 direction = PlayerController.Instance.transform.position - transform.position;
        direction.Normalize();

        // ���͸� ȸ�� ��(�����)�� ����.
        Quaternion look = Quaternion.LookRotation(direction);
        transform.rotation = look;

        // ȸ�������� x�� z���� ���� 0�� �����.
        Vector3 euler = transform.eulerAngles;
        euler.x = 0;
        euler.z = 0;
        transform.eulerAngles = euler;
    }

    void Update()
    {
        if (!isAlive)
            return;

        // ���� Ÿ�� üũ.
        showTime -= Time.deltaTime;
        if (showTime <= 0.0f)
            OnFallDown();
    }

    // ���� ��������. (�ð��� �Ǿ ��������.)
    private void OnFallDown()
    {
        isAlive = false;
        collider.enabled = false;
        anim.Play(KEY_FALL);
    }

    public override void OnHit(HitInfo info)
    {
        bool isHelmet = helmet > 0;
        bool isArmor = armor > 0;
        int damage = info.damage;

        switch(info.hitbox)
        {
            case BODY.Head:
                if (isHelmet)
                {
                    damage = Mathf.RoundToInt(damage * 1.65f);
                    helmet -= damage;
                }
                else
                    damage = Mathf.RoundToInt(damage * 2.35f);
                break;

            case BODY.Body:
                if (isArmor)
                {
                    damage = Mathf.RoundToInt(damage * 0.7f);
                    armor -= damage;
                }
                else
                    damage = Mathf.RoundToInt(damage * 1.0f);
                break;

            case BODY.Leg:
                damage = Mathf.RoundToInt(damage * 0.3f);
                break;

            case BODY.Arm:
                damage = Mathf.RoundToInt(damage * 0.1f);
                break;                            
        }

        // ���� ����Ʈ ����.
        ParticleSystem blood = Instantiate(hitBlood);
        blood.transform.position = info.hitPoint;
        blood.transform.eulerAngles = info.hitRotation;

        blood.Play();

        // ������ ����Ʈ ����.
        DamageManager.Instance.AppearDamage(info.hitPoint, damage);

        // ü�� ����.
        if ((hp -= damage) <= 0)
        {
            OnHitEvent?.Invoke();
            OnFallDown();
        }
    }
    public void OnEndHit()
    {
        Destroy(gameObject);
    }
}
