using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class AbilityManager : MonoBehaviour
{
    public List<Ability> abilities = new List<Ability>(4);
    public List<Image> images = new List<Image>(4);
    [SerializeField] private PlayerScript player_script_;

    void Start()
    {
        player_script_ = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
    }
 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) TryCast(0);
        if (Input.GetKeyDown(KeyCode.W)) TryCast(1);
        if (Input.GetKeyDown(KeyCode.E)) TryCast(2);
        if (Input.GetKeyDown(KeyCode.R)) TryCast(3);

        foreach(Ability ab in abilities)
        {
            if(ab != null)
            {
                ab.player_script_ = this.player_script_;
            }
        }

        for(int i =0 ;i <4; ++i)
        {
            if(abilities[i] != null)
            {
                if(images[i].sprite != abilities[i].Icon)
                {
                    images[i].sprite = abilities[i].Icon;
                    GetComponent<SpriteRenderer>().color = Color.white;
                }
                // Debug.Log(ab.CooldownRemaining);
                // update UI
            }
        }

        
    }

    void TryCast(int index)
    {
        if (index < abilities.Count && abilities[index] != null)
        {
            int mana_cost = abilities[index].ManaCost;
            if(!player_script_.HasMana(mana_cost)) return;

            // TODO: also check cooldown   
            if (abilities[index].Cast())
                player_script_.DrainMana(mana_cost);
        }
    }
}
