// 이 스크립트는 플레이어의 액션 데이터를 정의하는 ScriptableObject입니다.
using UnityEngine;

[CreateAssetMenu(fileName = "ActData", menuName = "Game/ActData")]
public class ActData : ScriptableObject
{
    public ProjectileData projectileData;
    public HitboxData hitboxData;
    public HurtboxData hurtboxData;
    public DashData dashData;
    public string animatorTrigger;
    public float delayTime;
    public float rigidMultiply;
    // 필요하면 VFX, SFX, 추가 파라미터(넉백, 히트스탑 등) 추가
}