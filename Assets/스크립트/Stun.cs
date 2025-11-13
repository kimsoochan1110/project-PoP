// 이 스크립트는 캐릭터가 스턴 상태에 빠졌을 때의 동작을 관리합니다.
using System.Collections;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public bool canAct = true;
    public Animator animator;
    private Rigidbody2D rigid;

    public float hitGravityScale = 0.5f; // 피격 시 적용할 중력 값
    private float originalGravityScale = 1f;

    private Coroutine stunRoutine;

    void Awake()
    {
        if (animator == null) animator = GetComponentInParent<Animator>();
        if (rigid == null) rigid = GetComponentInParent<Rigidbody2D>();
        if (rigid != null) originalGravityScale = rigid.gravityScale;
    }

    // 외부에서 호출 시 기존 스턴을 취소하고 새로 적용(갱신)함
    public void SetStun(float stunTime)
    {
        // 기존 스턴 타이머 중지
        if (stunRoutine != null)
        {
            StopCoroutine(stunRoutine);
            stunRoutine = null;
        }
        // 즉시 적용 및 코루틴 시작
        stunRoutine = StartCoroutine(StunCoroutine(stunTime));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        if (duration <= 0f)
        {
            // 0 이라면 즉시 해제
            ResetStunImmediate();
            yield break;
        }

        canAct = false;
        if (animator != null) animator.SetBool("isStunned", true);
        if (rigid != null) rigid.gravityScale = hitGravityScale;

        yield return new WaitForSeconds(duration);

        ResetStunImmediate();
    }

    public void ResetStunImmediate()
    {
        // 중복 안전 처리
        if (stunRoutine != null)
        {
            StopCoroutine(stunRoutine);
            stunRoutine = null;
        }

        canAct = true;
        if (animator != null) animator.SetBool("isStunned", false);
        if (rigid != null) rigid.gravityScale = originalGravityScale;
    }
}