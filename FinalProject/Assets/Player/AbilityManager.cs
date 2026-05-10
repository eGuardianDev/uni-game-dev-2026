using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class AbilityManager : MonoBehaviour
{
    public List<Ability> abilities = new List<Ability>(4);
    public List<Image> images = new List<Image>(4);
    public List<GameObject> CoolDownTimer = new List<GameObject>(4);
    List<TMP_Text> CoolDownTimer_Times = new List<TMP_Text>();
    [SerializeField] private PlayerScript player_script_;

    [SerializeField] private GameObject ability_range_;
    

    [Header("Behavior")]
    [SerializeField] private GameBehavior gm_Behavior_;


    void Start()
    {
        gm_Behavior_ = GameObject.Find("GameManager").GetComponent<GameBehavior>();

        ability_range_ = transform.Find("AbilityRange").gameObject;
        player_script_ = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
        foreach (GameObject obj in CoolDownTimer)
        {
            if (obj != null && obj.transform.childCount > 0)
            {
                TMP_Text text = obj.transform.GetChild(0).GetComponent<TMP_Text>();

                if (text != null)
                {
                    text.text = "";
                    CoolDownTimer_Times.Add(text);
                }
                else
                {
                    Debug.LogWarning($"No TMP_Text found in first child of {obj.name}");
                }
            }
        }
    }
 
    void Update()
    {

        if(gm_Behavior_.Is_Paused) return;

        if (Input.GetKeyDown(KeyCode.Q)) ShowDamageAndRange(0);
        if (Input.GetKeyDown(KeyCode.W)) ShowDamageAndRange(1);
        if (Input.GetKeyDown(KeyCode.E)) ShowDamageAndRange(2);
        if (Input.GetKeyDown(KeyCode.R)) ShowDamageAndRange(3);




        if (Input.GetKeyUp(KeyCode.Q)) TryCast(0);
        if (Input.GetKeyUp(KeyCode.W)) TryCast(1);
        if (Input.GetKeyUp(KeyCode.E)) TryCast(2);
        if (Input.GetKeyUp(KeyCode.R)) TryCast(3);

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
                
                CoolDownTimer[i].SetActive(abilities[i].IsOnCooldown);
                float cd = abilities[i].CooldownRemaining;
                if (cd > 1f)
                {
                    CoolDownTimer_Times[i].text = Mathf.CeilToInt(cd).ToString();
                }
                else
                {
                    CoolDownTimer_Times[i].text = cd.ToString("0.0");
                }
                // Debug.Log(ab.CooldownRemaining);
                // update UI
            }
        }

        
    }

    void ShowDamageAndRange(int index)
    {
        if(abilities[index] != null)
        {
            ability_range_.SetActive(true);

            CursorPositioning.Instance.Casting = true;

            SetSpriteRadius(abilities[index].Range);
        }
    }

    void TryCast(int index)
    {
        ability_range_.SetActive(false);
        CursorPositioning.Instance.Casting = false;

        if (index < abilities.Count && abilities[index] != null)
        {
            int mana_cost = abilities[index].ManaCost;
            if(!player_script_.HasMana(mana_cost)) return;

            if(abilities[index].IsOnCooldown) return;
            // TODO: also check cooldown   
            if (abilities[index].Cast())
                player_script_.DrainMana(mana_cost);
        }
    }



    public void SetSpriteRadius(float radius)
    {
        // Diameter = Radius * 2
        float diameter = radius * 2f;
        ability_range_.transform.localScale = new Vector3(diameter, diameter, 1f);
    }
}
