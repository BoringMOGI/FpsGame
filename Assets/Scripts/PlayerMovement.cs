using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void OnDamaged(float amount, DAMAGE_TYPE type = DAMAGE_TYPE.Normal);
}
public enum DAMAGE_TYPE
{
    Normal,
    Critical,
}

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    const float GRAVITY = -9.81f;

    [Header("Movement")]
    [SerializeField] Animator anim;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;
    [Range(1.0f, 4.0f)]
    [SerializeField] float gravityScale;

    [Header("Ground")]
    [SerializeField] Transform groundChecker;       // ���� üũ ������.
    [SerializeField] float groundRadius;            // ���� üũ ������.
    [SerializeField] LayerMask groundMask;          // ���� ���̾� ����ũ.

    
    CharacterController controller;         // ĳ���� ���� ������Ʈ.

    bool isDead;                            // �׾��°�?

    // �ִϸ����� �Ķ���͸� �̿��Ѵ�.
    float velocityY
    {
        get
        {
            return anim.GetFloat("velocityY");
        }
        set
        {
            anim.SetFloat("velocityY", value);
        }
    }
    bool isGround
    {
        get
        {
            return anim.GetBool("isGround");
        }
        set
        {
            anim.SetBool("isGround", value);
        }
    }
    float inputX
    {
        get
        {
            return anim.GetFloat("inputX");
        }
        set
        {
            anim.SetFloat("inputX", value);
        }
    }
    float inputY
    {
        get
        {
            return anim.GetFloat("inputY");
        }
        set
        {
            anim.SetFloat("inputY", value);
        }
    }

    float gravity => GRAVITY * gravityScale; // �߷� ���ӵ� * �߷� ����.

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        CheckGround();          // ground üũ.

        if (!isDead)
        {
            Movement();         // �̵�.
            Jump();             // ����.
        }

        Gravity();              // �߷� ��.
    }

    private void CheckGround()
    {
        isGround = Physics.CheckSphere(groundChecker.position, groundRadius, groundMask);
    }
    private void Movement()
    {
        // Input.GetAxisRaw = -1, 0  1.
        // Input.GetAxis = -1.0f ~ 1.0f.
        inputX = Input.GetAxis("Horizontal");       // Ű���� ��,�� (����,����)
        inputY = Input.GetAxis("Vertical");         // Ű���� ��,�� (����,�ĸ�)

        // transform.���� => �� ���� ���� (���� ��ǥ)
        Vector3 direction = (transform.right * inputX) + (transform.forward * inputY);
        controller.Move(direction * moveSpeed * Time.deltaTime);
    }
    private void Jump()
    {
        if (isGround && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetTrigger("onJump");
        }
    }
    private void Gravity()
    {
        if (isGround && velocityY < 0f)          // ���� ��Ұ� �ϰ� �ӷ��� �ִٸ�.
        {
            velocityY = -2f;                       // �ּ����� ������ �ӷ� ����.
        }

        velocityY += gravity * Time.deltaTime;
        controller.Move(new Vector3(0f, velocityY, 0f) * Time.deltaTime);

        anim.SetFloat("velocityY", velocityY);     // �ִϸ������� �ĸ����͸� ����.
    }

    private void OnDrawGizmos()
    {
        if (groundChecker != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundChecker.position, groundRadius);
        }
    }

}