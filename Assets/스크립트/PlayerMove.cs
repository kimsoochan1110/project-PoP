using System.Collections;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Delay delay; //딜레이 스크립트 참조
                        //일반공격

    //점프아래공격


    public PlayerActinfo playerAttackinfo;
    public HitboxController hitboxController;
    public HurtboxController hurtboxController;
    private DashController dashController; //대쉬 스크립트 참조
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public float maxSpeed; // 최대속도
    public float forceAmount; //가속정도

    public bool isAttacking = false; // 공격 중인지 여부
    public bool lastFlipX; // 이전 방향 저장

    public float jumpPower; //점프 파워
    public int jumpCount = 0; //점프 카운트
    public float jumpTime = 0f;  // 점프 버튼 누른 시간
    public int maxJumpCount = 2; //최대 점프 카운트
    public float jumpInputBufferTime = 0.5f;
    public float jumpInputBuffer = 0f;//점프버퍼
    public bool isJumping = false; // 점프 중인지 여부

    private float dashCooldownTimer = 0f; // 쿨타임 타이머 변수 추가
    public float dashCooldown = 0.5f; //대쉬 쿨타임
    public float dashBufferTime = 0.5f;
    public float dashInputBuffer = 0f;//대쉬버퍼
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitboxController = GetComponentInChildren<HitboxController>();
        hurtboxController = GetComponentInChildren<HurtboxController>();
        dashController = GetComponent<DashController>();
        delay = GetComponentInChildren<Delay>();
        animator = GetComponent<Animator>();
        playerAttackinfo = GetComponent<PlayerActinfo>();
        lastFlipX = spriteRenderer.flipX; // 초기 방향 저장
    }

    private void Update()
    {
        float vertical = Input.GetAxisRaw("Vertical");

        // 점프 입력 버퍼 처리
        if (Input.GetButtonDown("Jump"))
        {
            jumpInputBuffer = jumpInputBufferTime;
        }
        else
        {
            jumpInputBuffer -= Time.deltaTime;
        }


        // 대쉬 입력 버퍼 처리
        if (Input.GetButtonDown("Fire3") && dashCooldownTimer <= 0f)
        {
            dashInputBuffer = dashBufferTime;
            dashCooldownTimer = dashCooldown;
        }
        else
        {
            dashInputBuffer -= Time.deltaTime;
        }


        if (!delay.canAct) return;
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 점프 실행
        if (jumpInputBuffer > 0 && jumpCount < maxJumpCount)
        {
            jumpTime = 0f;
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, jumpPower);
            isJumping = true;
            jumpCount++;
            jumpInputBuffer = 0f; // 버퍼 소비
        }

        // 버튼 누르는 시간 기록
        if (Input.GetButton("Jump") && isJumping)
        {
            jumpTime += Time.deltaTime;
        }

        // 소점프 처리
        if (Input.GetButtonUp("Jump") && isJumping)
        {
            if (jumpTime <= 8f / 60f) // 소점프 기준 프레임
            {
                rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, rigid.linearVelocity.y * 0.4f);
            }
        }
        if (jumpCount == 1)
        {
            animator.SetBool("isJumping", true);
        }
        if (jumpCount == 2)
        {
            animator.SetBool("isJumping", false); // 상태 초기화
            animator.SetBool("isJumping", true);  // 다시 트리거

        }

        // 대쉬 쿨타임 타이머 감소
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;
        if (dashInputBuffer > 0)
        {
            playerAttackinfo.DoAction(ActType.Dash);
            dashInputBuffer = 0f; // 버퍼 소비
        }



        // 수평 이동
        float h = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x * 0.5f, rigid.linearVelocity.y);
        }
        // 이동 애니메이션 설정
        animator.SetBool("isRunning", h != 0);

        // 방향 전환 애니메이션 설정
        // **방향이 바뀌었을 때만 "Turn" 애니메이션 실행**
        if ((h > 0 && spriteRenderer.flipX) || (h < 0 && !spriteRenderer.flipX))
        {
            animator.SetTrigger("turnTrigger"); // "Turn" 실행
            spriteRenderer.flipX = !spriteRenderer.flipX; // 방향 변경
        }

        if (dashInputBuffer > 0)
        {
            playerAttackinfo.DoAction(ActType.Dash);
            dashInputBuffer = 0f; // 버퍼 소비
        }



        //공격  
        //일반
        if (Input.GetButtonDown("Fire1") && !isJumping)
        {
            // 위공
            if (vertical > 0)
            {
                playerAttackinfo.DoAction(ActType.UPAttack);
                isAttacking = true;
            }
            // 가만공
            else if (vertical == 0)
            {
                playerAttackinfo.DoAction(ActType.StandAttack);
                isAttacking = true;
            }
            else if (vertical < 0)
            { 
             // 앉아공격 (↓ 입력 & 땅에 있을 때)
                playerAttackinfo.DoAction(ActType.DownAttack);
                isAttacking = true;
            }
        }



        if (Input.GetButtonDown("Fire1") && isJumping)
        { // ↓ 입력 여부 확인 (-1이면 아래)
            // 점프위공격
            if (vertical > 0 && playerAttackinfo.JDACount < 1)
            {
            }
            // 점프공격
            else if (vertical == 0 && playerAttackinfo.JACount < 2)
            {
                playerAttackinfo.DoAction(ActType.JumpAttack);
                isAttacking = true;
            }
            // 점프아래공격
            if (vertical < 0 && playerAttackinfo.JDACount < 1)
            {
                playerAttackinfo.DoAction(ActType.JumpDownAttack);
                isAttacking = true;
            }
        }

    }

    void FixedUpdate()
    {
        //착지모션
        if (rigid.linearVelocity.y < 0)//땅에있을때
        {
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 7, LayerMask.GetMask("Ground") & ~LayerMask.GetMask("Sandbag"));
            if (rayHit.collider != null)//바닥감지
            {
                if (rayHit.distance < 7)
                    Debug.Log(rayHit.collider.name);
                animator.SetBool("isJumping", false);
                isJumping = false;
                jumpCount = 0;
                playerAttackinfo.JACount = 0;
                playerAttackinfo.JDACount = 0;
            }

        }


        if (!delay.canAct) return;


        float h = Input.GetAxisRaw("Horizontal");

        // AddForce를 사용해 한 번에 최대 속도로 힘을 가함
        rigid.AddForce(Vector2.right * h * forceAmount, ForceMode2D.Impulse);

        // 속도 제한 적용
        rigid.linearVelocity = new Vector2(Mathf.Clamp(rigid.linearVelocity.x, -maxSpeed, maxSpeed), rigid.linearVelocity.y);


    }


    // 히트박스 켜기 (애니메이션 이벤트로 호출)
    public void EnableHitboxAtFrame(int frameIndex)
    {
        hitboxController.EnableHitboxAtFrame(frameIndex);
    }
    // 히트박스 끄기 (애니메이션 끝나는 시점에 이벤트로 호출)
    public void DisableHitbox() //히트박스 끄기
    {
        hitboxController.DisableHitbox();
    }



    public void EnableHurtboxAtFrame(int frameIndex) //허트박스 프레임 감지

    {
        hurtboxController.EnableHurtboxAtFrame(frameIndex);
    }

    public void ResetHutbox()
    {
        hurtboxController.ResetHurtbox();
    }
    public void DisableHurtbox() //허트박스 끄기
    {
        hurtboxController.DisableHurtbox();
    }
    
    
}