using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] GameObject bulletHolePrefab;

    float lifeTime;         // ���� �ð�.
    float moveSpeed;        // �̵� �ӵ�.
    Vector3 direction;      // �̵� ����.

    float destroyTime;      // �����Ǵ� �ð�.

    private void OnCollisionEnter(Collision collision)
    {
        GameObject bulletHole = Instantiate(bulletHolePrefab);

        // collision.transform.position�� �浹�� ������Ʈ�� ���� ��ġ (X)
        // collision.contacts[0].point�� �浹�� ����.

        //bulletHole.transform.position = collision.transform.position;
        bulletHole.transform.position = transform.position;
        bulletHole.transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);

        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (destroyTime <= Time.time)
            Destroy(gameObject);
    }

    public void Shoot(float lifeTime, float moveSpeed, Vector3 direction)
    {
        this.lifeTime = lifeTime;
        this.moveSpeed = moveSpeed;
        this.direction = direction;

        // rigid.AddForce(direction * moveSpeed, ForceMode.Impulse);
        rigid.velocity = direction * moveSpeed;

        destroyTime = Time.time + lifeTime;
    }
}
