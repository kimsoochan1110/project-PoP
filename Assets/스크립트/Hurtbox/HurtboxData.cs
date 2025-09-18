using UnityEngine;

[System.Serializable]
public class HurtboxFrameData
{
    public Vector2 offset; // 허트트박스 위치
    public Vector2 size;   // 허트박스 크기
}
[CreateAssetMenu(fileName = "HurtboxData", menuName = "ScriptableObjects/HurtboxData", order = 2)]
public class HurtboxData : ScriptableObject
{
    public HurtboxFrameData[] frames;
}
