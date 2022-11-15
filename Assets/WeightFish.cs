using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightFish : MonoBehaviour
{
    [SerializeField] float AnchorOffset = -0.2f;
    [SerializeField] ParticleSystem DieEffect;
    [SerializeField] GameObject Hook;
    [SerializeField] public Rope Ropescript;
    public List<Transform> RootParents = new List<Transform>();
    private WeightFish ParentFish;
    private WeightFish ChildFish;
    float Timer = 0;
    public delegate void FishSaved(WeightFish fish);
    public delegate void BroadcastFishInfo(WeightFish fishInfo);
    public delegate void FishFallOnGround(WeightFish FallenFish);
    public delegate void FishDead(WeightFish DeadFish);
    public delegate void FishFreeFall(WeightFish FallenRopeFish);


    public static event FishFreeFall OnFishFreeFall;
    public static event FishDead onFishDead;
    public static event FishFallOnGround OnFishFallen;
    public static event BroadcastFishInfo OnFishInfoBradcast;
    public static FishSaved onFishSaved;
    
    
    
    bool CheckFishSafety = false;
    bool safetyacheived = false;
    private bool CanLinksBeReparented = true;

    HingeJoint2D[] JointTodispose;
    Rigidbody2D rb;
    private void Awake()
    {
        LinkDestroyer.Ondestroy += OnLinkDestroyed;
    }
    private void Start()
    {
        OnFishInfoBradcast?.Invoke(this);
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (!CheckFishSafety) return;
        CheckSafety();

    }

    
    void OnLinkDestroyed(Transform RootTransformOfLink)
    {
        if(RootTransformOfLink==transform)
        {
            CanLinksBeReparented = false;
            Debug.Log(CanLinksBeReparented+" Reparentable "+RootTransformOfLink.name);
          
        }
        //Debug.Log("total parents "+RootParents.Count+" , root link "+RootTransformOfLink.name);
        if (RootParents.Count<=0)return;
       
        if (!RootParents.Contains(RootTransformOfLink))
        {
            Debug.Log("returned from :"+transform.name);
            return;
        }
        //Debug.Log("contains "+RootTransformOfLink.name);
        
            RootParents.Remove(RootTransformOfLink);
            
        if (RootParents.Count <= 0)
        {
            if (!CanLinksBeReparented)
            {
                Debug.Log("cannot reparent falling fish "+transform.name);
                return;
            }
            DestroyLinks(Ropescript.GetAllLinks());
            OnFishFreeFall?.Invoke(this);
            
            /*
            ChildFish.GetComponent<Rope>().SetReparentRope(this);
            Ropescript.DestroyLinks();
            */
            //Ropescript.weightFish.SetUpRelativeJointActive();
        }

    }
    public void DestroyLinks(List<GameObject> AllLinks)
    {
        foreach (GameObject l in AllLinks)
        {
            Destroy(l);
        }
    }
 

    public void SetChildFish(WeightFish Child)
    {
        ChildFish = Child;
    }
    public void ConnectFishToRope(Rigidbody2D RB)
    {
        HingeJoint2D joint = gameObject.AddComponent<HingeJoint2D>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = RB;
        joint.anchor = Vector2.zero;
        joint.connectedAnchor = new Vector2(0, AnchorOffset);
        JointAngleLimits2D limits = joint.limits;
        limits.min = -20;
        limits.max = 20;
        joint.limits = limits;
        
        joint.useLimits = true;
        
        JointTodispose = gameObject.GetComponents<HingeJoint2D>();
        RootParents.Add(RB.transform.root);
        //Debug.Log("this "+transform.name+" parent "+RB.transform.root.name);
    }

    void CheckSafety()
    {
        Timer += Time.deltaTime;
        if (Timer >= 1.25f)
        {
            CheckFishSafety = false;
            SaveFish();
            return;
        }
    }
    void SaveFish()
    {
        safetyacheived = true;
        if(JointTodispose==null || JointTodispose.Length!>=0)return;
        for (int i = 0; i < JointTodispose.Length; i++)
        {
            JointTodispose[i].enabled = false;
        }
        Destroy(Hook);
        onFishSaved?.Invoke(this);
    }
     IEnumerator Die()
    {
        //ParticleSystem.Particle.d
        
        DieEffect.Emit(5);
        onFishDead?.Invoke(this);
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Bowl" && !safetyacheived)
        {
            Debug.Log("Checking safety");
            CheckFishSafety = true;
        }
        else if (collision.transform.tag == "Ground")
        {
            Debug.Log("Fish fallen");
            OnFishFallen?.Invoke(this);
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
        else if (collision.transform.tag == "Enemy")
        {
            StartCoroutine("Die");
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Bowl")
            Debug.Log("trigger Exit");
        CheckFishSafety = false;
        Timer = 0;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Bowl"&& !safetyacheived)
            CheckFishSafety = true;
    }
    private void OnDestroy()
    {
        LinkDestroyer.Ondestroy -= OnLinkDestroyed;
    }


}
