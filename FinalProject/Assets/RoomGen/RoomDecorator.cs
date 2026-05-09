using UnityEngine;
using System.Collections;   
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class RoomDecorator : MonoBehaviour
{

    public System.Action onDoorClosed;
    public System.Action onFinalLayout;

    RoomGenerator roomGenerator;

    public Tilemap tilemap;
    public TileBase doorTile; 

    public RoomSpawner[] checkers;

    public GameObject Door;

    public List<GameObject> enemies = new List<GameObject>();
    private Vector3[] doors =
    {
        new Vector3(-12,0,0),
        new Vector3(-12, -1, 0),
        new Vector3(11, 0, 0),
        new Vector3(11, -1, 0),
        new Vector3(-1, -11, 0),
        new Vector3(0, -11, 0),
        new Vector3(-1, 10, 0),
        new Vector3(0, 10, 0)
    };
    public void SpawnAt(Vector3 cellPos, TileBase tile)
    {
        Vector3Int cellPos1 = Vector3Int.RoundToInt(cellPos);
        tilemap.SetTile(cellPos1, tile);
    }

    [SerializeField] List<int> doorWays = new List<int>();
    bool playerInside_ = false;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f); 

        for(int i = 0; i < checkers.Length; ++i)
        {
            SpawnAt(doors[i*2], doorTile);
            SpawnAt(doors[i*2+1], doorTile);
        }
        onDoorClosed?.Invoke();

        yield return new WaitForSeconds(1f); 
        for(int i = 0; i < checkers.Length; ++i)
        {
            SpawnAt(doors[i*2], null);
            SpawnAt(doors[i*2+1], null);
        }


        for(int i = 0; i < checkers.Length; ++i)
        {
            if (!checkers[i].room_on_top)
            {
                SpawnAt(doors[i*2], doorTile);
                SpawnAt(doors[i*2+1], doorTile);
            }
            else
            {
                doorWays.Add(i);
            }
        }

        onFinalLayout?.Invoke();


        roomGenerator = GameObject.Find("GameManager").GetComponent<RoomGenerator>();



        if(roomGenerator.spawn_patterns.Length > 0)
        {
            int decoration_index = roomGenerator.RandomRange(0,roomGenerator.spawn_patterns.Length);
            GameObject room = Instantiate(
                    roomGenerator.spawn_patterns[decoration_index],
                    this.transform.position,
                    Quaternion.identity,
                    this.transform
            );

            if(this.transform.parent.GetComponent<RoomSpawn>().Spawn_counter == roomGenerator.dept_counter.Count-1){
                //boss
                int enemy_index = roomGenerator.RandomRange(0,roomGenerator.Bosses.Length);
                GameObject enemy = Instantiate(
                    roomGenerator.Bosses[enemy_index],
                    this.transform.position,
                    Quaternion.identity,
                    this.transform
                );
                enemies.Add(enemy);
                GameObject end_portal = Instantiate(
                    roomGenerator.nextRoom_portal,
                    this.transform.position,
                    Quaternion.identity,
                    this.transform
                );
                roomGenerator.nextRoom_portal_instance = end_portal;
                end_portal.SetActive(false);

            }else if(this.transform.parent.GetComponent<RoomSpawn>().Spawn_counter == (roomGenerator.dept_counter.Count-1)/2){
                //item room

                GameObject itemPicker = Instantiate(
                    roomGenerator.ItemPicker,
                    this.transform.position,
                    Quaternion.identity,
                    this.transform
                );
                
                int ability = roomGenerator.RandomRange(0,roomGenerator.Abilities.Count);

                itemPicker.GetComponent<AbilityHolder>().ab = roomGenerator.Abilities[ability];

                GameObject.Find("GameManager").GetComponent<UI_Manager>().AbilityPickupsStatue = itemPicker;

            }else 
            if(this.transform.parent.GetComponent<RoomSpawn>().id == 0){
                // start room
            }else{
                // normal room
                foreach (Transform child in room.transform)
                {
                    int enemy_index = roomGenerator.RandomRange(0,roomGenerator.Enemies.Length);
                    GameObject enemy = Instantiate(
                        roomGenerator.Enemies[enemy_index],
                        child.position,
                        Quaternion.identity,
                        this.transform
                    );
                    enemies.Add(enemy);
                }
            }



        }
    }

    bool doorSpawned = false;
    List<GameObject> spawned_doors = new List<GameObject>();
    void spawn_doors()
    {
        foreach(int a in doorWays)
        {  
            GameObject room = Instantiate(
                    Door,
                    this.transform.position+ doors[a*2]+ new Vector3(0.5f,0.5f,0),
                    Quaternion.identity,
                    this.transform
            );
            spawned_doors.Add(room);
            room = Instantiate(
                    Door,
                    this.transform.position+ doors[a*2+1] +new Vector3(0.5f,0.5f,0),
                    Quaternion.identity,
                    this.transform
            );
            spawned_doors.Add(room);
        }
        GameObject player = GameObject.Find("Player");
        Vector3 dir = player.transform.position - transform.position;

        bool isLeft  = dir.x < 0;
        bool isRight = dir.x > 0;
        bool isAbove = dir.y > 0;
        bool isBelow = dir.y < 0;

        float offset = 5f;
        if (isLeft)
        {
            player.transform.position += Vector3.right * offset;
        }
        else if (isRight)
        {
            player.transform.position += Vector3.left * offset;
        }
        else if (isAbove)
        {
            player.transform.position += Vector3.down * offset;
        }
        else if (isBelow)
        {
            player.transform.position += Vector3.up * offset;
        }
    } 
    void remove_doors()
    {
        foreach(GameObject door in spawned_doors)
        {
            Destroy(door);
        }
    }
    void Update()
    {
        if (playerInside_)
        {
            bool flag = false;
            foreach(GameObject enemy in enemies)
            {
                if(enemy != null)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                if (!doorSpawned)
                {
                    doorSpawned = true;
                    spawn_doors();   
                }
            }
            else
            {
                if (doorSpawned)
                {
                    if(spawned_doors.Count > 0)
                    {
                        remove_doors();
                        roomGenerator.Killed_All_in_a_room();
                        spawned_doors.Clear();
                    }
                }
            }
        }
    }

    public void PlayerEnter()
    {

        playerInside_ = true;
        foreach (GameObject enemy in enemies)
        {
            if(enemy == null) continue; 
            Enemy e = enemy.GetComponent<Enemy>();
            if(e) e.Engage();
            roomGenerator.EnterRoom();
        }
    }

}
