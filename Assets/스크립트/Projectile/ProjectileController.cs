using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private PlayerState playerState;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;

    public ProjectileData currentProjectileData;
    public ProjectileFrameData currentFrameData;
    private int currentFrameIndex;
    private int stratbullite;
    private bool isFired = false;   // 중복 방지 플래그

    public float fireCooldown = 0.05f; // 짧은 쿨타임 (50ms 정도)

    public void Start()
    {
        playerState = GetComponentInParent<PlayerState>();
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        stratbullite = playerState.bulletCount;
    }

    public void EnableProjectileFrame(int frameIndex)
    {
        if (playerState.bulletCount > 0)
        {
            if (isFired) return; // 이미 발사 중이면 무시
            StartCoroutine(FireOnce(frameIndex));
        }
        else
            return;
    }

    private IEnumerator FireOnce(int frameIndex)
    {
        isFired = true;
        ApplyProjectile(frameIndex, spriteRenderer != null ? spriteRenderer.flipX : false);
        yield return new WaitForSeconds(fireCooldown);
        isFired = false;
    }

    public void ApplyProjectile(int frameIndex, bool facingLeft)
    {
        if (currentProjectileData == null || frameIndex < 0 || frameIndex >= currentProjectileData.frames.Length)
            return;

        currentFrameIndex = frameIndex;
        currentFrameData = currentProjectileData.frames[frameIndex];

        Vector2 offset = currentFrameData.offset;
        if (facingLeft) offset.x *= -1;

        Vector3 spawnPos = transform.position + (Vector3)offset;
        GameObject projectilePrefab = currentFrameData.projectilePrefab;
        if (projectilePrefab == null) return;

        ProjectileFrameData hitData = new ProjectileFrameData
        {
            lifeTime = currentFrameData.lifeTime,
            damage = currentFrameData.damage,
            knockbackDirection = currentFrameData.knockbackDirection,
            knockbackPower = currentFrameData.knockbackPower,
            stunTime = currentFrameData.stunTime,
            explosionLifeTime = currentFrameData.explosionLifeTime,
            destroyPrefab = currentFrameData.destroyPrefab,
            hitEffectPrefab = currentFrameData.hitEffectPrefab,
            hitEffectLifeTime = currentFrameData.hitEffectLifeTime,
            hitStopTime = currentFrameData.hitStopTime
        };

        GameObject instance = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        instance.transform.parent = null;

        playerState.bulletCount--;

        var projectileHitbox = instance.GetComponent<ProjectileHitbox>();
        projectileHitbox.Init(gameObject, hitData);

        Rigidbody2D rigid = instance.GetComponent<Rigidbody2D>();
        Vector2 direction = currentFrameData.projectileDirection;
        if (facingLeft) direction.x *= -1;

        rigid.AddTorque(currentFrameData.rotation, ForceMode2D.Impulse);
        rigid.linearVelocity = direction.normalized * currentFrameData.speed;

        var sr = instance.GetComponent<SpriteRenderer>();
        if (sr != null) sr.flipX = facingLeft;
    }

    public void Rerode()
    {
        playerState.bulletCount = stratbullite;
    }
}