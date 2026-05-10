using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu_;
    [SerializeField] private GameObject PlayerSelect_;
    [SerializeField] private GameObject Credits;
    [SerializeField] private string MainGameSceneName_ = "MainGame";
    
    void Start()
    {
        transition = GameObject.Find("FadeImage").GetComponent<Animator>();
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

    public void Open_MainMenu()
    {
        StartCoroutine(FadeBetweenTransition(() =>
        {
            MainMenu_.SetActive(true);
            PlayerSelect_.SetActive(false);
            Credits.SetActive(false);
        }));
    }

    public void Open_PlayerSelect()
    {
        StartCoroutine(FadeBetweenTransition(() =>
        {
            MainMenu_.SetActive(false);
            PlayerSelect_.SetActive(true);
            Credits.SetActive(false);

        }));
    }  
    public void Open_Credits()
    {
        StartCoroutine(FadeBetweenTransition(() =>
        {
            MainMenu_.SetActive(false);
            PlayerSelect_.SetActive(false);
            Credits.SetActive(true);

        }));
    }
    public void Start_Game()
    {
        StartCoroutine(FadeBetweenTransition(() =>
        {
            SceneManager.LoadScene(MainGameSceneName_);
        }));
    }
}
