using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Animator animator;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void TakeHit(HitboxFrameData data, Vector3 attackerPosition)
    {
        if (animator != null)
        animator.SetTrigger("Hit");
        
        Vector2 baseDirection = data.knockbackDirection.normalized;

        // 공격자가 오른쪽에 있으면 방향 반전
    if (attackerPosition.x > transform.position.x)
        baseDirection.x *= -1;

        Vector2 knockback = baseDirection * data.knockbackPower;

        
    Debug.Log("최종 넉백 벡터: " + knockback);
        rigid.linearVelocity = Vector2.zero; // 기존 속도 초기화
        rigid.AddForce(knockback, ForceMode2D.Impulse); // 넉백 적용        
    }
}
