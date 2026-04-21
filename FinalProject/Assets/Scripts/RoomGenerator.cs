using UnityEngine;
using System.Collections;

using System.Collections.Generic;
public class RoomGenerator : MonoBehaviour
{
    public GameObject[] rooms;

    public float spawn_change = 1f;
    public int id = 0; // pickable id

    public List<int> dept_counter = new List<int>(){0,0,0,0,2}; // 16
    public int seed = 1234;


    public Stack<RoomSpawner> possibleLocations = new Stack<RoomSpawner>();

    System.Random rng;

      public int RandomRange(int min, int max) // max exclusive
    {
        return rng.Next(min, max);
    }

    public float RandomRange(float min, float max)
    {
        return (float)(rng.NextDouble() * (max - min) + min);
    }

    public int dept_counted = 0;
    public GameObject[] decorations;
    void Awake()
    {
        rng = new System.Random(seed);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // IEnumerator
    void Start()
    {
        int count = 0;
        Debug.Log("Count: " + dept_counter.Count);

        while(dept_counted < dept_counter.Count)
        {
            // if(count == 250) break;
            // if (possibleLocations.Count > 0)
            // {       
                RoomSpawner r_spawner = possibleLocations.Pop();
                if(!r_spawner.CanPlaceRoom()){
                    continue;
                }

                int j = RandomRange(0, rooms.Length);
                GameObject room = Instantiate(
                    rooms[j],
                    r_spawner.transform.position,
                    Quaternion.identity
                );

                r_spawner.can_spawn = false;
                r_spawner.spawned = true;


                // Physics2D.Simulate(Time.fixedDeltaTime);

                RoomSpawn sp = room.GetComponent<RoomSpawn>();
                sp.id = r_spawner.id;
                sp.Spawn_counter = dept_counted;
                dept_counted++;

                // yield return new WaitForSeconds(1f);s
                sp.SpawnLoop();
            // while (!sp.finished_calc)
            // {
            //     //wait 
            // }
            //     count =0;
            // }
            // count++;

        }

    }

    // Update is called once per frame
    void Update()
    {
    }
}
