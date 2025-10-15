using UnityEngine;

// ...existing code...
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

        Rigidbody2D target = playerRigid ?? rigid;
        originalGravityScale = (target != null) ? target.gravityScale : 1f;
    }

    // 유틸: 항상 조작할 Rigidbody 반환 (플레이어 우선)
    private Rigidbody2D TargetRigid()
    {
        return playerRigid != null ? playerRigid : rigid;
    }

    private bool IsTargetStunned()
    {
        if (player != null && player.stun != null) return !player.stun.canAct;
        return false;
    }

    private void ApplyDashFrame(int frameIndex)
    {
        if (currentDashData == null || frameIndex < 0 || frameIndex >= currentDashData.frames.Length) return;

        if (IsTargetStunned()) return; // 스턴 중이면 대쉬 적용 안 함

        DashFrameData frame = currentDashData.frames[frameIndex];
        Vector2 direction = frame.direction;
        if (spriteRenderer != null && spriteRenderer.flipX) direction.x *= -1;

        var target = TargetRigid();
        if (target == null) return;

        // X,Y 처리: 기본적으로 X는 덮어쓰고 Y는 보존(혹은 데이터 명시시 덮어쓰기)
        Vector2 baseVel = target.linearVelocity;
        float newY = direction.normalized.y * frame.DashForce;
        float newX = direction.normalized.x * frame.DashForce;
        target.linearVelocity = new Vector2(newX, newY);
        target.gravityScale = frame.gravityScale;
    }

    private void ApplyPlusDashFrame(int frameIndex)
    {
        if (currentDashData == null || frameIndex < 0 || frameIndex >= currentDashData.frames.Length) return;
        if (IsTargetStunned()) return;

        DashFrameData frame = currentDashData.frames[frameIndex];
        Vector2 direction = frame.direction;
        if (spriteRenderer != null && spriteRenderer.flipX) direction.x *= -1;

        var target = TargetRigid();
        if (target == null) return;

        // baseVel은 반드시 target의 현재 velocity로 가져오고 Y는 보존
        Vector2 baseVel = target.linearVelocity;
        Vector2 added = direction.normalized * frame.DashForce;
        Vector2 result = new Vector2(baseVel.x + added.x, Mathf.Abs(added.y) > 0.001f ? added.y : baseVel.y);
        target.linearVelocity = result;
        target.gravityScale = frame.gravityScale;
    }

    private void ApplyYDashFrame(int frameIndex)
    {
        if (currentDashData == null || frameIndex < 0 || frameIndex >= currentDashData.frames.Length) return;
        if (IsTargetStunned()) return;

        DashFrameData frame = currentDashData.frames[frameIndex];
        Vector2 direction = frame.direction;
        if (spriteRenderer != null && spriteRenderer.flipX) direction.x *= -1;

        var target = TargetRigid();
        if (target == null) return;

        Vector2 baseVel = target.linearVelocity;
        float newX = baseVel.x + direction.normalized.x * frame.DashForce;
        float newY = direction.normalized.y * frame.DashForce;
        target.linearVelocity = new Vector2(newX, newY);
        target.gravityScale = frame.gravityScale;
    }

    private void ApplyXDashFrame(int frameIndex)
    {
        if (currentDashData == null || frameIndex < 0 || frameIndex >= currentDashData.frames.Length) return;
        if (IsTargetStunned()) return;

        DashFrameData frame = currentDashData.frames[frameIndex];
        Vector2 direction = frame.direction;
        if (spriteRenderer != null && spriteRenderer.flipX) direction.x *= -1;

        var target = TargetRigid();
        if (target == null) return;

        Vector2 baseVel = target.linearVelocity;
        float newX = direction.normalized.x * frame.DashForce;
        float newY = baseVel.y + direction.normalized.y * frame.DashForce;
        target.linearVelocity = new Vector2(newX, newY);
        target.gravityScale = frame.gravityScale;
    }

    // Set/Add 메서드들: 위 유틸 사용
    public void SetVelocity(int frameIndex)
    {
        ApplyDashFrame(frameIndex);
    }
    public void SetYvelocity(int frameIndex)
    {
        ApplyYDashFrame(frameIndex);
    }
    public void SetXvelocity(int frameIndex)
    {
        ApplyXDashFrame(frameIndex);
    }
    public void AddVelocity(int frameIndex)
    {
        ApplyPlusDashFrame(frameIndex);
    }

    public void DisableDash()
    {
        var target = TargetRigid();
        if (target != null) target.gravityScale = originalGravityScale;
    }
}