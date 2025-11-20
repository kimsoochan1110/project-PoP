// 이 스크립트는 투사체의 데이터를 정의하는 ScriptableObject입니다.
using UnityEngine;

[System.Serializable]
public class ProjectileFrameData
{
    public GameObject hitEffectPrefab; // 히트 이펙트 프리팹
    public float hitEffectLifeTime; // 히트 이펙트 유지 시간(프리팹 자체가 자동 삭제하면 무시됨)
    //무브인포
    public GameObject projectilePrefab; // 투사체 프리팹
    public float speed; // 속도
    public float angle; // 각도
    public float rotation; // 회전력
    public Vector2 projectileDirection = Vector2.right; //발사 방향
    public Vector2 offset; // 발사 위치
    public float lifeTime; //초
    //히트인포
    public GameObject destroyPrefab; // 투사체 파괴 프리팹
    public float explosionLifeTime; //초
    public Vector2 knockbackDirection = Vector2.right; //넉백방향
    public int damage; //대미지
    public float knockbackPower; //넉백세기
    public float stunTime; // 스턴시간
    public float hitStopTime; // 히트스톱
    public float reloadTime; // 재장전 시간

}

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public ProjectileFrameData[] frames;
}