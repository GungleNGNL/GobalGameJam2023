
public struct DamageSource
{
    public Actor source;
    public int damage;

    public DamageSource(Actor source, int damage )
    {
        this.source = source;
        this.damage = damage;
    }
}
