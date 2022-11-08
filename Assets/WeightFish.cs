using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightFish : MonoBehaviour
{
    [SerializeField] float AnchorOffset = -0.2f;
    [SerializeField] ParticleSystem DieEffect;
    [SerializeField] GameObject Hook;
    float Timer = 0;
    public delegate void FishSaved(WeightFish fish);
    public delegate void BroadcastFishInfo(WeightFish fishInfo);
    public delegate void FishFallOnGround(WeightFish FallenFish);
    public delegate void FishDead(WeightFish DeadFish);

    public static event FishDead onFishDead;
    public static event FishFallOnGround OnFishFallen;
    public static event BroadcastFishInfo OnFishInfoBradcast;
    public static FishSaved onFishSaved;
    bool CheckFishSafety = false;
    bool safetyacheived = false;

    HingeJoint2D[] JointTodispose;
    private void Awake()
    {
        
    }
    private void Start()
    {
        OnFishInfoBradcast?.Invoke(this);
    }
    private void Update()
    {
        if (!CheckFishSafety) return;
        CheckSafety();

    }
  
    public void ConnectFishToRope(Rigidbody2D RB)
    {
        HingeJoint2D joint = gameObject.AddComponent<HingeJoint2D>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = RB;
        joint.anchor = Vector2.zero;
        joint.connectedAnchor = new Vector2(0, AnchorOffset);
        JointTodispose = gameObject.GetComponents<HingeJoint2D>();
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
        onFishDead?.Invoke(this);
    }


}