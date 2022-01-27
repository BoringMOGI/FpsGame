using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] GameObject bulletHolePrefab;

    float lifeTime;         // 생존 시간.
    float moveSpeed;        // 이동 속도.
    Vector3 direction;      // 이동 방향.

    float destroyTime;      // 삭제되는 시간.

    private void OnCollisionEnter(Collision collision)
    {
        // collision.transform.position은 충돌한 오브젝트의 기준 위치 (X)
        // collision.contacts[0].point는 충돌한 지점.
        GameObject bulletHole = Instantiate(bulletHolePrefab);                
        bulletHole.transform.position = transform.position;
        bulletHole.transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);
        //bulletHole.transform.SetParent(collision.transform);

        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
                // 상대가 총알과 상호작용하는 물체일 경우.
        ITarget target = other.gameObject.GetComponent<ITarget>();
        if (target != null)
        {
            // 데미지 전달.
            target.OnHit(50);
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
