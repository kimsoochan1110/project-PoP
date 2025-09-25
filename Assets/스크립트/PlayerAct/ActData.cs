using UnityEngine;

[CreateAssetMenu(fileName = "ActData", menuName = "Game/ActData")]
public class ActData : ScriptableObject
{
    public HitboxData hitboxData;
    public HurtboxData hurtboxData;
    public DashData dashData;
    public string animatorTrigger;
    public float delayTime;
    public float rigidMultiply;
    // 필요하면 VFX, SFX, 추가 파라미터(넉백, 히트스탑 등) 추가
}