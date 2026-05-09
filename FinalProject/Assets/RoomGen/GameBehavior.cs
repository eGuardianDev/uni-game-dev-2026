using UnityEngine;

public class GameBehavior : MonoBehaviour
{

    [SerializeField] private UI_Manager HUD;

    public bool Is_Paused
    {
        get
        {
            return HUD.PauseMenu.activeSelf;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HUD = this.GetComponent<UI_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
