using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int MaxNumEnemey;
    [SerializeField] int TotalEnemy;
    [SerializeField] int NumEnemy = 0;
    [SerializeField] public GameObject DefaultEnemy;
    [SerializeField] public GameObject PlayerPrefab;
    [SerializeField] public TextController TextManager;
    [SerializeField] public DoNotDestroy_Data Persist;
    [SerializeField] public float SpawnRate = 1f;
    private float timer = 0.0f;
    private int EnemySpawned = 0;
    private int CurrentEnemy = 0;
    void Start()
    {
        Persist = GameObject.FindGameObjectWithTag("Persist").GetComponent<DoNotDestroy_Data>();
        Instantiate(PlayerPrefab, new Vector2(0, -8), Quaternion.identity);
        TextManager = GameObject.Find("TextManager").GetComponent<TextController>();
        switch (SceneManager.GetActiveScene().buildIndex) // Sets appropriate values of a given level
        {
            case 1:
                TotalEnemy = DifficultyMenu.Level1Enemies[DifficultyMenu.Level1Enemies_Index];
                MaxNumEnemey = DifficultyMenu.MaxActiveEnemies[DifficultyMenu.MaxActiveEnemies_Index];
                CurrentEnemy = TotalEnemy;
                TextManager.SetLevel(1);
                TextManager.SetTotalEnemy(TotalEnemy);
                TextManager.SetCurrentEnemy(TotalEnemy);
                GameObject.FindGameObjectWithTag("BGM").GetComponent<DoNotDestroy_BGM>().PlayReg();
                break;
            case 2:
                TotalEnemy = DifficultyMenu.Level2Enemies[DifficultyMenu.Level2Enemies_Index];
                MaxNumEnemey = DifficultyMenu.MaxActiveEnemies[DifficultyMenu.MaxActiveEnemies_Index];
                CurrentEnemy = TotalEnemy;
                TextManager.SetLevel(2);
                TextManager.SetTotalEnemy(TotalEnemy);
                TextManager.SetCurrentEnemy(TotalEnemy);
                GameObject.FindGameObjectWithTag("BGM").GetComponent<DoNotDestroy_BGM>().PlayReg();
                break;
            case 3:
                TotalEnemy = 1;
                MaxNumEnemey = 1;
                CurrentEnemy = TotalEnemy;
                TextManager.SetLevel(3);
                TextManager.SetTotalEnemy(TotalEnemy);
                TextManager.SetCurrentEnemy(TotalEnemy);
                GameObject.FindGameObjectWithTag("BGM").GetComponent<DoNotDestroy_BGM>().PlayBoss();
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex < 3)
        {
            if (MaxNumEnemey > NumEnemy && timer > SpawnRate && EnemySpawned < TotalEnemy)
            {
                Instantiate(DefaultEnemy, new Vector2(Random.Range(-16f, 16f), 11.08f), Quaternion.identity);
                NumEnemy++;
                EnemySpawned++;
                timer = 0f;
            }
            else if (CurrentEnemy != 0)
                timer += Time.deltaTime;
            else // When no enemies are left, advance to the next level
            {
                Persist.SetHealth(GameObject.FindGameObjectWithTag("Player").GetComponent<PlaneController>().Health);
                Persist.SetScore(TextManager.GetScore());
                Persist.SetTotalElapsedTime(TextManager.GetTime()); 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void FixedUpdate() // Prevents the enemy count going into the negatives
    {
        if (NumEnemy < 0)
            NumEnemy = 0;
    }

    public void DecreaseNumEnemy() // Called when an enemy is destroyed
    {
        NumEnemy--;
        CurrentEnemy--;
        TextManager.SetCurrentEnemy(CurrentEnemy);
    }
}
