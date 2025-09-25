using UnityEngine;

public class Stun : MonoBehaviour
{
    public bool canAct = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetStun(float stunTime)//딜레이 함수 요긴하게 써먹자
    {
        canAct = false; //행동 불가능
        Invoke("ResetStun",stunTime); // 일정 시간 후 행동 가능
    }
    public void ResetStun()
    {
        canAct = true; // 행동 가능 상태로 변경
    }
}
