using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
 
    public GameObject platform;

    public int count_of_platform = 0;
    public GameObject lastPlatform;

    public int max_level_size = 50;
    public int min_height =10;    
    public int max_length = 5, max_height =10;    

    public GameObject key;
    public GameObject powerUp;

    public GameObject Enemy;
    public GameObject immune;
    public GameObject powerUp_Jump;

    int moving_chance = 40;
    int falling_chance = 40;
    int enemy_spawn_rate = 10;
    int jumper_spawn_rate = 10;
    int enemy_immunity = 25;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame  
    void Update()
    {
        if(count_of_platform != max_level_size)
        {
            
            int height = Random.Range(min_height, max_height);
            int length = Random.Range(-max_length, max_length);
            lastPlatform = Instantiate(platform,
            lastPlatform.transform.position + new Vector3(length,height,0),
            lastPlatform.transform.rotation);

            Platform _platform = lastPlatform.transform.GetChild(0).GetComponent<Platform>();
            Transform Spawner_pickups = _platform.Spawner_pickups;

            _platform.moving = Random.Range(0, 100) <= moving_chance;
            _platform.falling = Random.Range(0, 100) <= falling_chance;

            if(_platform.moving == false && _platform.falling == false)
            {
                int chance =  Random.Range(0, 100) ;//<= enemy_spawn_rate;
                if(chance <= enemy_spawn_rate)
                {
                    lastPlatform = Instantiate(Enemy,
                    _platform.Spawner_pickups.transform.position,
                    _platform.Spawner_pickups.transform.rotation);
                }
                chance =  Random.Range(0, 100) ;//<= enemy_spawn_rate;
                if(chance <= enemy_immunity)
                {
                    lastPlatform = Instantiate(immune,
                    _platform.Spawner_pickups.transform.position,
                    _platform.Spawner_pickups.transform.rotation);
                }
                chance =  Random.Range(0, 100) ;//<= enemy_spawn_rate;
                if(chance <= jumper_spawn_rate)
                {
                    lastPlatform = Instantiate(powerUp_Jump,
                    _platform.Spawner_jumping_powerup.transform.position,
                    _platform.Spawner_jumping_powerup.transform.rotation);
                }
            }

            count_of_platform++;

            if(count_of_platform % (max_level_size / 3) == 0)
            {
                Debug.Log("Spawning");
                lastPlatform = Instantiate(key,
                Spawner_pickups.transform.position,
                Spawner_pickups.transform.rotation);
            }



        }
    }
}
