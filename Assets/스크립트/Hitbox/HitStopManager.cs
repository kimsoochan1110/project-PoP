using System.Collections;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager Instance;

    private void Awake()
    {
        // 싱글톤 패턴 적용
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    
    public void DoHitStop(float duration)
    {
        StartCoroutine(HitStopRoutine(duration));
    }

    private IEnumerator HitStopRoutine(float duration)
    {
        // 게임을 멈춘다
        Time.timeScale = 0f;

        // 실시간 기준으로 대기 (Time.timeScale 영향을 받지 않음)
        yield return new WaitForSecondsRealtime(duration);

        // 게임을 다시 진행시킨다
        Time.timeScale = 1f;
    }
}
