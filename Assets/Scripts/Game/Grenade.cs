using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] float delay = 3.0f;
    [SerializeField] float explodeRadius;               // 폭발 반경.
    [SerializeField] float explodeForce;                // 힘.

    bool isLock = true;

    new Rigidbody rigidbody;
    float countDown = 0.0f;

    void Update()
    {
        if (isLock)
            return;

        // 일정 시간이 흐른 뒤 폭발.
        if((countDown -= Time.deltaTime) <= 0.0f)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    public void Setup()
    {
        countDown = delay;
        rigidbody = GetComponent<Rigidbody>();

        isLock = false;
    }

    public void Throw(Vector3 direction, float power)
    {
        rigidbody.AddForce(direction * power, ForceMode.Impulse);
    }
    private void Explode()
    {
        // 폭발 이펙트 재생.
        ParticleSystem effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        effect.Play();

        // 부서지는 오브젝트 검색.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (Collider collider in colliders)
        {
            DestructObject dest = collider.GetComponent<DestructObject>();
            if (dest != null)
            {
                dest.OnDestruct();
            }
        }


        // 폭발 반경의 오브젝트의 움직임.
        colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(explodeForce, transform.position, explodeRadius);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.2f);
        Gizmos.DrawSphere(transform.position, explodeRadius);
    }

}
