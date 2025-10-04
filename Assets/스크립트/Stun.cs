using UnityEngine;

public class Stun : MonoBehaviour
{
    public bool canAct = true;
    public Animator animator; 
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void SetStun(float stunTime)
    {
        animator.SetBool("isStunned", true);
        canAct = false; //행동 불가능
        
        Invoke("ResetStun", stunTime); // 일정 시간 후 행동 가능
    }
    public void ResetStun()
    {
        animator.SetBool("isStunned", false);
        canAct = true; // 행동 가능 상태로 변경
        
    }
}
