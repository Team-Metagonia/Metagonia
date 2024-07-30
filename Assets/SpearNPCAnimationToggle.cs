using UnityEngine;
using System.Collections;

public class SpearNPCAnimationToggle : MonoBehaviour
{
    private SpearNPC spearNPC;
    private Animator animator;
    private bool previousSelected = false;

    void Start()
    {
        spearNPC = GetComponent<SpearNPC>();
        if (spearNPC == null)
        {
            Debug.LogError("No SpearNPC component found on " + gameObject.name);
            return;
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No Animator component found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (spearNPC.isSelected && !previousSelected)
        {
            // 애니메이션 모드 설정
            UpdateAnimator();
        }

        previousSelected = spearNPC.isSelected;
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetInteger("AnimationMode", 5);
            StartCoroutine(ResetAnimationMode());
        }
    }

    private IEnumerator ResetAnimationMode()
    {
        yield return new WaitForSeconds(0.1f);

        if (animator != null)
        {
            animator.SetInteger("AnimationMode", 1);
        }
    }
}