
using System;
using Unity.VisualScripting;
using UnityEngine;

public class LinkDestroyer : MonoBehaviour
{
    private HingeJoint2D joint;
    private void Start()
    {
        joint = GetComponent<HingeJoint2D>();
    }

    private void OnJointBreak2D(Joint2D brokenJoint)
    {
        Debug.Log("Joint broken");
        if(brokenJoint==this)
            Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.transform.tag == "Ground")
        {
            Debug.Log("HitGround");
            Destroy(gameObject);
        }
        else if(collision.transform.tag=="Bowl")
        Destroy(gameObject);

    }
}
