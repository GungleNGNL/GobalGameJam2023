using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyFSM : FSM
{
    protected EnemyAttributes enemyAttributes;
    protected NavMeshPath path;
    private int pathIndex = 0;
    private CharacterController cc;
    [SerializeField]protected Animator animator;
    
    protected override AttackableObject MyAttributes() => enemyAttributes;
    protected NavMeshAgent m_NavMeshAgent;
    protected void SetNavDestination(Vector3 pos) => m_NavMeshAgent.SetDestination(pos);
    protected override float ScanRange() => enemyAttributes.DetectRange();
    //protected LayerMask EnemyLayer;
    protected override FSMState AfterScanState() => FSMState.Chasing;
    protected Timer attackTimer = new Timer(1);
    private void Awake()
    {
        enemyAttributes = gameObject.GetComponent<EnemyAttributes>();
        attackTimer.ResetDuration(enemyAttributes.AttackDuration);
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        if (!TryGetComponent(out cc))
        {
            cc = GetComponentInChildren<CharacterController>();
        }
        if (!TryGetComponent(out animator))
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void OnEnable()
    {
        m_NavMeshAgent.CalculatePath(BuildManager.Instance.BaseInstance.transform.position, path);
    }

    protected override void ChangeState(FSMState state)
    {
        base.ChangeState(state);
        if (!gameObject.activeSelf) return;
        if (state == FSMState.Chasing ||
            state == FSMState.Attack){
            m_NavMeshAgent.CalculatePath(attackTarget.transform.position, path);
        }
        //Animator
        if(state == FSMState.Attack)
        {
            animator.SetBool("IsAttacking", true);
        }
        else 
        {  
            animator.SetBool("IsAttacking", false);
        }
    }

    protected override void BaseActionState() {
        base.BaseActionState();
        Moving();
    }

    protected override void ChasingState()
    {
        base.ChasingState();
        if (attackTarget == null)
        {
            StateReset();
            return;
        }
        if(IsInRange(enemyAttributes.AttackRange))
        {
            ChangeState(FSMState.Attack);
        }
        else
        {
            Moving();
        }
    }

    protected override void AttackState()
    {
        base.AttackState();
        Rotate(attackTarget.transform.position - transform.position);
    }

    protected override void ScanTaget()
    {
        base.ScanTaget();
    }

    private void Moving()
    {
        if (pathIndex >= path.corners.Length) return;
        var v = path.corners[pathIndex] - transform.position;
        if (v.magnitude <= 10)
        {
            if (pathIndex < path.corners.Length -1)
            {
                pathIndex++;
                v = path.corners[pathIndex] - transform.position;
            }
        }
        Moving(v);
        Rotate(v);
    }

    private void Moving(Vector3 v)
    {
        v = enemyAttributes.MovingSpeed() * Time.deltaTime * v.normalized;
        cc.Move(v);       
    }

    private void Rotate(Vector3 v)
    {
        float angle = Vector3.SignedAngle(v, transform.forward, Vector3.up);
        if (angle > 5f || angle < -5f)
        {
            float x = angle > 0 ? -1f : 1f;
            transform.Rotate(0, x * Time.deltaTime * enemyAttributes.RotSpeed(), 0);
        }
        else
        {
            transform.Rotate(0, 0, 0);
        }
    }
    protected override void Attack()
    {
        base.Attack();
        if (attackTimer.RunTimer(Time.deltaTime))
        {           
            attackTimer.ResetRemainTime();
            enemyAttributes.PerformAttack(attackTarget.GetComponent<Actor>());
        }
    }

    protected override void OtherProcess()
    {
        base.OtherProcess();
        if(fsmState != FSMState.Attack) attackTimer.UpdateTimer(Time.deltaTime);
    }
}
