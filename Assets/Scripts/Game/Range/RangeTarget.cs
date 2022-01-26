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

    bool isAlive = false;        // 생존 여부.
    float showTime = 0;         // 등장 시간.
    int armorDefence = 0;       // 방어도.

    // 초기화 함수. (시작할 때 불러야 한다.)
    public void Setup(float showTime, bool isArmor)
    {
        isAlive = true;

        this.showTime = showTime;           // 등장 유지 시간. 
        armorDefence = isArmor ? 3 : 0;     // 방어구 착용 여부에 따라 3 또는 0의 값을 대입.

        // armorObject.SetActive(isArmor);     // isArmor의 값에 따라 방어구 오브젝트 활성화.
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

    public override void OnHit(BODY body)
    {
        base.OnHit(body);

        Debug.Log("Hit : " + body);

        switch(body)
        {
            case BODY.Head:
                break;

            case BODY.Leg:
                break;

            case BODY.Arm:
                break;

            case BODY.Body:
                break;
        }

        OnFallDown();
    }
    public void OnEndHit()
    {
        Destroy(gameObject);
    }
}
