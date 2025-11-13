using UnityEngine;

public class HurtboxController : MonoBehaviour
{
    public PlayerState playerState;
    public BoxCollider2D hurtbox;
    private SpriteRenderer spriteRenderer;
    public HurtboxData currentHurtboxData; // 플레이어가 공격 시작 시 할당
    HurtboxFrameData currentFrameData;
    private int currentFrameIndex;
    private Vector2 defaultOffset;
    private Vector2 defaultSize;

    private bool invincible = false;
    public float invincibleTime = 1f;
    private Color originalColor;
    
    void Start()
    {
        playerState = GetComponentInParent<PlayerState>();
        hurtbox = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        defaultOffset = hurtbox.offset;
        defaultSize = hurtbox.size;
        hurtbox.isTrigger = true;
        hurtbox.enabled = true;
        originalColor = spriteRenderer.color;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;
        if (invincible) return;

        if (other.CompareTag("Lifedestroyer"))
        {
            playerState.life -= 1;
            // 무적 시작, 반투명 처리, 일정시간 후 복구
            DisableHurtbox();
            invincible = true;
            Color c = spriteRenderer.color;
            c.a = 0.4f;
            spriteRenderer.color = c;
            Invoke("EndInvincible", invincibleTime);
            Debug.Log("Player Life: " + playerState.life);
        }
    }
    
    public void ResetHurtbox()
    {
        hurtbox.enabled = true;
        hurtbox.offset = defaultOffset;
        hurtbox.size = defaultSize;
    }

    // Update is called once per frame
    public void DisableHurtbox()
    {
        hurtbox.enabled = false;
    }

    private void EndInvincible()
    {
        ResetHurtbox();
        invincible = false;
        if (spriteRenderer != null) spriteRenderer.color = originalColor;
    }
}
