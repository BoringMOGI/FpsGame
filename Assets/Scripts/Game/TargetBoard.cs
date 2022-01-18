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

    void Start()
    {
        anim.Play(KEY_STAND);
    }

    public void OnHit()
    {
        anim.Play(KEY_HIT);
        collider.enabled = false;

        Invoke("OnStand", 3.0f);
    }

    private void OnStand()
    {
        anim.Play(KEY_STAND);
        collider.enabled = true;
    }


}
