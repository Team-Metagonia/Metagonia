using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public bool FollowTarget;
    public void WhenHover()
    {
        Debug.Log("Hover");
    }

    public void UnHover()
    {
        Debug.Log("UnHover");
    }

    public void Selected()
    {
        Debug.Log("Selected");
        FollowTarget = true;
    }

    public void UnSelect()
    {
        Debug.Log("UnSelected");
        FollowTarget = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(FollowTarget)
        {
            //this.gameObject.transform.position = GameObject.Find("Proxy RigidBody").transform.position;
            //this.gameObject.transform.rotation = GameObject.Find("Proxy RigidBody").transform.rotation;
        }
    }
}
