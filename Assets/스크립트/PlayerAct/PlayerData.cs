using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    public ActData[] acts; // 배열 인덱스는 ActType enum 순서에 맞춰 유지 권장

    public ActData GetAttackData(ActType type)
    {
        int idx = (int)type;
        if (acts == null || idx < 0 || idx >= acts.Length) return null;
        return acts[idx];
    }
}