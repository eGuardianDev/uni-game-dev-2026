using UnityEngine;

public class HealAb : Ability
{

    protected override bool isCastable()
    {
        return true; 
    }

    protected override bool OnCast()
    {
        this.player_script_.GetHealth(Damage);

        return true;
    }
}
