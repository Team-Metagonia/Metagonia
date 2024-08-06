using UnityEngine;
using System;

namespace TheoryTeam.PolymorphicGrid.Demo
{
    public class ScoreHandler : MonoBehaviour
    {
        /// <summary>
        /// String key used in PlayerPrefs to serialize max socre.
        /// </summary>
        public static readonly string serializedMaxScore = "Max Score";

        /// <summary>
        /// This ID will be used to save score on disk.
        /// </summary>
        public int id;

        /// <summary>
        /// This callback will automatically be called on score change.
        /// </summary>
        public event Action<int> OnScoreUpdatedCallback;

        /// <summary>
        /// Current score value in the active level.
        /// </summary>
        public int Score
        {
            get => score;
            private set
            {
                OnScoreUpdatedCallback?.Invoke(value);
                score = value;
            }
        }

        /// <summary>
        /// Max socre reached by this handler using it's id.
        /// </summary>
        public int MaxScore
        {
            get => PlayerPrefs.HasKey(serializedMaxScore + id) ? PlayerPrefs.GetInt(serializedMaxScore + id) : Score;
            private set
            {
                if (!PlayerPrefs.HasKey(serializedMaxScore + id) || value > MaxScore)
                    PlayerPrefs.SetInt(serializedMaxScore + id, value);
            }
        }

        private int score;

        private void OnDestroy()
        {
            UpdateMaxScore();
        }

        /// <summary>
        /// Increase current score value.
        /// </summary>
        /// <param name="amount">Score amount to increase</param>
        public void IncreaseScore(int amount) => Score += amount;

        /// <summary>
        /// Update serialized max score on disk.
        /// </summary>
        public void UpdateMaxScore() => MaxScore = Score;
    }
}
