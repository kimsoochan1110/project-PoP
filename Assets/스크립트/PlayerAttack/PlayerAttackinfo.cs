using UnityEngine;

public class PlayerAttackinfo : MonoBehaviour
{
    public PlayerData playerData; // 에디터에서 캐릭터별 SO 할당
    public HitboxController hitboxController;
    public HurtboxController hurtboxController;
    public DashController dashController; //대쉬 스크립트 참조
    [HideInInspector] public Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public Delay delay; //딜레이 스크립트 참조

    // 카운트들 등 상태는 그대로 유지
    public int SACount = 0;
    public int JACount = 0;
    public int JDACount = 0;
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
    public void DoAttack(AttackType type)
    {
        AttackData data = playerData != null ? playerData.GetAttackData(type) : null;
        if (data == null)
        {
            Debug.LogWarning($"어택데이터 까먹음");
            return;
        }

        // 히트/허트박스 설정
        hitboxController.currentHitboxData = data.hitboxData;
        hurtboxController.currentHurtboxData = data.hurtboxData;
        dashController.currentDashData = data.dashData;

        // 애니메이터 트리거
        animator.SetTrigger(data.animatorTrigger);

        
        // 딜레이
        if (delay != null) delay.SetDelay(data.delayTime);

        // 카운트(특수 처리 필요하면 switch로 분기)
        switch (type)
        {
            case AttackType.StandAttack: SACount++; break;
            case AttackType.JumpAttack: JACount++; break;
            case AttackType.JumpDownAttack: JDACount++; break;
            // 필요 시 더 추가
        }
    }

    // 기존 메서드 호환성 (선택)
    

}
