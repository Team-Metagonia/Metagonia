using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitySpawn : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject go;
        
    public void Spawn()
    {
        SpawnManager.Instance.SpawnObject(go, spawnPoint);
    }
}
