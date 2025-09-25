// ...existing code...
using System.Collections;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

public class CogWhell : MonoBehaviour
{
    public HitboxData hitboxData;
    public HitboxController hitboxController;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Inspector에 할당 안 되어 있으면 자식이나 같은 오브젝트에서 찾아서 폴백
        if (hitboxController == null)
            hitboxController = GetComponentInChildren<HitboxController>();

        // 기본 히트박스 데이터가 있으면 미리 할당 (공격 시작 전에 덮어씌워질 수 있음)
        if (hitboxController != null && hitboxData != null)
            hitboxController.currentHitboxData = hitboxData;
    }


    // 공격 시작(외부 호출 또는 테스트용)
    public void DoAttack()
    {
        if (hitboxController == null)
        {
            Debug.LogWarning($"DoAttack: hitboxController 없음 on {gameObject.name}");
            return;
        }
        if (hitboxData == null)
        {
            Debug.LogWarning($"DoAttack: hitboxData 없음 on {gameObject.name}");
            return;
        }

        // 공격용 데이터 세팅 후 애니메이터 트리거
        hitboxController.currentHitboxData = hitboxData;
        if (animator != null) animator.SetTrigger("CogWhell");
    }

    // 히트박스 켜기 (애니메이션 이벤트로 호출)
    public void EnableHitboxAtFrame(int frameIndex)
    {
        if (hitboxController == null)
        {
            Debug.LogWarning($"EnableHitboxAtFrame 호출됐지만 hitboxController가 없음 on {gameObject.name}");
            return;
        }
        if (hitboxController.currentHitboxData == null)
        {
            Debug.LogWarning($"EnableHitboxAtFrame: currentHitboxData가 없음 on {gameObject.name}");
            return;
        }

        hitboxController.EnableHitboxAtFrame(frameIndex);
    }
    // 히트박스 끄기 (애니메이션 끝나는 시점에 이벤트로 호출)
    public void DisableHitbox() //히트박스 끄기
    {
        if (hitboxController == null)
        {
            Debug.LogWarning($"DisableHitbox 호출됐지만 hitboxController가 없음 on {gameObject.name}");
            return;
        }
        hitboxController.DisableHitbox();
    }
}