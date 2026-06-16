using UnityEngine;

public class HealAb : Ability
{

    protected override bool isCastable()
    {
        return true; 
    }

    protected override bool OnCast()
    {
        this.player_script_.GetHeal(Damage);

        return true;
    }
}
