using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpiderAIBoss : Enemy
{

    [SerializeField] protected float wait_before_attacK_timer_ = 3f;
    private Vector2 _dashTarget;
    [SerializeField] private bool isDashing_ = false;

    [SerializeField] private Animator animator;

    [Header("Boss Phases")]
    [SerializeField] private int phase_ = 1;
    [SerializeField] private int phase2_health_threshold_ = 66; // % of max HP
    [SerializeField] private int phase3_health_threshold_ = 33;

    [Header("Phase 2 Stats")]
    [SerializeField] private float phase2_movement_speed_ = 8f;
    [SerializeField] private float phase2_attack_speed_ = 1.5f;

    [Header("Phase 2 Spawning")]
    [SerializeField] private GameObject minion_prefab_;
    [SerializeField] private int minions_to_spawn_ = 2;
    [SerializeField] private float spawn_radius_ = 2f;
    [Header("Phase 3 Spawning")]
    [SerializeField] private int minions_to_spawn_2_ = 4;


    [Header("Phase 3 Stats")]
    [SerializeField] private float phase3_movement_speed_ = 12f;
    [SerializeField] private float phase3_attack_speed_ = 2.5f;

    

    [Header("Phase 3 Stats")]
    [SerializeField] private Slider health_slider_;
    [SerializeField] private GameObject Health_bar_;

    private int maxHealth_;
    private bool transitioningPhase_ = false;
    override protected void AfterStart()
    {
        maxHealth_ = health_;
        animator = GetComponent<Animator>();
        Health_bar_ = GameObject.Find("Bossbar");
        health_slider_ = Health_bar_.transform.GetChild(0).GetComponent<Slider>();
        Health_bar_.SetActive(false);
    }


    override public void Engage()
    {
        StartCoroutine(EngageAfterDelay());
        Health_bar_.SetActive(true);

    }
    float wait_before_attack_ = 0f;
    override protected void Update()
    {
        if(gm_Behavior_.Is_Paused) return;
        
        if(!Player_in_room) return;

        attackTimer_ -= Time.deltaTime;
        damageTimer_ -= Time.deltaTime;

        health_slider_.value = health_;
        health_slider_.maxValue = maxHealth_;
        CheckPhaseTransition();

        if (!isDashing_)
        {
            wait_before_attack_ -= Time.deltaTime;
        }
        if(wait_before_attack_ > 0) return;

        if (player_ == null) return;

        MoveTowardsPlayer();
    }

    override public void GetDamage(int amount)
    {
        if(transitioningPhase_) return;
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
    private void CheckPhaseTransition()
    {
        float healthPercent = (float) health_ / maxHealth_ * 100f;

        if (phase_ == 1 && healthPercent <= phase2_health_threshold_)
            StartCoroutine(TransitionToPhase(2));
        else if (phase_ == 2 && healthPercent <= phase3_health_threshold_)
            StartCoroutine(TransitionToPhase(3));
    }
    private IEnumerator TransitionToPhase(int newPhase)
    {
        phase_ = newPhase;
        transitioningPhase_ = true;
        isDashing_ = false;

        for (int i = 0; i < 5; i++)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
            yield return new WaitForSeconds(0.15f);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.15f);
        }

        if (newPhase == 2)
        {
            movement_speed_ = (int)phase2_movement_speed_;
            attack_speed_ = phase2_attack_speed_;
            SpawnMinions(minions_to_spawn_);
        }
        else if (newPhase == 3)
        {
            movement_speed_ = (int)phase3_movement_speed_;
            attack_speed_ = phase3_attack_speed_;

            GetComponent<SpriteRenderer>().color = new Color(1f, 0.4f, 0.4f);
            SpawnMinions(minions_to_spawn_2_);
            
        }

        transitioningPhase_ = false;
    }

    private void SpawnMinions(int k)
    {
        for (int i = 0; i < k; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle.normalized * spawn_radius_;
            Vector3 spawnPos = transform.position + (Vector3)randomOffset;

            GameObject minion = Instantiate(minion_prefab_, spawnPos, Quaternion.identity);

            Enemy minionEnemy = minion.GetComponent<Enemy>();
            if (minionEnemy != null)
                minionEnemy.Engage();
        }
    }

    protected override void onDeath()
    {
        Health_bar_.SetActive(false);
        gm_Room_gen_.Spawn_end_portal();
    }
    
    override protected void AttackPlayer()
    {
        attackTimer_ -= Time.deltaTime;
        if (attackTimer_ <= 0f)
        {
            isDashing_ = false;
            player_script_.GetDamage(damage_);
            attackTimer_ = 1f / attack_speed_;
        }
    }

    override protected void MoveTowardsPlayer()
    {
        if (!isDashing_)
        {
            Vector2 dir = ((Vector2)player_.position - (Vector2)transform.position).normalized;
            float overshoot = Random.Range(1, 5);
            _dashTarget = (Vector2)player_.position + dir * overshoot;
            isDashing_ = true;
            if(animator != null)
            {    
                animator.SetBool("Walking",isDashing_);
            }
        }


        transform.position = Vector2.MoveTowards(
            transform.position,
            _dashTarget,
            movement_speed_ * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, _dashTarget) < 0.1f)
        { 
            isDashing_ = false;
            wait_before_attack_ = wait_before_attacK_timer_;
            if(animator != null)
            {    
                animator.SetBool("Walking",isDashing_);
            }
        }

    }

}
