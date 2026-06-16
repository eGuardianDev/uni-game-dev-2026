using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
public class PlayerScript : MonoBehaviour
{
    
    [Header("Stats")]
    [SerializeField] public int health_ = 100;
    [SerializeField] public int max_health_ = 100;
    [SerializeField] public int final_max_health_ = 100;
    [SerializeField] public int damage_ = 10;
    [SerializeField] public int final_damage_ = 10;
    [SerializeField] public int attack_speed_ = 1;
    [SerializeField] public int final_attack_speed_ = 1;
    [SerializeField] public int movement_speed_ = 5;
    [SerializeField] public int final_movement_speed_ = 5;
    [SerializeField] public int armour_ = 0;
    [SerializeField] public int mana_ = 0;
    [SerializeField] public int max_mana_ = 100;
    [SerializeField] public int final_max_mana_ = 100;
    
    [Header("SFX")]
    [SerializeField] [Range(0f, 1f)] public float sfxVolume = 1f;
    [SerializeField] private List<AudioClip> getDamagedSound;
    [SerializeField] private List<AudioClip> getDefeatedSound;

    [Header("Game feel")]
    [SerializeField] private GameObject Low_Health;
    [SerializeField] private CameraShake Camera_Shake;
    

    public int Invicible = 0;
    public int Invicible_default = 0;
    public int Health {
        get{
            return this.health_;
        }
    }
    public int Mana {
        get{
            return this.mana_;
        }
    }


    [Header("Detection")]
    [SerializeField] public Enemy enemy_;
    [SerializeField] public bool engage_enemy_ = false;
    [SerializeField] private float attack_radius_ = 2f;

    public bool InRangeToAttackEnemy 
    {
        get 
        {
            if (enemy_ == null) return false;
            float distance = Vector3.Distance(enemy_.transform.position, transform.position);
            return distance < attack_radius_;
        }
    }
    
    [Header("UI")]
    [SerializeField] private Slider health_slider_;
    [SerializeField] private TMP_Text health_text_;
    [SerializeField] private Slider mana_slider_;
    [SerializeField] private TMP_Text mana_text_;

    [Header("Behavior")]
    [SerializeField] private GameBehavior gm_Behavior_;
    [SerializeField] private UI_Manager gm_ui_;


    UnityEngine.AI.NavMeshAgent agent;


    SpriteRenderer sprites;
    void Start()
    {
        sprites = GetComponent<SpriteRenderer>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        final_max_health_ = max_health_;
        gm_Behavior_ = GameObject.Find("GameManager").GetComponent<GameBehavior>();
        gm_ui_ = GameObject.Find("GameManager").GetComponent<UI_Manager>();

        health_slider_.maxValue = final_max_health_;
        mana_slider_.maxValue = max_mana_;

        if (GameData.Instance != null && GameData.Instance.p_health > 0)
        {
            health_ = GameData.Instance.p_health;
            mana_ = GameData.Instance.p_mana;
        }
        else
        {
            health_ = final_max_health_;
            mana_ = max_mana_;
        }

        health_slider_.value = health_;
        mana_slider_.value = mana_;
        health_text_.text = health_ + "/" + final_max_health_;
        mana_text_.text = mana_ + "/" + max_mana_;

        StartCoroutine(regenerateMana());
    }

    public void update_HUD()
    {
        health_slider_.maxValue = final_max_health_;
        mana_slider_.maxValue = max_mana_;

        health_text_.text = health_ + "/" + final_max_health_;
        mana_text_.text = mana_ + "/" + max_mana_;
        
        health_slider_.value = health_;
        mana_slider_.value = mana_;
    }


    IEnumerator regenerateMana()
    {
        yield return new WaitForSeconds(1f);
        mana_ += 2;
        mana_ = Mathf.Min(max_mana_, mana_);
        mana_slider_.value = mana_;
        mana_text_.text = mana_ + "/" + max_mana_;

        StartCoroutine(regenerateMana());
    }

