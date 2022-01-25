using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget
{
    void OnHit();
}

public class TargetBoard : MonoBehaviour, ITarget
{
    [SerializeField] Animation anim;
    [SerializeField] new Collider collider;
    [SerializeField] GameObject armorObject;

    // ��������Ʈ : �Լ��� ���� �� �ִ� ����.
    // event�� ��������Ʈ : �ܺο��� ȣ��(x) �̺�Ʈ�� ���(O)
    public delegate void HitEvent();
    public event HitEvent OnHitEvent;               // ���� �¾��� �� ��ϵ� �̺�Ʈ ȣ��.

    const string KEY_FALL  = "Board_Hit";           // �������� �ִϸ��̼� Ű.
    const string KEY_STAND = "Board_Stand";         // �Ͼ�� �ִϸ��̼� Ű.


    bool isAlive = true;        // ���� ����.
    float showTime = 0;         // ���� �ð�.
    int armorDefence = 0;       // ��.

    // �ʱ�ȭ �Լ�. (������ �� �ҷ��� �Ѵ�.)
    public void Setup(float showTime, bool isArmor)
    {
        isAlive = true;

        this.showTime = showTime;           // ���� ���� �ð�. 
        armorDefence = isArmor ? 3 : 0;     // �� ���� ���ο� ���� 3 �Ǵ� 0�� ���� ����.

        armorObject.SetActive(isArmor);     // isArmor�� ���� ���� �� ������Ʈ Ȱ��ȭ.
        anim.Play(KEY_STAND);               // �Ͼ�� �ִϸ��̼� ���.
    }

    void Update()
    {
        if (!isAlive)
            return;

        showTime -= Time.deltaTime;
        if(showTime <= 0.0f)
            OnFallDown();
    }

    // ���� ��������. (�ð��� �Ǿ ��������.)
    private void OnFallDown()
    {
        isAlive = false;
        collider.enabled = false;
        anim.Play(KEY_FALL);
    }

    // �Ѿ˿� �¾Ƽ� ��������.
    public void OnHit()
    {
        if (armorDefence > 0)       // ���� ���� �����ִٸ�.
        {
            armorDefence -= 1;      // �� 1 ����.
        }
        else                        // ���� ���ٸ�,
        {
            OnHitEvent?.Invoke();   // ���� �ø���.
            OnFallDown();
        }
    }
    public void OnEndHit()
    {
        Destroy(gameObject);
    }
    

}
