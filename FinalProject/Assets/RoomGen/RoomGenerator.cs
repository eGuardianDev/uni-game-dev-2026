using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using NavMeshPlus.Components;

// using UnityEngine.AI;
// using Unity.AI.Navigation;
public class RoomGenerator : MonoBehaviour
{
    public GameObject[] rooms;
    public float spawn_change = 1f;
    public int id = 1; // pickable id

    public List<int> dept_counter = new List<int>(){0,0,0,0,2}; // 16
    public int seed = 1234;

    public int level = 1;

    public Stack<RoomSpawner> possibleLocations = new Stack<RoomSpawner>();

    public Animator anim;

    [SerializeField] private NavMeshPlus.Components.NavMeshSurface navMeshSurface_;
    [SerializeField] private NavMeshPlus.Components.NavMeshSurface navMeshSurface_Battle_;
    // [SerializeField] private NavMeshSurface navMeshSurface_;

    [SerializeField] public List<GameObject> Abilities;
    [SerializeField] public GameObject ItemPicker;

    [Header("SFX")]
    [SerializeField] [Range(0f, 1f)] public float sfxVolume = 1f;
    [SerializeField] private AudioClip EnteringRoomAudio;
    [SerializeField] private UI_Manager gm_ui_;



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
    public GameObject[] spawn_patterns;
    public GameObject[] Enemies;
    public GameObject[] Bosses;
    void Awake()
    {
        if(GameData.Instance != null)
        {
            seed = GameData.Instance.seed;
        }
        rng = new System.Random(seed);
        if (anim)
        {
            anim.speed = 0f; 
        }
    }

    public GameObject nextRoom_portal;
    public GameObject nextRoom_portal_instance;
    private GameData gd;
    private int pendingRooms = 0;
    private int pendingRoomsBattle = 0; 
    void Start()
    {
        // int count = 0;
        // Debug.Log("Count: " + dept_counter.Count);
        gm_ui_ = GameObject.Find("GameManager").GetComponent<UI_Manager>();

        if(GameData.Instance != null)
        {
            while(dept_counter.Count < GameData.Instance.room_size)
            {
                dept_counter.Add(dept_counter[dept_counter.Count-1]+1);   
            }
        }

        pendingRooms = dept_counter.Count;
        pendingRoomsBattle = dept_counter.Count;
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


            r_spawner.id = id;
            id++;

            RoomSpawn sp = room.GetComponent<RoomSpawn>();
            sp.id = r_spawner.id;
            sp.Spawn_counter = dept_counted;
            dept_counted++;


            RoomDecorator rd = room.GetComponentInChildren<RoomDecorator>();
            sp.SpawnLoop();

            if(dept_counted == dept_counter.Count-1)
            {
                
            }
            rd.onDoorClosed += OnRoomReady; 
            rd.onFinalLayout += OnFinalLayout; 
        }

    }

    void OnRoomReady()
    {
        pendingRoomsBattle--;
        if (pendingRoomsBattle <= 0)
        {
            StartCoroutine(Bake_Battle_Async());
        }
    }
    void OnFinalLayout()
    {
        pendingRooms--;
        if (pendingRooms <= 0)
        {
            StartCoroutine(BakeAsync());
        }
       
    }

    IEnumerator BakeAsync()
    {
        var op = navMeshSurface_.BuildNavMeshAsync();
        yield return op;
        Debug.Log("NavMesh baked!");
        if (anim)
        {
            anim.speed = 1f;
        }
    }    
    IEnumerator Bake_Battle_Async()
    {
        var op = navMeshSurface_Battle_.BuildNavMeshAsync();
        yield return op;
        Debug.Log("NavMesh Battle baked!");
        Killed_All_in_a_room();
    }

    public void EnterRoom()
    {
        if (EnteringRoomAudio)
        {
            AudioSource.PlayClipAtPoint(EnteringRoomAudio, transform.position,sfxVolume);
        }
        navMeshSurface_Battle_.gameObject.SetActive(true);
        navMeshSurface_.gameObject.SetActive(false);
    }
    public void Killed_All_in_a_room()
    {
        navMeshSurface_Battle_.gameObject.SetActive(false);
        navMeshSurface_.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        sfxVolume = gm_ui_.volume_level;
    }

    public void Spawn_end_portal()
    {
        nextRoom_portal_instance.SetActive(true);
    }
}
