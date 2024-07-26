using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public GameObject uiPanel;  // UI Panel을 여기서 연결
    public GameObject togglePrefab;  // Toggle 프리팹을 여기서 연결
    private List<IsNPCSelected> scriptObjects;
    private int currentToggleIndex = 0;
    private List<Toggle> toggles;

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
            uiPanel.SetActive(!uiPanel.activeSelf);  // P키를 누르면 UI Panel을 토글
            if (uiPanel.activeSelf)
            {
                currentToggleIndex = 0;
                UpdateToggleSelection();
            }
        }

        if (uiPanel.activeSelf)
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
    }

    void FindScriptObjects()
    {
        IsNPCSelected[] objects = FindObjectsOfType<IsNPCSelected>();
        scriptObjects = new List<IsNPCSelected>(objects);
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
            toggle.onValueChanged.AddListener((isSelected) =>
            {
                obj.isSelected = isSelected;
            });
            toggles.Add(toggle);
        }
    }

    void UpdateToggleSelection()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            var colors = toggles[i].colors;
            colors.normalColor = (i == currentToggleIndex) ? Color.yellow : Color.white;
            toggles[i].colors = colors;
        }
    }
}
