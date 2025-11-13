// 이 스크립트는 히트박스 데이터를 정의하는 ScriptableObject입니다.
using UnityEngine;

[System.Serializable]
public class HitboxFrameData
{
    public GameObject hitEffectPrefab; // 히트 이펙트 프리팹
    public float hitEffectLifeTime; // 히트 이펙트 유지 시간(프리팹 자체가 자동 삭제하면 무시됨)
    public Vector2 knockbackDirection = Vector2.right; //넉백방향
    public float knockbackPower; //넉백세기
    public float stunTime; // 스턴시간
    public Vector2 offset; // 히트박스 위치
    public Vector2 size;   // 히트박스 크기
     public float hitStopTime; // 히트스톱
}
[CreateAssetMenu(fileName = "HitboxData", menuName = "ScriptableObjects/HitboxData", order = 2)]
public class HitboxData : ScriptableObject
{   
    
    public HitboxFrameData[] frames;
}
