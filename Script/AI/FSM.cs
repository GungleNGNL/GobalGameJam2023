using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum FSMState
{
    BaseAction,
    Chasing,
    Attack,
    Dead,
    DoNothing,
}
[System.Serializable]
public class Timer
{
    [SerializeField] private float duration = 0;
    private float remainTime = 0;
    public Timer(float time = 0)
    {
        ResetDuration(time);
    }
    public bool RunTimer(float time)
    {
        remainTime -= time;
        return remainTime <= 0;
    }
    public void UpdateTimer(float time)
    {
        if(remainTime > 0)
        {
            remainTime = (remainTime - time) < 0? 0: remainTime - time;
        }
    }
    public bool IsTimerFinish()
    {
        return remainTime <= 0;
    }
    public void ResetRemainTime()
    {
        remainTime += duration;
    }
    public void ResetDuration(float time)
    {
        duration = time;
        remainTime = time;
    }
}
public class FSM : MonoBehaviour
{

    [SerializeField] protected FSMState fsmState = FSMState.BaseAction;
    [SerializeField] protected GameObject attackTarget = null;
    [SerializeField] protected Timer scanTimer = new Timer(1);
    public UnityAction onDead;
    [SerializeField] protected bool debug = false;
    public bool isDead() => fsmState == FSMState.Dead;
    private void Awake()
    {
        StateReset();
    }

    protected virtual void OnDisable()
    {
        StateReset();
        attackTarget = null;
    }

    void FixedUpdate()
    {
        switch (fsmState)
        {
            case FSMState.BaseAction: BaseActionState(); break;
            case FSMState.Chasing: ChasingState(); break;
            case FSMState.Attack: AttackState(); break;
            case FSMState.Dead: DeadState(); break;

            default:
                break;
        }
        //if (fsmState != FSMState.Dead) OtherProcess();
    }
    //=================================================(State)=================================================
    protected virtual void StateReset() => ChangeState(FSMState.BaseAction);
    protected virtual void ChangeState(FSMState state)
    {
        fsmState = state;
    } 
    protected virtual void BaseActionState() 
    {
        if (scanTimer.RunTimer(Time.deltaTime))
        {
            scanTimer.ResetRemainTime();
            ScanTaget();
        }
    }
    protected virtual void ChasingState() { }
    protected virtual void AttackState()
    {
        if (attackTarget == null || !attackTarget.activeSelf ||
            !IsInRange(MyAttributes().AttackRange))
        {
            attackTarget = null;
            StateReset();
            return;
        }
        Attack();
    }
    protected virtual void DeadState() { }
    protected virtual void OtherProcess()
    {
        if (MyAttributes().Hp <= 0)
        {
            fsmState = FSMState.Dead;
            onDead.Invoke();
        }
    }
    //=================================================(State)=================================================
    protected virtual void Attack() 
    {

    }

    protected virtual FSMState AfterScanState() => FSMState.Attack;
    protected bool IsInRange(float range)
    {
        var v = attackTarget.transform.position - transform.position;
        v.y = 0;
        return v.magnitude <= range;
    }
    protected virtual float ScanRange() => MyAttributes().AttackRange;
    [SerializeField] protected LayerMask EnemyLayer;
    protected virtual AttackableObject MyAttributes() => GetComponent<AttackableObject>();
    protected virtual void ScanTaget()
    {
        if(Physics.CheckSphere(transform.position, ScanRange(), EnemyLayer))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, ScanRange(), EnemyLayer);
            float distance = 0;
            foreach (Collider hit in hits)
            {
                if (!hit.gameObject.activeSelf)
                    continue;

                if (hit.transform.root.GetComponent<Actor>() != null)
                {
                    var v = hit.transform.position - transform.position;
                    if (v.magnitude <= distance || distance == 0)
                    {
                        distance = v.magnitude;
                        attackTarget = hit.transform.root.gameObject;
                    }
                }
            }
        }
        if(attackTarget != null)
        {
            ChangeState(AfterScanState());
        }
    }
}