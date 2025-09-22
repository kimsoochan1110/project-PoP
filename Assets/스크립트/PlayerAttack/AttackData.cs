using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Game/AttackData")]
public class AttackData : ScriptableObject
{
    public HitboxData hitboxData;
    public HurtboxData hurtboxData;
    public DashData dashData;
    public string animatorTrigger;
    public float delayTime;
    // 필요하면 VFX, SFX, 추가 파라미터(넉백, 히트스탑 등) 추가
}