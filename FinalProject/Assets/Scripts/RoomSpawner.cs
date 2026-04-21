using UnityEngine;

public class RoomSpawner : MonoBehaviour{
    public bool can_spawn = true;
    public bool spawned = false;
    public bool is_center = false;

    public bool room_on_top = false;
    public int id = 0;
    RoomGenerator roomGenerator;

    void Start()
    {
        roomGenerator = GameObject.Find("GameManager").GetComponent<RoomGenerator>();
        this.id = roomGenerator.id++;
    }
    void Update()
    {
        if(is_center == true)
        {
            can_spawn = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "room_spawner")
        {
            RoomSpawner rg = other.GetComponent<RoomSpawner>();
            if (rg.spawned)
            {
                this.can_spawn = false;
            }
            if(rg.id > this.id || rg.spawned)
            {
                this.can_spawn = false;
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "room_spawner")
        {
            RoomSpawner rg = other.GetComponent<RoomSpawner>();
            if (rg.spawned)
            {
                this.can_spawn = false;
                rg.room_on_top = true;
            }
            if(rg.id > this.id || rg.spawned)
            {
                this.can_spawn = false;
            }
            if (rg.room_on_top)
            {
                room_on_top = true;
            }
        }
    }
    public bool CanPlaceRoom()
    {
        float radius = 0.5f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            radius
        );


        foreach (var hit in hits)
        {

            if (hit.gameObject == gameObject)
                continue;

            if (hit.GetComponent<RoomSpawner>() != null)
            {
                RoomSpawner sp = hit.GetComponent<RoomSpawner>();
                if(sp.room_on_top || sp.is_center || sp.spawned || !sp.can_spawn){
                    return false;
                }
            }
        }

        return true;
    }
}
