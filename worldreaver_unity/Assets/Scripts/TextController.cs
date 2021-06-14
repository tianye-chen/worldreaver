using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public TextMeshProUGUI HPText;
    [SerializeField] public TextMeshProUGUI Score;
    [SerializeField] public TextMeshProUGUI Level;
    [SerializeField] public TextMeshProUGUI EnemyCount;
    [SerializeField] public TextMeshProUGUI Timer;
    [SerializeField] public TextMeshProUGUI FPS;
    [SerializeField] public GameObject Player;
    private int TotalEnemy = 0;
    private int CurrentEnemy = 0;
    private int LevelVal = 0;
    private float ScoreVal = 0;
    private bool IsLoaded = false;
    private float TimerTotal = 0;
    private int TimerMins = 0;
    private int TimerSecs = 0;
    private int TimerMilliSecs = 0;

    void Start()
    {
        ScoreVal = GameObject.FindGameObjectWithTag("Persist").GetComponent<DoNotDestroy_Data>().Score;
        TimerTotal = GameObject.FindGameObjectWithTag("Persist").GetComponent<DoNotDestroy_Data>().TotalElapsedTime;
        StartCoroutine(WaitForPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLoaded)
        {
            HPText.text = "HP: " + Player.GetComponent<PlaneController>().Health;
            Score.text = "Score: " + ScoreVal;
            Level.text = "Level: " + LevelVal;
            EnemyCount.text = "Enemies: "+CurrentEnemy + " / " + TotalEnemy;
            FPS.text = "FPS: " + (int)(1f / Time.unscaledDeltaTime);
        }
    }

    private void FixedUpdate()
    {
        TimerTotal += Time.deltaTime;
        TimerMilliSecs = (int)((TimerTotal * 100f) % 100f);
        TimerSecs = (int)((TimerTotal) % 60);
        TimerMins = (int)((TimerTotal) / 60f);
        if (TimerSecs >= 60)
            TimerSecs = 0;
        Timer.text = TimerMins.ToString("D2") + ":" + TimerSecs.ToString("D2") + "." + TimerMilliSecs.ToString("D2");
    }
    IEnumerator WaitForPlayer() 
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player"));
        Player = GameObject.FindGameObjectWithTag("Player");
        IsLoaded = true;
    }

    public void IncreaseScore(float val)
    {
        ScoreVal += val;
    }

    public void SetLevel(int val)
    {
        LevelVal = val;
    }

    public void SetCurrentEnemy(int val)
    {
        CurrentEnemy = val;
    }

    public void SetTotalEnemy(int val)
    {
        TotalEnemy = val;
    }

    public float GetScore()
    {
        return ScoreVal;    
    }

    public string TimeToString() 
    {
        return Timer.text = TimerMins.ToString("D2") + ":" + TimerSecs.ToString("D2") + "." + TimerMilliSecs.ToString("D2");
    }

    public float GetTime()
    {
        return TimerTotal;
    }
}
