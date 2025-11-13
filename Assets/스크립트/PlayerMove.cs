// 이 스크립트는 플레이어의 이동 및 행동을 제어하는 컴포넌트입니다.
// 플레이어의 입력을 처리하고, 점프, 대쉬, 공격 등의 동작을 수행합니다.
using System.Collections;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
    
public class Player : MonoBehaviour
{

    public Delay delay; //딜레이 스크립트 참조
    public Stun stun; //스턴 스크립트 참조

    public int playerIndex = 1; // Inspector에서 1 또는 2 설정 (입력 접미사: _1P, _2P)

    public PlayerActinfo playerAttackinfo;
    public HitboxController hitboxController;
    public HurtboxController hurtboxController;
    public ProjectileController projectileController;
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
        projectileController = GetComponentInChildren<ProjectileController>();
        dashController = GetComponent<DashController>();
        delay = GetComponentInChildren<Delay>();
        stun = GetComponent<Stun>();
        animator = GetComponent<Animator>();
        playerAttackinfo = GetComponent<PlayerActinfo>();
        lastFlipX = spriteRenderer.flipX; // 초기 방향 저장
    }
    // 입력 이름에 플레이어 인덱스 접미사 붙여 반환
    private string InputName(string baseName)
    {
        if (playerIndex <= 0) return baseName;
        return $"{baseName}_{playerIndex}P"; // 예: "Vertical_1P"
    }
    private float Axis(string name) => Input.GetAxisRaw(InputName(name));
    private bool ButtonDown(string name) => Input.GetButtonDown(InputName(name));
    private bool Button(string name) => Input.GetButton(InputName(name));
    private bool ButtonUp(string name) => Input.GetButtonUp(InputName(name));

    private void Update()
    {
        float vertical = Axis("Vertical");

        // 대쉬 입력 버퍼 처리
        if (ButtonDown("Fire3") && dashCooldownTimer <= 0f)
        {
            dashInputBuffer = dashBufferTime;
            dashCooldownTimer = dashCooldown;
        }
        else
        {
            dashInputBuffer -= Time.deltaTime;
        }

        // 점프 입력 버퍼 처리
        if (ButtonDown("Jump"))
        {
            jumpInputBuffer = jumpInputBufferTime;
        }
        else
        {
            jumpInputBuffer -= Time.deltaTime;
        }


        


        if (!delay.canAct) return;
        if (!stun.canAct) return;
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // 공중에서 대쉬+공격 동시입력 시 대쉬 우선 처리
        if (dashInputBuffer > 0f && isJumping)
        {
            dashInputBuffer = 0f;
            playerAttackinfo.DoAction(ActType.Dash);
            isAttacking = true;
            return; // 대쉬가 우선이면 해당 프레임의 공격 처리는 건너뜀
        }
        //공격  
        //일반
        if (ButtonDown("Fire1") && !isJumping)
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



        if (ButtonDown("Fire1") && isJumping)
        { // ↓ 입력 여부 확인 (-1이면 아래)
            // 점프위공격
            if (vertical > 0 && playerAttackinfo.JUACount < 1)
            {
                playerAttackinfo.DoAction(ActType.JumpUPAttack);
                isAttacking = true;
            }
            // 점프공격
            else if (vertical == 0 && playerAttackinfo.JACount < 2)
            {
                playerAttackinfo.DoAction(ActType.JumpAttack);
                isAttacking = true;
            }
            // 점프아래공격
            else if (vertical < 0)
            {
                playerAttackinfo.DoAction(ActType.JumpDownAttack);
                isAttacking = true;
            }
        }

        if (ButtonUp("Fire2"))
        { // ↓ 입력 여부 확인 (-1이면 아래)
            // 점프위공격
            if (vertical > 0)
            {
                
            }
            // 점프공격
            else if (vertical == 0)
            {
                playerAttackinfo.DoAction(ActType.StandShoot);
                isAttacking = true;
            }
            // 점프아래공격
            else if (vertical < 0)
            {
                
            }
        }
        

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
        if (Button("Jump") && isJumping)
        {
            jumpTime += Time.deltaTime;
        }

        // 소점프 처리
        if (ButtonUp("Jump") && isJumping)
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
        float h = Axis("Horizontal");
        if (ButtonUp("Horizontal"))
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x * 0.05f, rigid.linearVelocity.y);
        }
        // 이동 애니메이션 설정
        animator.SetBool("isRunning", h != 0);

        // 방향 전환 애니메이션 설정
        // **방향이 바뀌었을 때만 "Turn" 애니메이션 실행**
        if ((h > 0 && spriteRenderer.flipX) || (h < 0 && !spriteRenderer.flipX))
        {
            animator.SetTrigger("turnTrigger"); // "Turn" 실행
            spriteRenderer.flipX = !spriteRenderer.flipX; // 방향 변경
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x * 0f, rigid.linearVelocity.y); // 방향 전환 시 속도 0으로 초기화
        }

        if (dashInputBuffer > 0)
        {
            playerAttackinfo.DoAction(ActType.Dash);
            dashInputBuffer = 0f; // 버퍼 소비
        }



        
        Debug.Log($"vertical={vertical}");

    }

    void FixedUpdate()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 7, LayerMask.GetMask("Ground") & ~LayerMask.GetMask("Sandbag"));
        //착지모션
        if (rigid.linearVelocity.y < 0)//땅에있을때
        {
            if (rayHit.collider != null)//바닥감지
            {
                if (rayHit.distance < 7)
                    Debug.Log(rayHit.collider.name);
                animator.SetBool("isJumping", false);
                isJumping = false;
                jumpCount = 0;
                playerAttackinfo.JACount = 0;
                playerAttackinfo.JUACount = 0;
            }
        }
        else if (rigid.linearVelocity.y >= 0 && rayHit.collider == null)
        {
            isJumping = true;
            animator.SetBool("isJumping", true);
        } 


        if (!delay.canAct) return;
        if (!stun.canAct) return;


        float h = Axis("Horizontal");

        // AddForce를 사용해 한 번에 최대 속도로 힘을 가함
        rigid.AddForce(Vector2.right * h * forceAmount, ForceMode2D.Impulse);

        // 속도 제한 적용x`
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
    
    public void EnableProjectileFrame(int frameIndex)
    {
        projectileController.EnableProjectileFrame(frameIndex);
    }
}