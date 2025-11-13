// 이 스크립트는 플레이어의 액션 데이터를 정의하는 ScriptableObject입니다.
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