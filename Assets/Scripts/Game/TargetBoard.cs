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

    const string KEY_HIT = "Board_Hit";
    const string KEY_STAND = "Board_Stand";

    float countdown = 0;

    // 초기화 함수.
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
    }

    public void OnEndHit()
    {
        Destroy(gameObject);
    }
}
