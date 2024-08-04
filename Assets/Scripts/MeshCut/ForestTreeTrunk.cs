using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestTreeTrunk : MonoBehaviour
{
    public ItemSO itemInfo;
    public ForestTreeRoot other;
    public float respawnCooldown;
    
    [SerializeField] private float dieDelayTime = 2.5f;

    private bool isDying = false;
    private bool isDead = false;
    
    private void Start()
    {
        DieAfterDelay(respawnCooldown);
    }
    
    private void Update()
    {
        
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        
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
            DieAfterDelay(dieDelayTime);
        }
    }

    private IEnumerator IEDieAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Die();
    }

    private void DieAfterDelay(float delayTime)
    {
        StartCoroutine(IEDieAfterDelay(dieDelayTime));
    }
}
