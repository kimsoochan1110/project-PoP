    using UnityEngine;

public class SandBag : MonoBehaviour
{
    public HurtboxData hurtboxData;
    public HurtboxController hurtboxController;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
        void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnableHurtboxAtFrame(int frameIndex) //허트박스 프레임 감지
    {
        hurtboxController.EnableHurtboxAtFrame(frameIndex);
    }
    public void ResetHutbox()
    {
        hurtboxController.ResetHurtbox();

    }
    public void DisableHurtbox() //허트박스 끄기
    {
        hurtboxController.DisableHurtbox();
    }
}
