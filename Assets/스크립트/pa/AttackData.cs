using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Game/AttackData")]
public class AttackData : ScriptableObject
{
    public string displayName;
    public HitboxData hitboxData;
    public HurtboxData hurtboxData;
    public string animatorTrigger = "attackTrigger";
    public float velocityMultiplier = 0.3f;
    public float delayTime = 0.4f;
    // 필요하면 VFX, SFX, 추가 파라미터(넉백, 히트스탑 등) 추가
}