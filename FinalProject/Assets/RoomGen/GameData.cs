using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    public int killed_enemies;
    public int points;
    public int level;

    bool firstTime = true;

    // data between levels
    public int p_health;
    public int p_mana;

    [SerializeField] private List<GameObject> abilities = new List<GameObject> { null, null, null, null };

    public void LoadNextScene()
    {

        int next = SceneManager.GetActiveScene().buildIndex;
        
        room_size +=1;
        seed+=1;

        SceneManager.LoadScene(next);

    }

    public void LoadNextSceneTransition()
    {
        StartCoroutine(Transition());
    }

    public Animator transition;
    public float transitioning_time = 1f;
    IEnumerator Transition()
    {
        GameObject player;
        if ((player = GameObject.Find("Player")) != null)
        {
            PlayerScript ps = player.GetComponent<PlayerScript>();
            save_abilities();
            this.p_health = ps.Health;
            this.p_mana = ps.Mana;
        }
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitioning_time);
        LoadNextScene();
    }

    public static GameData Instance;
    public int seed = 69;
    [SerializeField] private TMP_InputField seedInputField;

    public int room_size = 10;
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        transition = GameObject.Find("FadeImage").GetComponent<Animator>();
        seed = new System.Random().Next(0, 1000000);
        seedInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        
        // set default
        seedInputField.text = seed.ToString();
        
        // listen for changes
        seedInputField.onEndEdit.AddListener(OnSeedChanged);
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transition = GameObject.Find("FadeImage").GetComponent<Animator>();

        GameObject player = GameObject.Find("Player");
        
        if (player != null){
            PlayerScript ps = player.GetComponent<PlayerScript>();
            if (ps != null)
            {
                level+=1;
                ps.SetHealth(p_health);
                ps.SetMana(p_mana);
                load_abilities();
            }
        }
        else
        {
            level = 0;
        }
    }

    void save_abilities()
    {
        GameObject player_ = GameObject.Find("Player");
        AbilityManager am = player_.GetComponent<AbilityManager>();

        for (int i = 0; i < 4; i++)
        {
            if (am.abilities[i] != null)
            {
                abilities[i] = am.abilities[i].gameObject;
                am.abilities[i].transform.SetParent(this.transform);
            }
        }
    }

    void load_abilities()
    {
        GameObject player_ = GameObject.Find("Player");
        AbilityManager am = player_.GetComponent<AbilityManager>();

        int count = Mathf.Min(am.abilities.Count, this.abilities.Count);
        for (int i = 0; i < count; i++)
        {
            if (this.abilities[i] != null)
            {
                abilities[i].transform.SetParent(player_.transform); 
                am.abilities[i] = abilities[i].GetComponent<Ability>();
            }
        }
    }
    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F12))
        {
            LoadNextSceneTransition();
        }
        #endif
    }

    private void OnSeedChanged(string value)
    {
        level = 0;
        if (int.TryParse(value, out int parsed))
        {
            seed = parsed;
            GameData.Instance.seed = parsed;
        }
        else
        {
            // reset to last valid value if somehow invalid
            seedInputField.text = seed.ToString();
        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject); // prevent duplicates on reload
    }
}
