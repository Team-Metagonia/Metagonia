using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void RespawnAfterDelay(GameObject obj, float delayTime)
    {
        StartCoroutine(IERespawnAfterDelay(obj, delayTime));
    }
    
    public IEnumerator IERespawnAfterDelay(GameObject obj, float delayTime)
    {
        obj.SetActive(false);
        yield return new WaitForSeconds(delayTime);
        obj.SetActive(true);
    }

    public void SpawnObject(GameObject go, Transform spawnPoint)
    {
        Instantiate(go, spawnPoint.transform.position, Quaternion.identity);
    }
    
    
}
