// 이 스크립트는 데미지를 받는 캐릭터의 동작을 처리합니다.
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    private PlayerActinfo playerActinfo;
    private Rigidbody2D rigid;
    private Animator animator;
    private Stun stun;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerActinfo = GetComponent<PlayerActinfo>();
        animator = GetComponent<Animator>();
        stun = GetComponent<Stun>();
    }

    public void TakeHit(HitboxFrameData data, Vector3 attackerPosition)
    {
        if (data.hitEffectPrefab != null)
        {
            if (transform.Find(data.hitEffectPrefab.name + "(Clone)") != null) //중복감지
                Destroy(transform.Find(data.hitEffectPrefab.name + "(Clone)").gameObject);

            GameObject hitEffectInstance = Instantiate(data.hitEffectPrefab, transform.position, Quaternion.identity);
            hitEffectInstance.transform.parent = transform; // 히트 이펙트를 피격 대상의 자식으로 설정
            Destroy(hitEffectInstance, data.hitEffectLifeTime);

        }
        
        if (data.hitStopTime > 0) // ✅ 히트스톱 실행
            HitStopManager.Instance.DoHitStop(data.hitStopTime);

        Vector2 baseDirection = data.knockbackDirection.normalized; //넉백 방향
        stun.SetStun(data.stunTime); // 스턴 적용

        // 공격자가 오른쪽에 있으면 방향 반전
        if (attackerPosition.x > transform.position.x)
            baseDirection.x *= -1;

        Vector2 knockback = baseDirection * data.knockbackPower;


        Debug.Log("최종 넉백 벡터: " + knockback);
        rigid.linearVelocity = Vector2.zero; // 기존 속도 초기화
        rigid.AddForce(knockback, ForceMode2D.Impulse); // 넉백 적용        
    }

    public void ProjectileTakeHit(ProjectileFrameData data, Vector3 attackerPosition)
    {
        if (data.hitEffectPrefab != null)
        {
            if (transform.Find(data.hitEffectPrefab.name + "(Clone)") != null) //중복감지
                Destroy(transform.Find(data.hitEffectPrefab.name + "(Clone)").gameObject);

            GameObject hitEffectInstance = Instantiate(data.hitEffectPrefab, transform.position, Quaternion.identity);
            hitEffectInstance.transform.parent = transform; // 히트 이펙트를 피격 대상의 자식으로 설정
            Destroy(hitEffectInstance, data.hitEffectLifeTime);
        }

        if (data.hitStopTime > 0) // ✅ 히트스톱 실행
            HitStopManager.Instance.DoHitStop(data.hitStopTime);

        Vector2 baseDirection = data.knockbackDirection.normalized; //넉백 방향
        stun.SetStun(data.stunTime); // 스턴 적용

        // 공격자가 오른쪽에 있으면 방향 반전
        if (attackerPosition.x > transform.position.x)
            baseDirection.x *= -1;

        Vector2 knockback = baseDirection * data.knockbackPower;

        
        Debug.Log("최종 넉백 벡터: " + knockback);
        rigid.linearVelocity = Vector2.zero; // 기존 속도 초기화
        rigid.AddForce(knockback, ForceMode2D.Impulse); // 넉백 적용        
    }
}
