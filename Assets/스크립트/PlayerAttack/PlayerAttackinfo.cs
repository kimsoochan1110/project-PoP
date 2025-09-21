using UnityEngine;

public class PlayerAttackinfo : MonoBehaviour
{
    public Delay delay; //딜레이 스크립트 참조
    //일반공격
    public HitboxData SAHitboxData; //일반공격
    public HurtboxData SAHurtboxData; //일반공격 허트박스
    public int SACount = 0; //일반공격 카운트

    public HitboxData DSAHitboxData; //대쉬공격
    public HurtboxData DSAHurtboxData; //대쉬공격 허트박스

    //점프공격
    public HitboxData JAHitboxData;
    public HurtboxData JAHurtboxData;
    public int JACount = 0; //점프공격 카운트
    //점프아래공격
    public HitboxData JDAHitboxData;
    public HurtboxData JDAHurtboxData;
    public int JDACount = 0;  //점프아래공격 카운트
    //대쉬
    public DashData dashData; //대쉬 데이터

    public HitboxController hitboxController;
    public HurtboxController hurtboxController;
    private DashController dashController; //대쉬 스크립트 참조
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitboxController = GetComponentInChildren<HitboxController>();
        hurtboxController = GetComponentInChildren<HurtboxController>();
        dashController = GetComponent<DashController>();
        delay = GetComponentInChildren<Delay>();
        animator = GetComponent<Animator>();

    }
    public void StandAttack()
    {
        hitboxController.currentHitboxData = SAHitboxData;
        hurtboxController.currentHurtboxData = SAHurtboxData;
        animator.SetTrigger("attackTrigger"); // 공격 애니메이션 실행
        rigid.linearVelocity = rigid.linearVelocity * 0.3f;
        delay.SetDelay(0.4f);
    }

    public void JumpAttack()
    {
        hitboxController.currentHitboxData = JAHitboxData;
        hurtboxController.currentHurtboxData = JAHurtboxData;
        JACount++;
        animator.SetTrigger("jumpattackTrigger"); // 점프공격 애니메이션 실행
        rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, 100f);
        delay.SetDelay(0.2f);
    }

    public void JumpDownAttack()
    {
        hitboxController.currentHitboxData = JDAHitboxData;
        hurtboxController.currentHurtboxData = JDAHurtboxData;
        JDACount++;
        animator.SetTrigger("JDA"); // 점프아래공격 애니메이션 실행
        rigid.linearVelocity = new Vector2(rigid.linearVelocity.x + 100 * (spriteRenderer.flipX ? -1 : 1), 100f);
        delay.SetDelay(0.6f);
    }
}
