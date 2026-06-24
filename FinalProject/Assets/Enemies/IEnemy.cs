using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int health_ = 100;
    [SerializeField] protected int damage_ = 10;
    [SerializeField] protected bool touch_damage_ = false;
    [SerializeField] protected int touch_damage_amount_ = 10;



    [Header("SFX")]
    [SerializeField] [Range(0f, 1f)] public float sfxVolume = 1f;
    [SerializeField] protected List<AudioClip> getDamagedSound;
    [SerializeField] private List<AudioClip> getDefeatedSound;
        public int Damage
    {
        get
        {
            return damage_;
        }
    }
    [SerializeField] protected float attack_speed_ = 1;
    [SerializeField] protected int movement_speed_ = 5;
    [SerializeField] protected int armour_ = 0;
    
    [SerializeField] protected ParticleSystem hit_particles_;
    
    [Header("Detection")]
    [SerializeField] protected float attack_radius_ = 2f;
    [SerializeField] public float detection_radius_ = 10f;

    [Header("Player")]
    [SerializeField] protected Transform player_;
    [SerializeField] protected PlayerScript player_script_;

    [Header("Drops")]
    [SerializeField] protected int mana_drop_ = 0;
    [SerializeField] protected GameObject mana_object_;
    [SerializeField] protected int health_drop_ = 0;
    [SerializeField] protected GameObject health_object_;

    [SerializeField] protected int money_drop_ = 0;
    
    [Header("Behavior")]
    [SerializeField] protected GameBehavior gm_Behavior_;
    [SerializeField] protected RoomGenerator gm_Room_gen_;
    [SerializeField] private UI_Manager gm_ui_;


    [SerializeField] protected bool Player_in_room = false; 
    protected virtual void Start()
    {
        Player_in_room = false;
        gm_Behavior_ = GameObject.Find("GameManager").GetComponent<GameBehavior>();
        gm_Room_gen_ = GameObject.Find("GameManager").GetComponent<RoomGenerator>();
        gm_ui_ = GameObject.Find("GameManager").GetComponent<UI_Manager>();

        player_ = GameObject.FindGameObjectWithTag("Player")?.transform;
        player_script_ = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerScript>();

        AfterStart();
    }
    protected virtual void AfterStart()
    {
        
    }


    protected virtual void Update()
    {
        sfxVolume = gm_ui_.volume_level;

        if(gm_Behavior_.Is_Paused) return;

        if(!Player_in_room) return;
        attackTimer_ -= Time.deltaTime;
        damageTimer_ -= Time.deltaTime;

        if (player_ == null) return;

        if (Detectplayer_IsClose())
        {
            if (IsInAttackRange())
            {
                Debug.Log("Attacking player");
                AttackPlayer();
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
        


    }
    // functions
    public virtual void GetDamage(int amount)
    {
        if(!Player_in_room) return;

        if (getDamagedSound.Count > 0)
        {
            AudioClip clip = getDamagedSound[Random.Range(0, getDamagedSound.Count)];
            AudioSource.PlayClipAtPoint(clip, transform.position,sfxVolume);
        }
        int finalDamage = Mathf.Max(amount - armour_, 0);
        health_ -= finalDamage;
        StartCoroutine(FlashRed());
        
        
        DamageTextSpawner.Instance.Spawn(amount, this.transform.position, false, false);
        
        if (health_ <= 0)
        {
            Die();
        }
        
    }
    protected IEnumerator FlashRed()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    protected float attackTimer_ = 0f;
    protected bool IsAttackOnCooldown => attackTimer_ > 0f;
    protected virtual void AttackPlayer()
    {
        if(!Player_in_room) return;
        attackTimer_ -= Time.deltaTime;
        if (attackTimer_ <= 0f)
        {
            Debug.Log("Enemy attacks player");
            player_script_.GetDamage(damage_);
            attackTimer_ = 1f / attack_speed_;
        }
    }

    public virtual void Engage()
    {
        StartCoroutine(EngageAfterDelay());
    }

    protected IEnumerator EngageAfterDelay()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
        yield return new WaitForSeconds(2f);
        GetComponent<SpriteRenderer>().color = Color.white;
        Player_in_room = true;
    }
    protected virtual bool Detectplayer_IsClose()
    {
        return Vector3.Distance(transform.position, player_.position) <= detection_radius_;
    }

    protected virtual bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, player_.position) <= attack_radius_;
    }

    protected virtual void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            player_.position,
            movement_speed_ * Time.deltaTime
        );
    }

    protected virtual void SpawnMana(){
        GameObject mana = Instantiate(mana_object_, transform.position, Quaternion.identity);
        mana.GetComponent<Mana>().value = mana_drop_;
    }
    protected virtual void SpawnHealth(){
        GameObject mana = Instantiate(health_object_, transform.position, Quaternion.identity);
        mana.GetComponent<Health>().value = health_drop_;
    }
    protected virtual void Die()
    {
        if (getDefeatedSound.Count > 0)
        {
            AudioClip clip = getDefeatedSound[Random.Range(0, getDefeatedSound.Count)];
            AudioSource.PlayClipAtPoint(clip, transform.position,sfxVolume);
        }
        onDeath();
        hit_particles_.transform.SetParent(null); 
        hit_particles_.Emit(25);
        if(mana_drop_ > 0)
        {
            SpawnMana();
        }
        if(health_drop_ > 0)
        {
            SpawnHealth();
        }
        Destroy(hit_particles_.gameObject, 2f); 
        Destroy(gameObject);
        if (GameData.Instance)
        {
            GameData.Instance.killed_enemies++;
            GameData.Instance.points+=5;
        }
    }

    protected virtual void onDeath()
    {
    }

    protected float damageTimer_ = 0f;
    private void OnTriggerStay2D(Collider2D other)
    {
        if(!Player_in_room) return;
        if (touch_damage_)
        {       
            if (other.CompareTag("Player"))
            {
                if (damageTimer_ <= 0f)
                {
                    other.GetComponent<PlayerScript>().GetDamage(touch_damage_amount_   );
                    damageTimer_ = 1f / attack_speed_;
                }
            }
        }
    }
    // state machine
    enum EnemyState { Idle, Chase, Attack }
}