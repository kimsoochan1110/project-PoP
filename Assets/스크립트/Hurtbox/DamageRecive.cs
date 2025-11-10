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
