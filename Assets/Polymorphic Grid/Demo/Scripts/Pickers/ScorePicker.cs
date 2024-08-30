#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheoryTeam.PolymorphicGrid.Demo
{
    public class ScorePicker : TextPicker<int>
    {
        public ScoreHandler targetHandler;
        public bool pickMaxScore;

        protected virtual void Awake()
        {
            if (!pickMaxScore)
                targetHandler.OnScoreUpdatedCallback += Pick;
        }

        protected virtual void OnEnable() => Pick(pickMaxScore ? targetHandler.MaxScore : targetHandler.Score);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ScorePicker))]
    public class ScorePickerEditor : TextPickerEditor<int> { }
#endif
}
