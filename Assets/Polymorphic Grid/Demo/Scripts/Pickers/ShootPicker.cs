#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheoryTeam.PolymorphicGrid.Demo
{
    public class ShootPicker : TextPicker<int>
    {
        public ShootingHandler handler;

        private void Awake()
        {
            handler.OnPrefabsCountUpdatedCallback += Pick;
        }

        private void Start()
        {
            Pick(handler.CurrentPrefabsCount);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ShootPicker))]
    public class ShootPickerEditor : TextPickerEditor<int> { }
#endif
}
