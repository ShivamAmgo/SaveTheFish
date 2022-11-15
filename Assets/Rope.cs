using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] Rigidbody2D Hook;
    [SerializeField] GameObject LinkNodePrefab;
    [SerializeField] public int RopesCount=1;
    //[SerializeField] float OffsetLink=-0.3f;
    [SerializeField] int LinkCount = 8;
    public WeightFish weightFishobj;
    private bool IsFishWithROpe = false;
    private List<GameObject> AllLinks = new List<GameObject>();
    private bool LinksGenerated = false;
    private WeightFish ThisWeightFish;
    private bool Reparentable = true;
    private void Awake()
    {
        WeightFish.OnFishFreeFall += ReparentFish;
    }

    private void Start()
    {
        if (transform.tag=="Fish")
        {
            ThisWeightFish = GetComponent<WeightFish>();
            Debug.Log(ThisWeightFish.name);
        }
        
        GenerateRope(weightFishobj);
    }


   

    public List<GameObject> GetAllLinks()
    {
        return AllLinks;
    }
    //reparenting fish on freefall
    void ReparentFish(WeightFish Freefallingfish)
    {


        //Freefallingfish.Ropescript.RopesCount++;
        if (ThisWeightFish==null)
        {
            //Debug.Log(transform.name);
            return;
            
        }
        if (ThisWeightFish.RootParents.Contains(Freefallingfish.transform))
        {
            
            ThisWeightFish.RootParents.Remove(Freefallingfish.transform);
            Debug.Log(ThisWeightFish.name+"With Size "+ThisWeightFish.RootParents.Count+" Contains "+Freefallingfish.name+
                      "  As Parent with size"+Freefallingfish.RootParents.Count );
            RopesCount++;
            GenerateRope(Freefallingfish);
            
            Freefallingfish.RootParents.Remove(ThisWeightFish.transform);
        }
    }
    public void GenerateRope(WeightFish weightFishReparented)
    {
        if (RopesCount <= 0 || !Reparentable)
        {
            Debug.Log("Rope Returned");
            return;
        }

        Rigidbody2D PreviousRb = Hook;
        
        for (int i = 0; i < LinkCount; i++)
        {
            GameObject   GeneratedLink = Instantiate(LinkNodePrefab, transform);
            HingeJoint2D hingejoint = GeneratedLink.GetComponent<HingeJoint2D>();
            hingejoint.connectedBody = PreviousRb;
            //hingejoint.connectedAnchor = new Vector2(0, OffsetLink);
            
            if (i == LinkCount - 1)
            {
                if (weightFishReparented == null) return;
                
               
                weightFishReparented.ConnectFishToRope(GeneratedLink.GetComponent<Rigidbody2D>());
                //return;
            }
            //if (i! > 1) continue;
            //Debug.Log(PreviousRb.name);
            Transform[] points=new Transform[2];
            points[0] = PreviousRb.transform;
            points[1] = GeneratedLink.transform;
            GeneratedLink.GetComponentInChildren<Linerenderer>().SetupLine(points);
            PreviousRb = GeneratedLink.GetComponent<Rigidbody2D>();
            if (!LinksGenerated)
            {
                AllLinks.Add(GeneratedLink);
            }
            
        }

        LinksGenerated = true;
    }

    private void OnDestroy()
    {
        WeightFish.OnFishFreeFall -= ReparentFish;
    }
}
