public interface IDamagable
{
    int HP { get; set; }
    string colliderToIgnore { get; set; }
    void GetDamage(int damage);
    void Die();
}
