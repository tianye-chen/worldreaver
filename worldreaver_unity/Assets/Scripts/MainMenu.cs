using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Highscore;
    [SerializeField] TextMeshProUGUI BestTime;

    private void Start()
    {
        Highscore.text = "Highscore: " + GameObject.FindGameObjectWithTag("Persist").GetComponent<DoNotDestroy_Data>().HighScore;
        BestTime.text = "Best Time: " + GameObject.FindGameObjectWithTag("Persist").GetComponent<DoNotDestroy_Data>().BestTimeStr;
        GameObject.FindGameObjectWithTag("BGM").GetComponent<DoNotDestroy_BGM>().PlayMenu();
    }

    public void PlayGame() 
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLvl1()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLvl2()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadLvl3()
    {
        SceneManager.LoadScene(3);
    }
}
