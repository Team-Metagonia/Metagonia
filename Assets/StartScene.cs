using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public string mainSceneName;
    public string tutorialSceneName;
    
    public void EnterMain()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    public void EnterTutorial()
    {
        SceneManager.LoadScene(tutorialSceneName);        
    }
}
