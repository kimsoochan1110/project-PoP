using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileController : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    public ProjectileData currentProjectileData; // 플레이어가 공격 시작 시 할당
    ProjectileFrameData currentFrameData;
    private int currentFrameIndex;
    public ProjectileFrameData projectileData;
    public GameObject projectileTemplate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public void ApplyProjectile(int frameIndex, bool facingLeft)
    {
        if (currentProjectileData == null || frameIndex < 0 || frameIndex >= currentProjectileData.frames.Length)
        return;

    currentFrameIndex = frameIndex;
    currentFrameData = currentProjectileData.frames[frameIndex]; // 선택된 프레임 사용

    Vector2 offset = currentFrameData.offset;
    if (facingLeft) offset.x *= -1;

    // 플레이어(또는 이 컴포넌트)의 월드 위치에 offset을 더해 발사 위치 계산
    Vector3 spawnPos = transform.position + (Vector3)offset;

    // 프리팹 null 체크
    GameObject prefab = currentFrameData.projectilePrefab;
    if (prefab == null)
    {
        Debug.LogWarning("프리팹 없음 " + frameIndex);
        return;
    }

    // 반드시 Instantiate 해서 씬에 인스턴스 생성
    GameObject instance = Instantiate(prefab, spawnPos, Quaternion.identity);
    instance.transform.parent = null; // 템플릿이 플레이어 자식일 경우 묶이지 않게 분리

    // Rigidbody2D 가져와서 속도 설정 (null 체크)
    Rigidbody2D rigid = instance.GetComponent<Rigidbody2D>();
    if (rigid == null) rigid = instance.AddComponent<Rigidbody2D>();
    // 필요하면 bodyType/gravity 조정 (프리팹 기본 설정 권장)
    rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

    Vector2 direction = currentFrameData.projectileDirection;
    if (direction == Vector2.zero) direction = Vector2.right;
    if (facingLeft) direction.x *= -1;

    rigid.linearVelocity = direction.normalized * currentFrameData.speed;

    // 스프라이트 flip 처리 (프리팹에 SpriteRenderer가 있으면 덮어쓰기)
    var sr = instance.GetComponent<SpriteRenderer>();
    if (sr != null) sr.flipX = facingLeft;

    // 수명 설정
    Destroy(instance, Mathf.Max(0.01f, currentFrameData.lifeTime));
        
    }
    public void EnableProjectileFrame(int frameIndex)
    {
        if (currentProjectileData != null && frameIndex >= 0 && frameIndex < currentProjectileData.frames.Length)
        {
            ApplyProjectile(frameIndex, spriteRenderer != null ? spriteRenderer.flipX : false);
        }
    }
}
