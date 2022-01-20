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

    // 델리게이트 : 함수를 담을 수 있는 변수.
    // event형 델리게이트 : 외부에서 호출(x) 이벤트에 등록(O)
    public delegate void HitEvent();
    public event HitEvent OnHitEvent;

    const string KEY_HIT = "Board_Hit";
    const string KEY_STAND = "Board_Stand";

    float countdown = 0;

    public void Setup(float countdown)
    {
        this.countdown = countdown;
        anim.Play(KEY_STAND);
    }


    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void OnHit()
    {
        anim.Play(KEY_HIT);
        collider.enabled = false;

        OnHitEvent?.Invoke();
    }

    public void OnEndHit()
    {
        Destroy(gameObject);
    }
}
