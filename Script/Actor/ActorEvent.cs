using UnityEngine.Events;

public class ActorEvent
{
    public UnityAction<DamageSource> OnDamage;
    public UnityAction OnDestory;
    public UnityAction<Actor> OnDie;
}
