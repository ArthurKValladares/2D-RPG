using UnityEngine;

public class SkillObject_SwordPierce : SkillObject_Sword
{
    private int pierceAmount;

    public override void SetupSword(Skill_SwordThrow swordThrow, Vector2 throwForce)
    {
        base.SetupSword(swordThrow, throwForce);

        pierceAmount = swordThrow.pierceAmount;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        bool groundHit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");

        if (pierceAmount <= 0 || groundHit)
        {
            base.OnTriggerEnter2D(collision);
        } else
        {
            DamageEnemies();
            --pierceAmount;
        }
        
    }
}
