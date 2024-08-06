using UnityEngine.UI;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheoryTeam.PolymorphicGrid.Demo
{
    public class HealthPicker : MonoBehaviour, IPicker<float>
    {
        public HealthHandler targetHandler;
        public TextMeshProUGUI targetText;
        public Slider targetGraphic;

        private void Awake()
        {
            targetHandler.OnHealthUpdatedCallback += value => Pick(value / (float)targetHandler.maxHealth);
            Pick(targetHandler.CurrentHealth / (float)targetHandler.maxHealth);
        }

        public void Pick(float value)
        {
            targetGraphic.value = value;
            if (targetText != null)
                targetText.text = ((int)(value * 100)).ToString();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(HealthPicker))]
    public class HealthPickerEditor : Editor
    {
        private HealthPicker picker;

        private void OnEnable()
        {
            picker = target as HealthPicker;
            if (picker.targetGraphic == null)
                picker.targetGraphic = picker.GetComponent<Slider>();
            if (picker.targetText == null)
                picker.targetText = picker.GetComponentInChildren<TextMeshProUGUI>();
        }
    }
#endif
}
