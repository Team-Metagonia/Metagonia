using UnityEngine;

public class ObjectStateSynchronizer : MonoBehaviour
{
    // 오브젝트와 스크립트를 인스펙터에서 할당받기 위해 public 변수로 선언
    public GameObject targetObject;
    public MonoBehaviour targetScript;

    // 매 프레임마다 오브젝트의 상태를 체크하고 스크립트의 상태를 동기화
    void Update()
    {
        // targetObject가 할당되어 있는지 확인
        if (targetObject != null && targetScript != null)
        {
            // 오브젝트가 활성화 상태인지 확인하고 스크립트의 활성화 상태 동기화
            if (targetObject.activeInHierarchy)
            {
                if (!targetScript.enabled)
                {
                    targetScript.enabled = true;
                    Debug.Log("Target script enabled.");
                }
            }
            else
            {
                if (targetScript.enabled)
                {
                    targetScript.enabled = false;
                    Debug.Log("Target script disabled.");
                }
            }
        }
    }
}
