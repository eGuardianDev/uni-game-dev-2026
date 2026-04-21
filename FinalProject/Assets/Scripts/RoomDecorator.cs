using UnityEngine;
using System.Collections;   
using UnityEngine.Tilemaps;
public class RoomGenerator1 : MonoBehaviour
{
    RoomGenerator roomGenerator;

    public Tilemap tilemap;
    public TileBase doorTile; 

    public RoomSpawner[] checkers;


    private Vector3Int[] doors =
    {
        new Vector3Int(-12,0,0),
        new Vector3Int(-12, -1, 0),
        new Vector3Int(11, 0, 0),
        new Vector3Int(11, -1, 0),
        new Vector3Int(-1, -11, 0),
        new Vector3Int(0, -11, 0),
        new Vector3Int(-1, 10, 0),
        new Vector3Int(0, 10, 0)
    };
    public void SpawnAt(Vector3Int cellPos)
    {
        tilemap.SetTile(cellPos, doorTile);
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f); 
        for(int i = 0; i < checkers.Length; ++i)
        {
            if (!checkers[i].room_on_top)
            {
                SpawnAt(doors[i*2]);
                SpawnAt(doors[i*2+1]);
            }
        }
        roomGenerator = GameObject.Find("GameManager").GetComponent<RoomGenerator>();



        if(roomGenerator.decorations.Length > 0)
        {
            int decoration_index = roomGenerator.RandomRange(0,roomGenerator.decorations.Length);
            GameObject room = Instantiate(
                    roomGenerator.decorations[decoration_index],
                    this.transform.position,
                    Quaternion.identity,
                    this.transform
            );
        }

    }

    void Update()
    {
    }
}
