using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] float delay = 3.0f;
    [SerializeField] float explodeRadius;               // ���� �ݰ�.
    [SerializeField] float explodeForce;                // ��.

    bool isLock = true;

    new Rigidbody rigidbody;
    float countDown = 0.0f;

    void Update()
    {
        if (isLock)
            return;

        // ���� �ð��� �帥 �� ����.
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
        // ���� ����Ʈ ���.
        ParticleSystem effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        effect.Play();

        // �μ����� ������Ʈ �˻�.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (Collider collider in colliders)
        {
            DestructObject dest = collider.GetComponent<DestructObject>();
            if (dest != null)
            {
                dest.OnDestruct();
            }
        }


        // ���� �ݰ��� ������Ʈ�� ������.
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
