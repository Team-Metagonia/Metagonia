using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public GameObject uiPanel;  // UI Panel을 여기서 연결
    public GameObject togglePrefab;  // Toggle 프리팹을 여기서 연결
    private List<SpearNPC> scriptObjects;
    private int currentToggleIndex = 0;
    private List<Toggle> toggles;
    private bool isUIActive = false;

    void Start()
    {
        uiPanel.SetActive(false);  // 초기에는 UI Panel을 숨김
        FindScriptObjects();
        PopulateUI();
        UpdateToggleSelection();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isUIActive = !isUIActive;
            uiPanel.SetActive(isUIActive);  // P키를 누르면 UI Panel을 토글
            if (isUIActive)
            {
                currentToggleIndex = 0;
                UpdateToggleSelection();
                EnableUIControls();
            }
            else
            {
                DisableUIControls();
            }
        }

        if (isUIActive)
        {
            HandleNavigation();
        }
    }

    void FindScriptObjects()
    {
        SpearNPC[] objects = FindObjectsOfType<SpearNPC>();
        scriptObjects = new List<SpearNPC>(objects);
        Debug.Log($"[UIManager] Found {scriptObjects.Count} SpearNPC objects.");
        foreach (var obj in scriptObjects)
        {
            Debug.Log($"[UIManager] Found SpearNPC object: {obj.name}");
        }
    }

    void PopulateUI()
    {
        toggles = new List<Toggle>();
        foreach (var obj in scriptObjects)
        {
            GameObject toggleObj = Instantiate(togglePrefab, uiPanel.transform);
            Toggle toggle = toggleObj.GetComponent<Toggle>();
            Text label = toggleObj.GetComponentInChildren<Text>();
            label.text = obj.name;
            toggle.isOn = obj.isSelected;
            toggle.onValueChanged.AddListener((isSelected) =>
            {
                obj.isSelected = isSelected;
                Debug.Log($"[UIManager] {obj.name} isSelected set to {isSelected}");
            });
            toggles.Add(toggle);
            Debug.Log($"[UIManager] Added toggle for {obj.name}");
        }
        AdjustPanelSize();
    }

    void HandleNavigation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentToggleIndex = (currentToggleIndex > 0) ? currentToggleIndex - 1 : toggles.Count - 1;
            UpdateToggleSelection();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentToggleIndex = (currentToggleIndex < toggles.Count - 1) ? currentToggleIndex + 1 : 0;
            UpdateToggleSelection();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            toggles[currentToggleIndex].isOn = !toggles[currentToggleIndex].isOn;
        }
    }

    void UpdateToggleSelection()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            var colors = toggles[i].colors;
            colors.normalColor = (i == currentToggleIndex) ? Color.yellow : Color.white;
            toggles[i].colors = colors;
            Debug.Log($"[UIManager] Toggle {i} ({toggles[i].GetComponentInChildren<Text>().text}) selection color updated.");
        }
    }

    void AdjustPanelSize()
    {
        // Adjust the size of the panel to fit the content
        var layoutGroup = uiPanel.GetComponent<VerticalLayoutGroup>();
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.childForceExpandWidth = false;
        Debug.Log("[UIManager] Adjusted panel size and alignment.");
    }

    void EnableUIControls()
    {
        // Restrict input to UI only
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("[UIManager] UI controls enabled.");
    }

    void DisableUIControls()
    {
        // Restore normal input controls
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("[UIManager] UI controls disabled.");
    }
}
