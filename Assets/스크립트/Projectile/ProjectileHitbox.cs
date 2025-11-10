using UnityEngine;

public class ProjectileHitbox : MonoBehaviour
{
    public GameObject owner;
    public Collider2D projectileCollider;
    public ProjectileFrameData currentFrameData;

    void Awake()
    {
        projectileCollider = GetComponent<Collider2D>();
        
    }

    public void Init(GameObject ownerObject, ProjectileFrameData fixedHitData)
    {
        owner = ownerObject;
        currentFrameData = fixedHitData;
        // 발사자 및 발사자 자식들에 있는 모든 Collider2D와 무시 처리
        var ownerCols = owner.GetComponentsInChildren<Collider2D>();
        foreach (var oc in ownerCols)
        {
            if (oc != null)
                Physics2D.IgnoreCollision(projectileCollider, oc, true);
        }
        Invoke(nameof(ExplodeAndDestroy), currentFrameData.lifeTime);
    }

    // 핵심: Hurtbox 태그를 가진 대상과 충돌하면 즉시 파괴
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        // 피격 대상이 Hurtbox 태그면 처리
        if (other.CompareTag("Hurtbox"))
        {
            ExplodeAndDestroy();
        }
    }

    private void ExplodeAndDestroy()
    {
        bool facingLeft = GetFacingLeft();


        if (currentFrameData.destroyPrefab != null)
        {
            GameObject instance = Instantiate(currentFrameData.destroyPrefab, transform.position, Quaternion.identity);
            instance.transform.parent = null;

            var projectileExplosion = instance.GetComponent<ProjectileExplosion>();
            projectileExplosion.Init(owner, currentFrameData);

            var sprites = instance.GetComponentsInChildren<SpriteRenderer>(true);
            if (sprites != null && sprites.Length > 0)
            {
                foreach (var sr in sprites) sr.flipX = facingLeft;
            }
        }
        else
        {
            Debug.LogWarning("파괴 프리팹 없음");
        }
        
        
        Destroy(gameObject);
    }
    
    private bool GetFacingLeft()
    {
        var sprite = GetComponent<SpriteRenderer>();
        if (sprite != null) return sprite.flipX;

        var rigid = GetComponent<Rigidbody2D>();
        if (rigid != null) return rigid.linearVelocity.x < 0f;

        return transform.lossyScale.x < 0f;
    }
}
