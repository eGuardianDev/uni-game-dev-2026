using UnityEngine;
using System.Collections;
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int health_ = 100;
    [SerializeField] private int damage_ = 10;
    [SerializeField] private int attack_speed_ = 1;
    [SerializeField] private int movement_speed_ = 5;
    [SerializeField] private int armour_ = 0;
    
    [SerializeField] private ParticleSystem hit_particles_;
    
    [Header("Detection")]
    [SerializeField] private float attack_radius_ = 2f;
    [SerializeField] public float detection_radius_ = 10f;

    [Header("Player")]
    [SerializeField] private Transform player_;
    [SerializeField] private PlayerScript player_script_;


    protected virtual void Start()
    {
        player_ = GameObject.FindGameObjectWithTag("Player")?.transform;
        player_script_ = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerScript>();
    }

    protected virtual void Update()
    {
        if (player_ == null) return;

        if (Detectplayer_IsClose())
        {
            if (IsInAttackRange())
            {
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
        int finalDamage = Mathf.Max(amount - armour_, 0);
        health_ -= finalDamage;
        StartCoroutine(FlashRed());
        
        if (health_ <= 0)
        {
            Die();
        }
    }
    private IEnumerator FlashRed()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    private float attackTimer_ = 0f;

    protected virtual void AttackPlayer()
    {
        attackTimer_ -= Time.deltaTime;
        if (attackTimer_ <= 0f)
        {
            Debug.Log("Enemy attacks player");
            player_script_.GetDamage(damage_);
            attackTimer_ = 1f / attack_speed_;
        }
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

     protected virtual void Die()
    {
        hit_particles_.transform.SetParent(null); 
        hit_particles_.Emit(25);
        Destroy(hit_particles_.gameObject, 2f); 
        Destroy(gameObject);
    }
    // state machine
    enum EnemyState { Idle, Chase, Attack }
}