using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] float delay = 3.0f;
    [SerializeField] bool isLock = true;

    new Rigidbody rigidbody;
    float countDown = 0.0f;

    void Start()
    {
        countDown = delay;
        rigidbody = GetComponent<Rigidbody>();
    }

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

    public void Throw(Vector3 direction, float power)
    {
        rigidbody.AddForce(direction * power, ForceMode.Impulse);
    }
    private void Explode()
    {
        // ���� ����Ʈ ���.
        ParticleSystem effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        effect.Play();
    }

}
