using UnityEngine;

[System.Serializable]
public class DashFrameData
{
    public Vector2 direction = Vector2.right; // 대쉬 방향
    public float DashForce;                // 대쉬 힘/크기
    public float gravityScale;           // 대쉬 중 중력
}

[CreateAssetMenu(fileName = "DashData", menuName = "ScriptableObjects/DashData", order = 3)]
public class DashData : ScriptableObject
{
    public DashFrameData[] frames;
}