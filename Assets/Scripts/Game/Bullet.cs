using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;

    float lifeTime;         // 생존 시간.
    float moveSpeed;        // 이동 속도.
    Vector3 direction;      // 이동 방향.

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        Destroy(gameObject);
    }

    public void Shoot(float lifeTime, float moveSpeed, Vector3 direction)
    {
        this.lifeTime = lifeTime;
        this.moveSpeed = moveSpeed;
        this.direction = direction;

        rigid.AddForce(direction * moveSpeed, ForceMode.Impulse);
    }
}
