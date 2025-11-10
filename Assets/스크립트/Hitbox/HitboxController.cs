// 이 스크립트는 AttackHitbox 오브젝트에 붙는다
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public BoxCollider2D hitbox;
    private SpriteRenderer spriteRenderer;
    public HitboxData currentHitboxData; // 플레이어가 공격 시작 시 할당
    HitboxFrameData currentFrameData;
    private int currentFrameIndex;
    

    public void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        hitbox = GetComponent<BoxCollider2D>();
        hitbox.enabled = false;
    }
    

    public void ApplyFrame(int frameIndex, bool facingLeft)
    {
        hitbox.enabled = false;
        if (currentHitboxData == null || frameIndex < 0 || frameIndex >= currentHitboxData.frames.Length)
            return;
        currentFrameIndex = frameIndex;
        currentFrameData = currentHitboxData.frames[frameIndex]; // 프레임 데이터 할당

        Vector2 offset = currentFrameData.offset;
        if (facingLeft) offset.x *= -1;

        hitbox.offset = offset;
        hitbox.size = currentFrameData.size;
        hitbox.enabled = true;
    }
    public void EnableHitboxAtFrame(int frameIndex)
    {
        if (frameIndex >= 0 && frameIndex < currentHitboxData.frames.Length)
        {
            ApplyFrame(frameIndex, spriteRenderer.flipX);
        }
    }
    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D other) //히트감지
    {
        if (other.CompareTag("Hurtbox"))
        {
            if (currentHitboxData == null)
            {
                Debug.LogWarning("❗ currentHitboxData가 null입니다!");
                return;
            }
            
            Debug.Log("플레이어 피격!");
            DamageReceiver receiver = other.GetComponentInParent<DamageReceiver>();
            if (receiver != null)
            {
                // ✅ 히트스톱 실행
            if (currentFrameData.hitStopTime > 0)
                HitStopManager.Instance.DoHitStop(currentFrameData.hitStopTime);

                receiver.TakeHit(currentFrameData, transform.position);; // 여기서 피격 애니메이션 호출
            }
        }       
    }
}