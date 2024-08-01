using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestTreeTrunk : MonoBehaviour
{
    public ItemSO itemInfo;
    public ForestTreeRoot other;

    [SerializeField] private float dieDelayTime = 2.5f;

    private bool isDying = false;
    
    private void Start()
    {
        
    }
    
    private void Update()
    {
        
    }

    private void Die()
    {
        other.Die();
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDying) return;
        
        int terrainLayer = LayerMask.NameToLayer("Terrain");
        if (collision.gameObject.layer == terrainLayer)
        {
            isDying = true;
            StartCoroutine(DieAfterDelay(dieDelayTime));
        }
    }

    private IEnumerator DieAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Die();
    }
}
