using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
      [Header("Stats")]
    [SerializeField] private int health_ = 100;
    [SerializeField] private int max_health_ = 100;
    [SerializeField] private int damage_ = 10;
    [SerializeField] private int attack_speed_ = 1;
    [SerializeField] private int movement_speed_ = 5;
    [SerializeField] private int armour_ = 0;
    [SerializeField] private int mana_ = 0;
    [SerializeField] private int max_mana_ = 100;
    


    [Header("Detection")]
    [SerializeField] public Enemy enemy_;
    [SerializeField] private float attack_radius_ = 2f;

    [Header("UI")]
    [SerializeField] private Slider health_slider_;
    [SerializeField] private Slider mana_slider_;

    void Start()
    {
        
        health_slider_.maxValue = max_health_;
        health_slider_.value = max_health_;
        health_ = max_health_;

        mana_slider_.maxValue = max_mana_;
        mana_slider_.value = max_mana_;
        mana_ = max_mana_;
    }

    void Update()
    {
        if (enemy_ != null && IsInAttackRange())
        {
            Attack();
        }
    }
    // functions
    public void GetDamage(int amount)
    {
        int finalDamage = Mathf.Max(amount - armour_, 0);
        health_ -= finalDamage;

        health_slider_.value = health_;
        if (health_ <= 0)
        {
            Die();
        }
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
        }
        
    }
    private float attackTimer_ = 0f;

    void Attack()
    {
        attackTimer_ -= Time.deltaTime;
        if (attackTimer_ <= 0f)
        {
            // Debug.Log("Player Attacks");
            enemy_.GetDamage(damage_);
            attackTimer_ = 1f / attack_speed_;
        }
    }
    bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, enemy_.transform.position) <= attack_radius_;
        return false;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
