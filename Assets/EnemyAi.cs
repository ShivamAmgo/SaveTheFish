using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyState
{
    Idle,
    Chase,
    Attack
    
}

public class EnemyAi : MonoBehaviour
{
    List<WeightFish> TargetsToAttack = new List<WeightFish>();
    EnemyState enemyState;
    Vector3 Dir = Vector3.zero;
    Transform Target;
    [SerializeField] float MovementSpeed = 5;
    [SerializeField] CircleCollider2D AttackRegion;
    [SerializeField] float AttackDistance=2;
    
    Animator anim;
    int move = Animator.StringToHash("IsMoving");
    bool IsMoving = false;
    private void Awake()
    {
        WeightFish.OnFishFallen += FallenFishCheck;
        WeightFish.onFishDead += OnFishKilled;
        //WeightFish.OnFishInfoBradcast += AcquireTargetFishInfo;
    }

    

    private void Start()
    {
        enemyState = EnemyState.Idle;
        anim = GetComponent<Animator>();
        
    }
    private void OnFishKilled(WeightFish DeadFish)
    {
        if (TargetsToAttack.Contains(DeadFish))
            TargetsToAttack.Remove(DeadFish);
        if (TargetsToAttack.Count > 0)
        {
            Target = TargetsToAttack[0].transform;
            RotateTowardsTarget();
            
        }
            
        else
        {
            enemyState = EnemyState.Idle;
        }
    }
    public void AttackTrigger()
    {
        //Debug.Log(AttackRegion.isActiveAndEnabled + " region");
        if (AttackRegion.isActiveAndEnabled)
        {
            AttackRegion.enabled = false;
            anim.ResetTrigger("Attack");
        }
            
        else
            AttackRegion.enabled = true;
    }
    private void FallenFishCheck(WeightFish FallenFish)
    {
        if (TargetsToAttack.Contains(FallenFish)) return;
        TargetsToAttack.Add(FallenFish);
        

        if (Target != TargetsToAttack[0])
        {
            Target = TargetsToAttack[0].transform;
            RotateTowardsTarget();
        }

        if (enemyState != EnemyState.Chase)
            enemyState = EnemyState.Chase;

    }
    private void Update()
    {
        anim.SetBool(move, IsMoving);
        if (enemyState == EnemyState.Chase)
            Chase();
        else if (enemyState == EnemyState.Attack)
            Attack();
        else if (enemyState == EnemyState.Idle)
            Idle();

    }
    void RotateTowardsTarget()
    {
        
         Dir= (Target.position - transform.position).normalized;

        Vector3 forward = transform.TransformDirection(Vector3.right);
        //Vector3 toOther = Target.position - transform.position;
        float dotproduct = Vector3.Dot(forward, Dir);
        //Debug.Log(dotproduct);
        if (dotproduct < 0)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y+180, 0);
        }
        //else
            //transform.eulerAngles = Vector3.zero;
    }
    void Chase()
    {


        //Look.LookAt(Target);
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x * Dir.x, transform.eulerAngles.y, transform.eulerAngles.z);
        if (AttackRegion.isActiveAndEnabled) return;
        if (Vector3.Distance(Target.position, transform.position) < AttackDistance)
        {
            enemyState = EnemyState.Attack;
            return;
        }

        IsMoving = true;
        transform.position += new Vector3(Time.deltaTime * MovementSpeed*Dir.x, 0, 0);
        //transform.eulerAngles = new Vector3(0, 0, 0);

        //transform.position = Vector2.MoveTowards(transform.position, Target.position, MovementSpeed*Time.deltaTime);
    }
    void Attack()
    {
        IsMoving = false;
        
        if (Vector3.Distance(Target.position, transform.position) > AttackDistance)
        {
            enemyState = EnemyState.Chase;
            return;
        }
        anim.SetTrigger("Attack");
    }
    void Idle()
    {
        IsMoving = false;
        anim.SetTrigger("Idle");
    }
    private void OnDestroy()
    {
        WeightFish.OnFishFallen -= FallenFishCheck;
        
        WeightFish.onFishDead -= OnFishKilled;
        //WeightFish.OnFishInfoBradcast -= AcquireTargetFishInfo;
    }
    
    
}
