public interface IWeapon
{
    string ownerTag { get; set; }
    float attackSpeed { get; set; }
    float spriteAngle { get; set; }
    void Attack();
}
