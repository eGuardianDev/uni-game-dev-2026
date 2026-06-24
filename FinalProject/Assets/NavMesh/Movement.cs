using UnityEngine;
using UnityEngine.AI;
public class MovementTest : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private Animator animator;
    [SerializeField] private float stopDistance = 1.5f; // tweak in Inspector
    [SerializeField] private PlayerScript player_script_;
    [Header("Behavior")]
    [SerializeField] private GameBehavior gm_Behavior_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = this.GetComponent<Animator>(); 
        gm_Behavior_ = GameObject.Find("GameManager").GetComponent<GameBehavior>();
        player_script_ = this.transform.GetComponent<PlayerScript>();
        agent = this.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gm_Behavior_.Is_Paused)
        {
            agent.SetDestination(this.transform.position);
            animator.SetBool("Walking", false);
            return;
        }

        float distanceToTarget = Vector3.Distance(this.transform.position, target.position);
        bool isCloseToTarget = distanceToTarget <= stopDistance;

        if (player_script_.InRangeToAttackEnemy)
        {
            agent.ResetPath(); 
            agent.velocity = Vector3.zero;
            target.transform.position = this.transform.position;
            animator.SetBool("Walking", false);
        }
        else
        {
            agent.SetDestination(target.position);
            if (isCloseToTarget) animator.SetBool("Walking", false);   
            else animator.SetBool("Walking", true); 
        }
    }
}


