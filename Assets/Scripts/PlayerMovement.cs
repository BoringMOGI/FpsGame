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
    [SerializeField] Transform groundChecker;       // 지면 체크 기준점.
    [SerializeField] float groundRadius;            // 지면 체크 반지름.
    [SerializeField] LayerMask groundMask;          // 지면 레이어 마스크.

    
    CharacterController controller;         // 캐릭터 제어 컴포넌트.

    bool isDead;                            // 죽었는가?

    // 애니메이터 파라미터를 이용한다.
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

    float gravity => GRAVITY * gravityScale; // 중력 가속도 * 중력 비율.

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        CheckGround();          // ground 체크.

        if (!isDead)
        {
            Movement();         // 이동.
            Jump();             // 점프.
        }

        Gravity();              // 중력 값.
    }

    private void CheckGround()
    {
        isGround = Physics.CheckSphere(groundChecker.position, groundRadius, groundMask);
    }
    private void Movement()
    {
        // Input.GetAxisRaw = -1, 0  1.
        // Input.GetAxis = -1.0f ~ 1.0f.
        inputX = Input.GetAxis("Horizontal");       // 키보드 좌,우 (좌측,우측)
        inputY = Input.GetAxis("Vertical");         // 키보드 상,하 (정면,후면)

        // transform.방향 => 내 기준 방향 (로컬 좌표)
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
        if (isGround && velocityY < 0f)          // 땅을 밟았고 하강 속력이 있다면.
        {
            velocityY = -2f;                       // 최소한의 값으로 속력 대입.
        }

        velocityY += gravity * Time.deltaTime;
        controller.Move(new Vector3(0f, velocityY, 0f) * Time.deltaTime);

        anim.SetFloat("velocityY", velocityY);     // 애니메이터의 파리미터를 갱신.
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
