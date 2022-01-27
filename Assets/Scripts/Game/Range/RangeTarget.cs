using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTarget : HitManager
{
    [SerializeField] Animation anim;
    [SerializeField] new Collider collider;
    [SerializeField] GameObject armorObject;

    const string KEY_FALL = "Board_Hit";           // 쓰러지는 애니메이션 키.
    const string KEY_STAND = "Board_Stand";         // 일어나는 애니메이션 키.

    // 델리게이트 : 함수를 담을 수 있는 변수.
    // event형 델리게이트 : 외부에서 호출(x) 이벤트에 등록(O)
    public delegate void HitEvent();
    public event HitEvent OnHitEvent;               // 내가 맞았을 때 등록된 이벤트 호출.

    bool isAlive = false;       // 생존 여부.
    float showTime = 0;         // 등장 시간.


    int hp;         // 체력.
    int armor;      // 몸 보호구.
    int helmet;     // 머리 보호구.

    // 초기화 함수. (시작할 때 불러야 한다.)
    public void Setup(float showTime, bool isArmor)
    {
        isAlive = true;

        this.showTime = showTime;           // 등장 유지 시간. 

        hp = 100;
        armor = isArmor ? 100 : 0;          // 방어구 착용 시 방탄복의 내구도를 100으로 증가.
        helmet= isArmor ? 100 : 0;          // 방어구 착용 시 헬멧의 내구도를 100으로 증가.

        // armorObject.SetActive(isArmor);  // isArmor의 값에 따라 방어구 오브젝트 활성화.
        anim.Play(KEY_STAND);               // 일어나는 애니메이션 재생.
    }

    void Update()
    {
        if (!isAlive)
            return;

        showTime -= Time.deltaTime;
        if (showTime <= 0.0f)
            OnFallDown();
    }

    // 적이 쓰러진다. (시간이 되어서 쓰러진다.)
    private void OnFallDown()
    {
        isAlive = false;
        collider.enabled = false;
        anim.Play(KEY_FALL);
    }

    public override void OnHit(BODY hitbox, int damage)
    {
        base.OnHit(hitbox, damage);

        bool isHelmet = helmet > 0;
        bool isArmor = armor > 0;

        switch(hitbox)
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

        // 체력 저하.
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
