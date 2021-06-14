using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DoNotDestroy_Data : MonoBehaviour
{
    [SerializeField] public bool Godmode = false;
    [SerializeField] public float Score = 0;
    [SerializeField] public float HighScore = 0;
    [SerializeField] public string BestTimeStr = "N/A";
    public float PlayerHealth;
    public float TotalElapsedTime = 0;
    public float BestTime = 0;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("Persist").Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayerHealth = DifficultyMenu.PlayerHealth[DifficultyMenu.PlayerHealth_Index];
    }

    public void SetScore(float val)
    {
        Score = val;
    }

    public void SetHealth(float val)
    {
        PlayerHealth = val;
    }

    public void SetTotalElapsedTime(float val)
    {
        TotalElapsedTime = val;
    }

    public void SetHighScore() 
    {
        if (Score > HighScore)
            HighScore = Score;
    }

    public void SetBestTime() 
    {
        float time = GameObject.Find("TextManager").GetComponent<TextController>().GetTime();
        if (BestTime == 0 || time < BestTime)
        {
            Debug.Log("Total: "+TotalElapsedTime+" Best: "+BestTime);
            BestTime = time;
            BestTimeStr = GameObject.Find("TextManager").GetComponent<TextController>().TimeToString();
        }
    }
}
