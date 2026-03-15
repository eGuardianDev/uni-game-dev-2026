using UnityEngine;

public class PrefabDecoration : MonoBehaviour
{
    public Sprite[] Decorations;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int spriteIndex = Random.Range(0, Decorations.Length); 
        this.GetComponent<SpriteRenderer>().sprite = Decorations[spriteIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
