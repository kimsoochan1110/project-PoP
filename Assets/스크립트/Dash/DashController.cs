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
        private void ApplyYDashFrame(int frameIndex)
    {
        if (currentDashData == null || frameIndex < 0 || frameIndex >= currentDashData.frames.Length)
            return;

        DashFrameData frame = currentDashData.frames[frameIndex];

        // 방향 처리 (좌우 반전)
        Vector2 direction = frame.direction;
        if (spriteRenderer != null && spriteRenderer.flipX) direction.x *= -1;

        // 기준 속도(플레이어 Rigidbody 우선, 없으면 자신의 rigid)
        Vector2 baseVel = Vector2.zero;
        if (playerRigid != null) baseVel = playerRigid.linearVelocity;
        else if (rigid != null) baseVel = rigid.linearVelocity;

        // X는 더하고(Y는 절대값으로 설정)
        float newX = baseVel.x + (direction.normalized.x * frame.DashForce);
        float newY = direction.normalized.y * frame.DashForce;

        if (rigid != null)
        {
            rigid.linearVelocity = new Vector2(newX, newY);
            rigid.gravityScale = frame.gravityScale;
        }
    }
    private void ApplyXDashFrame(int frameIndex)
    {
        if (currentDashData == null || frameIndex < 0 || frameIndex >= currentDashData.frames.Length)
            return;

        DashFrameData frame = currentDashData.frames[frameIndex];

        // 방향 처리 (좌우 반전)
        Vector2 direction = frame.direction;
        if (spriteRenderer != null && spriteRenderer.flipX) direction.x *= -1;

        // 기준 속도(플레이어 Rigidbody 우선, 없으면 자신의 rigid)
        Vector2 baseVel = Vector2.zero;
        if (playerRigid != null) baseVel = playerRigid.linearVelocity;
        else if (rigid != null) baseVel = rigid.linearVelocity;

        // X는 더하고(Y는 절대값으로 설정)
        float newX = direction.normalized.x * frame.DashForce;
        float newY = baseVel.y + (direction.normalized.y * frame.DashForce);
        if (rigid != null)
        {
            rigid.linearVelocity = new Vector2(newX, newY);
            rigid.gravityScale = frame.gravityScale;
        }
    }


    public void SetVelocity(int frameIndex)
    {
        if (frameIndex >= 0 && frameIndex < currentDashData.frames.Length)
        {
            ApplyDashFrame(frameIndex);
        }
    }

    public void SetYvelocity(int frameIndex)
    {
        if (frameIndex >= 0 && frameIndex < currentDashData.frames.Length)
        {
            ApplyYDashFrame(frameIndex);
        }
    }

    public void SetXvelocity(int frameIndex)
    {
        if (frameIndex >= 0 && frameIndex < currentDashData.frames.Length)
        {
            ApplyXDashFrame(frameIndex);
        }
    }

    public void AddVelocity(int frameIndex)
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