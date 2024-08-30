using UnityEngine;

public class ConstructionModeUI : MonoBehaviour
{
    public GameObject constructionUIPanel; // 패널을 참조
    public Transform rightController; // 오른쪽 컨트롤러 참조
    public GameObject[] buildingPrefabs; // 건축 프리팹 배열
    private GameObject previewInstance; // 프리뷰 인스턴스
    private GameObject selectedPrefab; // 선택된 프리팹
    private bool isConstructionMode = false;

    void Update()
    {
        // 왼쪽 컨트롤러의 Y 버튼을 눌렀을 때
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch)) // Y 버튼으로 변경
        {
            ToggleConstructionMode();
        }

        if (isConstructionMode && selectedPrefab != null)
        {
            ShowPreview();
            constructionUIPanel.SetActive(false); // 버튼 선택 후 UI 비활성화
        }

        // 오른쪽 컨트롤러의 A 버튼을 눌렀을 때 건축
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch)) // A 버튼으로 변경
        {
            if (previewInstance != null)
            {
                Instantiate(selectedPrefab, previewInstance.transform.position, previewInstance.transform.rotation);
                Destroy(previewInstance);
                selectedPrefab = null;
                isConstructionMode = false; // 건축 후 건축 모드 비활성화
            }
        }
    }

    void ToggleConstructionMode()
    {
        if (!isConstructionMode || constructionUIPanel.activeSelf)
        {
            isConstructionMode = !isConstructionMode;
            constructionUIPanel.SetActive(isConstructionMode);
        }
        else
        {
            constructionUIPanel.SetActive(true);
        }
    }

    // 버튼 클릭 시 호출되는 메서드
    public void SelectBuilding(int index)
    {
        if (index >= 0 && index < buildingPrefabs.Length)
        {
            selectedPrefab = buildingPrefabs[index];
            if (previewInstance != null)
            {
                Destroy(previewInstance);
            }
            previewInstance = Instantiate(selectedPrefab);
            SetPreviewMaterial(previewInstance);
        }
    }

    void ShowPreview()
    {
        Ray ray = new Ray(rightController.position, rightController.forward);

        int terrainLayer = LayerMask.NameToLayer("Terrain");
        
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << terrainLayer))
        {
            previewInstance.transform.position = hit.point;
            previewInstance.transform.rotation = Quaternion.identity; // 필요 시 회전 조정
        }
    }

    void SetPreviewMaterial(GameObject preview)
    {
        // 투명한 재질 설정
        foreach (Renderer renderer in preview.GetComponentsInChildren<Renderer>())
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                Material transparentMaterial = new Material(Shader.Find("Standard"));
                transparentMaterial.CopyPropertiesFromMaterial(materials[i]);

                Color color = Color.white;
                color.a = 0.5f; // 투명도 설정
                transparentMaterial.color = color;

                transparentMaterial.SetFloat("_Mode", 3);
                transparentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                transparentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                transparentMaterial.SetInt("_ZWrite", 0);
                transparentMaterial.DisableKeyword("_ALPHATEST_ON");
                transparentMaterial.EnableKeyword("_ALPHABLEND_ON");
                transparentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                transparentMaterial.renderQueue = 3000;

                materials[i] = transparentMaterial;
            }
            renderer.materials = materials;
        }
    }
}