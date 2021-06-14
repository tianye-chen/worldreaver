using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyMenu : MonoBehaviour
{
    [SerializeField] public Button Right;
    [SerializeField] public Button Left;
    [SerializeField] public TextMeshProUGUI OptionText;
    [SerializeField] public TextMeshProUGUI ScoreMultText;
    public static float ScoreMult = 1;

    public static int[] Level1Enemies = new int[5] { 5, 10, 15, 20, 25 };
    public static int Level1Enemies_Index = 2;
    public static int[] Level2Enemies = new int[5] { 10, 15, 20, 25, 30 };
    public static int Level2Enemies_Index = 2;
    public static int[] MaxActiveEnemies = new int[3] { 5, 10, 15 };
    public static int MaxActiveEnemies_Index = 1;
    public static int[] EnemyHealth = new int[3] { 5, 10, 15 };
    public static int EnemyHealth_Index = 1;
    public static string[] BossDifficulty = new string[4] { "EASY", "NORMAL", "HARD", "CHAOS" };
    public static int BossDifficulty_Index = 1;
    public static float[] BossHealth = new float[3] { 0.75f, 1, 1.25f };
    public static int BossHealth_Index = 1;
    public static string[] PlayerRegen = new string[5] { "Off", "0.5", "1", "1.5", "2" };
    public static int PlayerRegen_Index = 3;
    public static int[] PlayerHealth = new int[5] { 1, 5, 10, 15, 20 };
    public static int PlayerHealth_Index = 3;

    private GameObject[] OptionTextArray;

    void Start()
    {
        OptionTextArray = GameObject.FindGameObjectsWithTag("OptionText");
        foreach (GameObject obj in OptionTextArray) // Sets all option texts to its appropirate value and checks if its within bounds of the array
        {
            switch (obj.transform.name)
            {
                case "Lvl1Enemy":
                    obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + Level1Enemies[Level1Enemies_Index];
                    IndexCheck(Level1Enemies_Index, 0, 4, obj);
                    break;
                case "Lvl2Enemy":
                    obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + Level2Enemies[Level2Enemies_Index];
                    IndexCheck(Level2Enemies_Index, 0, 4, obj);
                    break;
                case "MaxActiveEnemies":
                    obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + MaxActiveEnemies[MaxActiveEnemies_Index];
                    IndexCheck(MaxActiveEnemies_Index, 0, 2, obj);
                    break;
                case "EnemyHealth":
                    obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + EnemyHealth[EnemyHealth_Index];
                    IndexCheck(EnemyHealth_Index, 0, 2, obj);
                    break;
                case "BossDifficulty":
                    obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + BossDifficulty[BossDifficulty_Index];
                    IndexCheck(BossDifficulty_Index, 0, 3, obj);
                    break;
                case "BossHealth":
                    obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (BossHealth[BossHealth_Index] * 100) + "%";
                    IndexCheck(BossHealth_Index, 0, 2, obj);
                    break;
                case "PlayerRegen":
                    obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + PlayerRegen[PlayerRegen_Index];
                    IndexCheck(PlayerRegen_Index, 0, 4, obj);
                    break;
                case "PlayerHealth":
                    obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + PlayerHealth[PlayerHealth_Index];
                    IndexCheck(PlayerHealth_Index, 0, 4, obj);
                    break;
                default:
                    break;
            }
        }
    }

    void Update()
    {
        if (Level1Enemies_Index == 4 && Level2Enemies_Index == 4 && MaxActiveEnemies_Index == 2 && EnemyHealth_Index == 2 && BossDifficulty_Index == 3 &&
            BossHealth_Index == 2 && PlayerRegen_Index == 0 && PlayerHealth_Index == 0)
            ScoreMult = 5;
        else
            ScoreMult = (1f + (Level1Enemies_Index * 0.05f) + (Level2Enemies_Index * 0.1f) + (MaxActiveEnemies_Index * 0.2f) + (EnemyHealth_Index * 0.2f)
            + (Mathf.Pow(2, BossDifficulty_Index)) + (4 / Mathf.Pow(2, PlayerRegen_Index)) + (Mathf.Pow(2,-PlayerHealth_Index+2)) + (BossHealth_Index * 0.3f)) / 5;
        ScoreMultText.text = "Score Multiplier: " + ScoreMult + "x";
    }

    public void ChangeValue(int val)  // Increment or decrement the option values of arrays through button presses
    { 
        switch (transform.name) // Indentify which option is being changed through the name of the current object
        {
            case "Lvl1Enemy":
                Level1Enemies_Index += val;
                OptionText.text = "" + Level1Enemies[Level1Enemies_Index];
                IndexCheck(Level1Enemies_Index, 0, 4);
                break;
            case "Lvl2Enemy":
                Level2Enemies_Index += val;
                OptionText.text = "" + Level2Enemies[Level2Enemies_Index];
                IndexCheck(Level2Enemies_Index, 0, 4);
                break;
            case "MaxActiveEnemies":
                MaxActiveEnemies_Index += val;
                OptionText.text = "" + MaxActiveEnemies[MaxActiveEnemies_Index];
                IndexCheck(MaxActiveEnemies_Index, 0, 2);
                break;
            case "EnemyHealth":
                EnemyHealth_Index += val;
                OptionText.text = "" + EnemyHealth[EnemyHealth_Index];
                IndexCheck(EnemyHealth_Index, 0, 2);
                break;
            case "BossDifficulty":
                BossDifficulty_Index += val;
                OptionText.text = "" + BossDifficulty[BossDifficulty_Index];
                IndexCheck(BossDifficulty_Index, 0, 3);
                break;
            case "BossHealth":
                BossHealth_Index += val;
                OptionText.text = (BossHealth[BossHealth_Index] * 100)+"%";
                IndexCheck(BossHealth_Index, 0, 2);
                break;
            case "PlayerRegen":
                PlayerRegen_Index += val;
                OptionText.text = "" + PlayerRegen[PlayerRegen_Index];
                IndexCheck(PlayerRegen_Index, 0, 4);
                break;
            case "PlayerHealth":
                PlayerHealth_Index += val;
                OptionText.text = "" + PlayerHealth[PlayerHealth_Index];
                GameObject.FindGameObjectWithTag("Persist").GetComponent<DoNotDestroy_Data>().SetHealth(PlayerHealth[PlayerHealth_Index]);
                IndexCheck(PlayerHealth_Index, 0, 4);
                break;
            default:
                break;
        }
    }

    private void IndexCheck(int index, int LowerBound, int UpperBound) // Ensures that index stays within range of options array
    {
        if (index >= UpperBound && Right.interactable)
            Right.interactable = false;
        else if (index <= LowerBound && Left.interactable)
            Left.interactable = false;
        else 
        {
            Left.interactable = true;
            Right.interactable = true;
        }
    }

    private void IndexCheck(int index, int LowerBound, int UpperBound, GameObject obj) // Used in start function
    {
        if (index >= UpperBound && Right.interactable)
            obj.GetComponent<DifficultyMenu>().Right.interactable = false;
        else if (index <= LowerBound && Left.interactable)
            obj.GetComponent<DifficultyMenu>().Left.interactable = false;
        else
        {
            obj.GetComponent<DifficultyMenu>().Left.interactable = true;
            obj.GetComponent<DifficultyMenu>().Right.interactable = true;
        }
    }
}
