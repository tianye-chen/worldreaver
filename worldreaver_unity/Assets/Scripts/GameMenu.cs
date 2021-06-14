using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject LoseScreen;
    [SerializeField] GameObject WinScreen;
    [SerializeField] DoNotDestroy_Data Persist;
    [SerializeField] Button MenuButton;
    [SerializeField] TextMeshProUGUI Score;
    [SerializeField] TextMeshProUGUI HighScore;
    [SerializeField] TextMeshProUGUI WinScore;
    [SerializeField] TextMeshProUGUI WinHighScore;
    [SerializeField] TextMeshProUGUI TimeOnLoss;
    [SerializeField] TextMeshProUGUI TimeOnWin;
    public static bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        Persist = GameObject.FindGameObjectWithTag("Persist").GetComponent<DoNotDestroy_Data>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !LoseScreen.activeSelf)
        {
            if (paused)
            {
                Resume();  
            }
            else 
            {
                Time.timeScale = 0f;
                paused = true;
                PauseMenu.SetActive(true);
            }
        }
    }

    public void Resume() 
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    public void Menu()
    {
        Resume();
        Persist.SetScore(0);
        Persist.SetHealth(DifficultyMenu.PlayerHealth[DifficultyMenu.PlayerHealth_Index]);
        Persist.SetTotalElapsedTime(0);
        SceneManager.LoadScene(0);
    }

    public void Retry() 
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        Persist.SetScore(0);
        Persist.SetHealth(DifficultyMenu.PlayerHealth[DifficultyMenu.PlayerHealth_Index]);
        Persist.SetTotalElapsedTime(0);
    }

    public void GameLost()
    {
        LoseScreen.SetActive(true);
        Time.timeScale = 0f;
        Persist.SetScore(GameObject.Find("TextManager").GetComponent<TextController>().GetScore());
        Persist.SetHighScore();
        Score.text = "Score: " + Persist.Score;
        HighScore.text = "Highscore: " + Persist.HighScore;
        TimeOnLoss.text = "Time: " + GameObject.Find("TextManager").GetComponent<TextController>().TimeToString();
    }

    public void GameWon() 
    {
        MenuButton.interactable = true;
        WinScreen.SetActive(true);
        Time.timeScale = 0f;
        Persist.SetScore(GameObject.Find("TextManager").GetComponent<TextController>().GetScore());
        Persist.SetHighScore();
        Persist.SetBestTime();
        WinScore.text = "Score: " + Persist.Score;
        WinHighScore.text = "Highscore: " + Persist.HighScore;
        TimeOnWin.text = "Time: " + GameObject.Find("TextManager").GetComponent<TextController>().TimeToString();
    }
}
