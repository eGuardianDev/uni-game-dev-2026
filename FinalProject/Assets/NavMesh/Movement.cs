using UnityEngine;
using UnityEngine.AI;
public class MovementTest : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    [SerializeField] private PlayerScript player_script_;
    [Header("Behavior")]
    [SerializeField] private GameBehavior gm_Behavior_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            return;
        }

        if (player_script_.InRangeToAttackEnemy)
        {
            agent.ResetPath(); 
            agent.velocity = Vector3.zero;
            target.transform.position = this.transform.position;
        }
        else
        {
            agent.SetDestination(target.position);
        }
    }
}
