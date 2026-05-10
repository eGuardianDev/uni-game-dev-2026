using UnityEngine;

public class Bomb : MonoBehaviour
{

    [SerializeField] public float detonation_time;
    [SerializeField] private float detonation_time_left_;
    [SerializeField] protected ParticleSystem explode_particles_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        detonation_time_left_ = detonation_time;
    }

    // Update is called once per frame
    void Update()
    {
        detonation_time -= Time.deltaTime;

        if(detonation_time <= 0)
        {
            explode_particles_.transform.SetParent(null); 
            explode_particles_.Emit(25);
            Destroy(explode_particles_.gameObject, 2f); 
            Destroy(gameObject);
        }
    }
}
