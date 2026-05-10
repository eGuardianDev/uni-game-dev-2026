using UnityEngine;

public class SimpleDamage : Ability
{

    protected override bool isCastable()
    {
        return player_script_.enemy_ != null;
    }
    protected override bool OnCast()
    {
        if(player_script_.enemy_ == null) return false;
        Debug.Log("Fireball!");
        player_script_.enemy_.GetDamage(this.Damage);
        return true;
    }
}
