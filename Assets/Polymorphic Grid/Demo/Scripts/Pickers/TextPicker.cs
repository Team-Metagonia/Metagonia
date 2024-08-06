using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheoryTeam.PolymorphicGrid.Demo
{
    public class TextPicker<T> : MonoBehaviour, IPicker<T>
    {
        public TextMeshProUGUI targetGraphic;
        public string prefix;
        public string sufix;

        public void Pick(T value) => targetGraphic.text = prefix + value.ToString() + sufix;
    }

#if UNITY_EDITOR
    public class TextPickerEditor<T> : Editor
    {
        protected TextPicker<T> picker;

        protected virtual void OnEnable()
        {
            picker = target as TextPicker<T>;
            if (picker.targetGraphic == null)
                picker.targetGraphic = picker.GetComponent<TextMeshProUGUI>();
        }
    }
#endif
}
