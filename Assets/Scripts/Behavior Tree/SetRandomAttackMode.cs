using UnityEngine;

public class SetRandomAttackMode : MonoBehaviour
{
    public int numberOfAttackModes = 6; // Attack 모드의 갯수
    public float changeInterval = 0.1f; // 모드 변경 간격 (초)

    private Animator animator;
    private float timer = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 타이머 증가
        timer += Time.deltaTime;

        // 지정된 간격마다 랜덤 모드 설정
        if (timer >= changeInterval)
        {
            SetRandomMode();
            timer = 0f; // 타이머 리셋
        }
    }

    private void SetRandomMode()
    {
        if (animator == null)
        {
            Debug.LogError("[SetRandomAttackMode] SetRandomMode: Animator is null. Make sure the Animator component is attached.");
            return;
        }

        // AttackMode 설정 (랜덤 값)
        int mode = Random.Range(1, numberOfAttackModes + 1);
        animator.SetInteger("AttackMode", mode);
        Debug.Log($"[SetRandomAttackMode] Set AttackMode to {mode}");
    }
}