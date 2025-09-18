using UnityEngine;

public class HurtboxController : MonoBehaviour
{
    public BoxCollider2D hurtbox;
    private SpriteRenderer spriteRenderer;
    public HurtboxData currentHurtboxData; // 플레이어가 공격 시작 시 할당
    HurtboxFrameData currentFrameData;
    private int currentFrameIndex;
    private Vector2 defaultOffset;
    private Vector2 defaultSize;
    
    void Start()
    {
        hurtbox = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        defaultOffset = hurtbox.offset;
        defaultSize = hurtbox.size;
        hurtbox.isTrigger = true;
        hurtbox.enabled = true;
    }

    public void ApplyFrame(int frameIndex, bool facingLeft)
    {
        

        hurtbox.enabled = false;
        if (currentHurtboxData == null || frameIndex < 0 || frameIndex >= currentHurtboxData.frames.Length)
            return;
        currentFrameIndex = frameIndex;
        currentFrameData = currentHurtboxData.frames[frameIndex]; // 프레임 데이터 할당

        Vector2 offset = currentFrameData.offset;
        if (facingLeft) offset.x *= -1;

        hurtbox.offset = offset;
        hurtbox.size = currentFrameData.size;
        hurtbox.enabled = true;
    }
    public void EnableHurtboxAtFrame(int frameIndex)
    {
        if (currentHurtboxData == null)
    {
        Debug.LogWarning("❗ currentHurtboxData가 null입니다! Hurtbox를 적용할 수 없습니다.");
        return;
    }

    if (frameIndex >= 0 && frameIndex < currentHurtboxData.frames.Length)
    {
        ApplyFrame(frameIndex, spriteRenderer.flipX);
    }

    }
    public void ResetHurtbox()
    {
        hurtbox.offset = defaultOffset;
        hurtbox.size = defaultSize;
    }

    // Update is called once per frame
    public void DisableHurtbox()
    {
        hurtbox.enabled = false;
    }
}
