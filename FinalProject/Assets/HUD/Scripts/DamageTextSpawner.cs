using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
   
    public static DamageTextSpawner Instance;

    [Header("Prefab")]
    public GameObject damageTextPrefab;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color critColor = Color.yellow;
    public Color healColor = Color.green;
    public Color manaColor = Color.blue;


    void Awake()
    {
        Instance = this;
    }

    public void Spawn(int amount, Vector3 worldPos, bool isMana = false, bool isHeal = false)
    {
        Vector3 offset = new Vector3(Random.Range(-0.1f, 0.1f), 0.3f, 0f);
        Vector3 spawnPos = new Vector3(
            worldPos.x + offset.x,
            worldPos.y + offset.y,
            -1f  // Keep Z at 0, same as your sprites
        );

        GameObject obj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);

        MeshRenderer mr = obj.GetComponentInChildren<MeshRenderer>();
        if (mr != null)
        {
            mr.sortingLayerName = "UI"; // Must match your top sorting layer name
            mr.sortingOrder = 10;
        }

        Color color = isHeal ? healColor : (isMana ? manaColor : normalColor);
        DamageTextFloat dt = obj.GetComponentInChildren<DamageTextFloat>();
        dt.Init(amount, color);
    }
}
