using UnityEngine;

public class DashController : MonoBehaviour
{
    public DashData currentDashData;
    public Player player;
    public Rigidbody2D playerRigid; 
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private float originalGravityScale; // 클래스 레벨 변수로 선언

    void Start()
    {
        playerRigid = player.GetComponent<Rigidbody2D>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalGravityScale = rigid.gravityScale; // 시작 시 저장

    }

    private void ApplyDashFrame(int frameIndex)
    {
        if (currentDashData == null || frameIndex < 0 || frameIndex >= currentDashData.frames.Length)
            return;

        DashFrameData frame = currentDashData.frames[frameIndex];

        // 방향 처리 (좌우 반전)
        Vector2 direction = frame.direction;
        if (spriteRenderer.flipX) direction.x *= -1;

        // 대쉬 적용
        rigid.linearVelocity = direction.normalized * frame.DashForce;
        rigid.gravityScale = frame.gravityScale;
    }

    private void ApplyPlusDashFrame(int frameIndex)
    {
        if (currentDashData == null || frameIndex < 0 || frameIndex >= currentDashData.frames.Length)
            return;

        DashFrameData frame = currentDashData.frames[frameIndex];

        // 방향 처리 (좌우 반전)
        Vector2 direction = frame.direction;
        if (spriteRenderer.flipX) direction.x *= -1;

        // 대쉬 적용
        Vector2 baseVel = playerRigid.linearVelocity;
        rigid.linearVelocity = baseVel + (direction.normalized * frame.DashForce);
        rigid.gravityScale = frame.gravityScale;
    }

    public void EnableDashFrame(int frameIndex)
    {
        if (frameIndex >= 0 && frameIndex < currentDashData.frames.Length)
        {
            ApplyDashFrame(frameIndex);
        }
    }


    public void EnablePlusDashFrame(int frameIndex)
    {
        if (frameIndex >= 0 && frameIndex < currentDashData.frames.Length)
        {
            ApplyPlusDashFrame(frameIndex);
        }
    }

    public void DisableDash()
    {
        rigid.gravityScale = originalGravityScale; // 원래 중력 복원
    }
}