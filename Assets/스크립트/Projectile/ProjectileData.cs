using UnityEngine;

[System.Serializable]
public class ProjectileFrameData
{
    //무브인포
    public GameObject projectilePrefab; // 투사체 프리팹
    public float speed; // 속도
    public Vector2 projectileDirection = Vector2.right; //발사 방향
    public Vector2 offset; // 발사 위치
    public float lifeTime; //초
    //히트인포
    public Vector2 knockbackDirection = Vector2.right; //넉백방향
    public int damage; //대미지
    public float knockbackPower; //넉백세기
    public float stunTime; // 스턴시간
        public float hitStopTime; // 히트스톱
}

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public ProjectileFrameData[] frames;
}