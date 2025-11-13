// 이 스크립트는 플레이어의 액션 정보를 관리하는 컴포넌트입니다.
// 플레이어의 공격, 대쉬 등의 액션 데이터를 설정하고 애니메이션을 제어합니다.
using UnityEngine;

public class PlayerActinfo : MonoBehaviour
{
    public PlayerData playerData; // 에디터에서 캐릭터별 SO 할당
    public Player player;
    public ProjectileController projectileController;
    public HitboxController hitboxController;
    public HurtboxController hurtboxController;
    public DashController dashController; //대쉬 스크립트 참조
    public Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public Delay delay; //딜레이 스크립트 참조

    // 카운트들 등 상태는 그대로 유지
    public int JACount = 0;
    public int JUACount = 0;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectileController = GetComponentInChildren<ProjectileController>();
        hitboxController = GetComponentInChildren<HitboxController>();
        hurtboxController = GetComponentInChildren<HurtboxController>();
        dashController = GetComponent<DashController>();
        delay = GetComponentInChildren<Delay>();
        animator = GetComponent<Animator>();

    }
    public void DoAction(ActType type)
    {
        ActData data = playerData != null ? playerData.GetAttackData(type) : null;
        if (data == null)
        {
            Debug.LogWarning($"어택데이터 까먹음");
            return;
        }
        //투사체 설정
        projectileController.currentProjectileData = data.projectileData;

        // 히트/허트박스 설정
        hitboxController.currentHitboxData = data.hitboxData;
        hurtboxController.currentHurtboxData = data.hurtboxData;
        dashController.currentDashData = data.dashData;
        

        // 애니메이터 트리거
        animator.SetTrigger(data.animatorTrigger);

        
        // 딜레이
        delay.SetDelay(data.delayTime);

        // 카운트(특수 처리 필요하면 switch로 분기)
        switch (type)
        {
            case ActType.JumpAttack: JACount++; break;
            case ActType.JumpUPAttack: JUACount++; break;
            // 필요 시 더 추가
        }
    }

    // 기존 메서드 호환성 (선택)
    

}
