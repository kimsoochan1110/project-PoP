// 이 스크립트는 일정 시간 동안 행동을 지연시키는 기능을 제공합니다.
using UnityEngine;

public class Delay : MonoBehaviour
{
    public bool canAct = true; // 행동 가능 여부
    
    public void SetDelay(float delayTime)//딜레이 함수 요긴하게 써먹자
    {
        canAct = false; //행동 불가능
        Invoke("ResetDelay",delayTime); // 일정 시간 후 행동 가능
    }
    public void ResetDelay()
    {
        canAct = true; // 행동 가능 상태로 변경
    }
}
