// 이 스크립트는 플레이어의 상태를 관리하는 컴포넌트입니다.
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public int life = 3;
    public int bulletCount;

    public float InvincibilityFrame;
    public float stunTimeEditor;
    public float knuckbackEditor;
}
