using UnityEngine;

public class FrameActivationManager : MonoBehaviour
{
    [Header("Objects to Check")]
    public GameObject[] objectsToCheck; // 비활성화 여부를 확인할 오브젝트 배열

    [Header("Objects to Activate")]
    public GameObject[] objectsToActivate; // 활성화할 오브젝트 배열

    [Header("Prefab to Replace With")]
    public GameObject newPrefab; // 교체할 새로운 프리팹

    private bool _allConditionsMet = false; // 모든 조건이 충족되었는지 여부
    private bool _prefabReplaced = false; // 프리팹이 이미 교체되었는지 여부

    private void Start()
    {
        // 초기에는 활성화할 오브젝트들을 비활성화
        foreach (var obj in objectsToActivate)
        {
            obj.SetActive(false);
        }
    }

    private void Update()
    {
        // 모든 조건이 충족되었는지 확인
        if (!_allConditionsMet && AreAllConditionsMet())
        {
            ActivateObjects();
        }

        // 조건이 충족된 후 오브젝트 상태를 검사하여 프리팹 교체 여부 결정
        if (_allConditionsMet && !_prefabReplaced)
        {
            if (AreAllObjectsInActivateArrayInactive())
            {
                ReplacePrefab();
            }
        }
    }

    private bool AreAllConditionsMet()
    {
        foreach (var obj in objectsToCheck)
        {
            // 오브젝트가 활성화되어 있으면 false 반환
            if (obj.activeSelf)
            {
                return false;
            }
        }
        return true; // 모든 오브젝트가 비활성화되었으면 true 반환
    }

    private bool AreAllObjectsInActivateArrayInactive()
    {
        foreach (var obj in objectsToActivate)
        {
            // 하나라도 활성화된 오브젝트가 있으면 false 반환
            if (obj.activeSelf)
            {
                return false;
            }
        }
        return true; // 모든 오브젝트가 비활성화된 경우 true 반환
    }

    private void ActivateObjects()
    {
        foreach (var obj in objectsToActivate)
        {
            obj.SetActive(true);
        }
        _allConditionsMet = true; // 조건 충족 상태로 설정
    }

    private void ReplacePrefab()
    {
        // 현재 게임 오브젝트를 새로운 프리팹으로 교체
        if (newPrefab != null)
        {
            // 새로운 프리팹을 생성
            GameObject newObject = Instantiate(newPrefab, transform.position, transform.rotation);

            // 새로운 오브젝트에 기존 오브젝트의 이름을 할당
            newObject.name = gameObject.name;

            // 기존 오브젝트를 제거
            Destroy(gameObject);

            _prefabReplaced = true; // 프리팹 교체 상태로 설정
        }
    }
}
