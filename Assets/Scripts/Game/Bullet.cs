using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] GameObject bulletHolePrefab;
    [SerializeField] LayerMask exceptMask;          // ���� ����ũ.(�浹 ó��X)

    float lifeTime;         // ���� �ð�.
    float moveSpeed;        // �̵� �ӵ�.
    Vector3 direction;      // �̵� ����.

    float destroyTime;      // �����Ǵ� �ð�.

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");

        // collision.transform.position�� �浹�� ������Ʈ�� ���� ��ġ (X)
        // collision.contacts[0].point�� �浹�� ����.

        // �浹 ������Ʈ�� ���̾ ���� ����ũ�� �ɸ��� �����Ѵ�.
        if ((exceptMask & (1 << collision.gameObject.layer)) != 0)
            return;

        GameObject bulletHole = Instantiate(bulletHolePrefab);                

        bulletHole.transform.position = transform.position;
        bulletHole.transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);
        bulletHole.transform.SetParent(collision.transform);

        // ��밡 �Ѿ˰� ��ȣ�ۿ��ϴ� ��ü�� ���.
        ITarget target = collision.gameObject.GetComponent<ITarget>();
        if(target != null)
        {
            // �¾Ҵٰ� �˷���.
            target.OnHit();
        }

        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger : " + other.name);

        // ��밡 �Ѿ˰� ��ȣ�ۿ��ϴ� ��ü�� ���.
        ITarget target = other.gameObject.GetComponent<ITarget>();
        if (target != null)
        {
            // �¾Ҵٰ� �˷���.
            target.OnHit();
        }

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
