using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace TheoryTeam.PolymorphicGrid.Demo
{
    public class GameManagement : MonoBehaviour
    {
        public GridBasedSpawner enemySpawner;
        public MapVisualizer visualizer;
        public UserInput user;
        public GameObject losePanel;

        private void Lose()
        {
            losePanel.SetActive(true);
            user.gameObject.SetActive(false);
            StartCoroutine(LoadScene());
        }

        private void Start()
        {
            enemySpawner.Enabled = true;
            user.GetComponent<HealthHandler>().OnDieCallback += Lose;
        }

        private void Update()
        {
            enemySpawner.Update(Time.deltaTime);
        }

        private IEnumerator LoadScene()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}