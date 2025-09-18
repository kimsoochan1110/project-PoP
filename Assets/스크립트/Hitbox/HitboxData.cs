using UnityEngine;

[System.Serializable]
public class HitboxFrameData
{
    public int damage; //대미지
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
