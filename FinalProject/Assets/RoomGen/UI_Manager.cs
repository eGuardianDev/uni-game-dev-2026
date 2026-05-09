using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections;
public class UI_Manager : MonoBehaviour
{
    [SerializeField] private Slider Opacity_HUD;
    [SerializeField] private Slider Size_HUD;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private Canvas hudCanvas;

    [SerializeField] public GameObject PauseMenu;
    [Header("death screen stats")]
    [SerializeField] public GameObject DeathSreen;
    [SerializeField] public TMP_Text ScoreText;
    [SerializeField] public TMP_Text LevelText;
    [SerializeField] public TMP_Text KilledEnemiesText;
    [SerializeField] public RoomGenerator roomGen;
    [SerializeField] public GameObject pickUpAbilityMenu;
    [SerializeField] public TMP_Text pickUpAbilityMenuName;
    [SerializeField] public TMP_Text pickUpAbilityMenuDescription;
    [SerializeField] public TMP_Text pickUpAbilityMenuCastKey;
    [SerializeField] public Image pickUpAbilityMenuIcon;
    [SerializeField] public GameObject AbilityPickupsStatue;

    private CanvasGroup hudCanvasGroup;

    void Start()
    {
        roomGen = this.GetComponent<RoomGenerator>();
    
        GameObject fader = GameObject.Find("FadeImage");
        if(fader != null){
            transition = fader.GetComponent<Animator>();
        }

        DeathSreen.SetActive(false);
        PauseMenu.SetActive(false);
        hudCanvasGroup = hudPanel.GetComponent<CanvasGroup>();
        hudCanvasGroup.ignoreParentGroups = true;
        Opacity_HUD.value = 1f;
        Size_HUD.value = 1f;

        // Opacity applies live, looks fine
        Opacity_HUD.onValueChanged.AddListener(val => hudCanvasGroup.alpha = val);

        // Size only applies on pointer release
        AddPointerUpListener(Size_HUD, () => hudCanvas.scaleFactor = Size_HUD.value);
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePauseMenu();
    }

    private void AddPointerUpListener(Slider slider, UnityEngine.Events.UnityAction callback)
    {
        EventTrigger trigger = slider.gameObject.GetComponent<EventTrigger>() 
                            ?? slider.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener(_ => callback());
        trigger.triggers.Add(entry);
    }


    public void TogglePauseMenu()
    {
        PauseMenu.SetActive(!PauseMenu.activeSelf);
    }

    public void Display_Death_Screen()
    {
        LevelText.text = "Reached level: " + roomGen.level;
        if (GameData.Instance)
        {
            KilledEnemiesText.text = "Enemies killed: " + GameData.Instance.killed_enemies;
        }  
        if (GameData.Instance)
        {
            ScoreText.text = "Your Score: " + GameData.Instance.points;
        }
        DeathSreen.SetActive(true);
    }

    public void AbilityExchange()
    {
        AbilityPickupsStatue.GetComponent<AbilityHolder>().ExchangeItem();
        pickUpAbilityMenu.SetActive(false);
    }

    public void Restart_Game()
    {
        if (GameData.Instance)
        {
            GameData.Instance.LoadNextSceneTransition();
        }
    }
    public Animator transition;
    public float transitioning_time = 1f;

    public IEnumerator FadeBetweenTransition(Action onMidpoint)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitioning_time);
        onMidpoint?.Invoke();
        transition.SetTrigger("Normal");
    }
    public void MainMenu()
    {
        StartCoroutine(FadeBetweenTransition(() =>
        {
            SceneManager.LoadScene("MainMenu");
        }));
    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    public void closePickUP()
    {
        pickUpAbilityMenu.SetActive(false);
    }
}