using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
[DefaultExecutionOrder(-100)]
public class RoomSpawn : MonoBehaviour
{
    RoomGenerator roomGenerator;
    public int id =0;
    public int Spawn_counter = 0;
  
    void Start()
    {
        roomGenerator = GameObject.Find("GameManager").GetComponent<RoomGenerator>();

        if(id ==0){
            SpawnLoop();
        }
        // Start spawning process
        // StartCoroutine(SpawnLoop());
    }
    public bool finished_calc = false;

    public void SpawnLoop()
    {
        // yield return new WaitForSeconds(0.01f);

        roomGenerator = GameObject.Find("GameManager").GetComponent<RoomGenerator>();

        RoomSpawner[] spawners = GetComponentsInChildren<RoomSpawner>();

        
        for (int i = spawners.Length - 1; i >= 0; i--)
        {
            int rand = roomGenerator.RandomRange(0, i + 1);
            (spawners[i], spawners[rand]) = (spawners[rand], spawners[i]);
        }


        List<RoomSpawner> visited_spawners = new List<RoomSpawner>();
        while (true)
        {

            if(visited_spawners.Count == 4) break;

            int index = roomGenerator.RandomRange(0,spawners.Length);
            RoomSpawner r_spawner = spawners[index];

            if (!visited_spawners.Contains(r_spawner))
            {
                visited_spawners.Add(r_spawner);

                if (!r_spawner.can_spawn || r_spawner.is_center || r_spawner.spawned || r_spawner.room_on_top)
                    continue;
                roomGenerator.possibleLocations.Push(r_spawner);
            }

            // if (roomGenerator.dept_counted >= roomGenerator.dept_counter.Count)
            //     // yield
            //     break;


            // if (roomGenerator.dept_counted >= roomGenerator.dept_counter.Count)
            //     // yield 
            //     break;

            // if (roomGenerator.RandomRange(0f, 1f) > roomGenerator.spawn_change)
            // {
            //     visited.Remove(r_spawner);
            //     continue;
            // }


            //roomGenerator.dept_counter[roomGenerator.dept_counted];

        }
        finished_calc = true;

        // yield return new WaitForSeconds(0.05f); // visual delay only

        // GameObject room = Instantiate(
        //     roomGenerator.rooms[j],
        //     r_spawner.transform.position,
        //     Quaternion.identity
        // );
        // RoomSpawn sp = room.GetComponent<RoomSpawn>();
        // sp.id = r_spawner.id;
        // sp.Spawn_counter = roomGenerator.dept_counted;
    }
}