    void Update()
    {
        if(Invicible > 0)
        {
            Color c = sprites.color;
            c.a = 0.75f;
            sprites.color = c;
        }
        else
        {
            Color c = sprites.color;
            c.a = 1f;
            sprites.color = c;
        }

        if(health_ < max_health_ * 0.2)
            Low_Health.SetActive(true);
        else
            Low_Health.SetActive(false);

        agent.speed = final_movement_speed_;
        if(gm_Behavior_.Is_Paused) return;

        if (enemy_ != null && IsInAttackRange() && engage_enemy_)
        {
            Attack();
        }
    }
    // functions
    public void GetDamage(int amount)
    {
        if(Invicible > 0) return;
        int finalDamage = Mathf.Max(amount - armour_, 0);
        health_ -= finalDamage;

        health_slider_.value = health_;
        health_text_.text = health_ +"/"+ final_max_health_;

        CameraShake.Instance.Shake(0.1f, 0.05f);
        DamageTextSpawner.Instance.Spawn(amount, this.transform.position);
        if (health_ <= 0)
        {
            Die();
        }
        else
        {
            if (getDamagedSound.Count > 0)
            {
                AudioClip clip = getDamagedSound[Random.Range(0, getDamagedSound.Count)];
                AudioSource.PlayClipAtPoint(clip, transform.position, sfxVolume);
            }
        }
    }
    public void GetHealth()
    {
        health_ = Mathf.Min(health_, final_max_health_);

        health_slider_.value = health_;
        health_text_.text = health_ +"/"+ final_max_health_;
    }

    public void GetHeal(int amount)
    {
        health_ = Mathf.Min(health_+amount, final_max_health_);

        health_slider_.value = health_;
        health_text_.text = health_ +"/"+ final_max_health_;
        DamageTextSpawner.Instance.Spawn(amount, this.transform.position, false, true);

        
    }

    public void SetHealth(int amount)
    {
        health_ = Mathf.Min(amount, max_health_);

        health_slider_.value = health_;
        health_text_.text = health_ +"/"+ final_max_health_;

        DamageTextSpawner.Instance.Spawn(amount, this.transform.position, false, true);
    }
    public void SetMana(int amount)
    {
        mana_ = amount;
        mana_ = Mathf.Min(max_mana_, mana_);
        mana_slider_.value = mana_;
        mana_text_.text = mana_ + "/" + max_mana_;

    }
        
    public void GetMana()
    {
        mana_slider_.value = mana_;
        mana_text_.text = mana_ + "/" + max_mana_;
    }

    public bool HasMana(int mana)
    {
        return mana_ >= mana;
    }
    public void DrainMana(int amount)
    {
        if (HasMana(amount))
        {
            mana_ -= amount;
            mana_slider_.value = mana_;
            mana_text_.text = mana_ + "/" + max_mana_;
        }
    }    
    public void FillMana(int amount)
    {
        mana_ += amount;
        mana_ = Mathf.Min(max_mana_, mana_);
        mana_slider_.value = mana_;
        mana_text_.text = mana_ + "/" + max_mana_;

        DamageTextSpawner.Instance.Spawn(amount, this.transform.position, true, false);
    }
    private float attackTimer_ = 0f;

    void Attack()
    {
        attackTimer_ -= Time.deltaTime;
        if (attackTimer_ <= 0f)
        {
            // Debug.Log("Player Attacks");
            enemy_.GetDamage(damage_);
            attackTimer_ = 1f / final_attack_speed_;
        }
    }
    bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, enemy_.transform.position) <= attack_radius_;
        return false;
    }

    void Die()
    {
        if (getDefeatedSound.Count > 0)
        {
            AudioClip clip = getDefeatedSound[Random.Range(0, getDefeatedSound.Count)];
            AudioSource.PlayClipAtPoint(clip, transform.position,sfxVolume);
        }
        gm_ui_.Display_Death_Screen();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Pickable")
        {
            Mana mana = other.GetComponent<Mana>();
            if (mana != null)
            {
                FillMana(mana.value);
            }
            
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                GetHeal(health.value);
            }
            Destroy(other.gameObject);
        }
    }
}
