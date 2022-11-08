using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] Rigidbody2D Hook;
    [SerializeField] GameObject LinkNodePrefab;
    [SerializeField] int RopesCount=1;
    //[SerializeField] float OffsetLink=-0.3f;
    [SerializeField] int LinkCount = 8;
    public WeightFish weightFish;
    
    private void Start()
    {
        GenerateRope();
    }
  
    void GenerateRope()
    {
        if (RopesCount <=0) return;
        Rigidbody2D PreviousRb = Hook;
        
        for (int i = 0; i < LinkCount; i++)
        {
            GameObject   GeneratedLink = Instantiate(LinkNodePrefab, transform);
            HingeJoint2D hingejoint = GeneratedLink.GetComponent<HingeJoint2D>();
            hingejoint.connectedBody = PreviousRb;
            //hingejoint.connectedAnchor = new Vector2(0, OffsetLink);
            
            if (i == LinkCount - 1)
            {
                if (weightFish == null) return;
                weightFish.ConnectFishToRope(GeneratedLink.GetComponent<Rigidbody2D>());
                //return;
            }
            //if (i! > 1) continue;
            //Debug.Log(PreviousRb.name);
            Transform[] points=new Transform[2];
            points[0] = PreviousRb.transform;
            points[1] = GeneratedLink.transform;
            GeneratedLink.GetComponentInChildren<Linerenderer>().SetupLine(points);
            PreviousRb = GeneratedLink.GetComponent<Rigidbody2D>();
        }
    }
}
