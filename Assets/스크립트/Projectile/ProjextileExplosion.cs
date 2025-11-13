// 이 스크립트는 투사체의 폭발 효과를 처리하는 컴포넌트입니다.
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{
    public GameObject owner;
    public Collider2D explosionCollider;
    public ProjectileFrameData currentFrameData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        explosionCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    public void Init(GameObject ownerObject, ProjectileFrameData fixedHitData)
    {
        owner = ownerObject;
        currentFrameData = fixedHitData;
        // 발사자 및 발사자 자식들에 있는 모든 Collider2D와 무시 처리
        var ownerCols = owner.GetComponentsInChildren<Collider2D>();
        foreach (var oc in ownerCols)
        {
            if (oc != null)
                Physics2D.IgnoreCollision(explosionCollider, oc, true);
        }
        Destroy(gameObject, currentFrameData.explosionLifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.CompareTag("Hurtbox"))
        {
            if (owner != null)
            {
                var receiver = other.GetComponentInParent<DamageReceiver>();
                if (receiver != null && currentFrameData != null)
                {
                    receiver.ProjectileTakeHit(currentFrameData, transform.position);
                }
            }
        }
    }
}
