using UnityEngine;

public class FrameUI : MonoBehaviour
{
    public GameObject constructionUIPanel; // 패널을 참조
    public Transform rightController; // 오른쪽 컨트롤러 참조
    public GameObject[] buildingPrefabs; // 완성된 건축 프리팹 배열
    public GameObject[] framePrefabs; // 프레임 프리팹 배열
    private GameObject previewInstance; // 프리뷰 인스턴스
    private GameObject selectedFramePrefab; // 선택된 프레임 프리팹
    private GameObject selectedBuildingPrefab; // 선택된 건축 프리팹
    private bool isConstructionMode = false;

    void Update()
    {
        // 왼쪽 컨트롤러의 X 버튼을 눌렀을 때
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            ToggleConstructionMode();
        }

        if (isConstructionMode && selectedFramePrefab != null)
        {
            ShowPreview();
        }

        // 오른쪽 컨트롤러의 B 버튼을 눌렀을 때 건축
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) && previewInstance != null)
        {
            Instantiate(selectedFramePrefab, previewInstance.transform.position, previewInstance.transform.rotation);
            Destroy(previewInstance);
            selectedFramePrefab = null;
            selectedBuildingPrefab = null;
        }
    }

    void ToggleConstructionMode()
    {
        isConstructionMode = !isConstructionMode;
        constructionUIPanel.SetActive(isConstructionMode);
    }

    // 버튼 클릭 시 호출되는 메서드
    public void SelectBuilding(int index)
    {
        if (index >= 0 && index < buildingPrefabs.Length && index < framePrefabs.Length)
        {
            selectedBuildingPrefab = buildingPrefabs[index];
            selectedFramePrefab = framePrefabs[index];
            if (previewInstance != null)
            {
                Destroy(previewInstance);
            }
            previewInstance = Instantiate(selectedFramePrefab);
            SetPreviewMaterial(previewInstance);
        }
    }

    void ShowPreview()
    {
        Ray ray = new Ray(rightController.position, rightController.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
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
