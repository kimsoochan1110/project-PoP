using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    public ActData[] attacks; // 배열 인덱스는 AttackType enum 순서에 맞춰 유지 권장

    public ActData GetAttackData(AttackType type)
    {
        int idx = (int)type;
        if (attacks == null || idx < 0 || idx >= attacks.Length) return null;
        return attacks[idx];
    }
}