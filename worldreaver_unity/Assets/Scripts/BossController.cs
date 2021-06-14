using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] public float MaxHealth = 500;
    [SerializeField] public float Health = 500;
    [SerializeField] public float FireRate = 1f;
    [SerializeField] public float RocketsFireRate = 2f;
    [SerializeField] public Rigidbody2D rigid;
    [SerializeField] public GameObject GameMenu;
    [SerializeField] public GameObject death_explosion;
    [SerializeField] public GameObject DefaultBullet;
    [SerializeField] public GameObject beam;
    [SerializeField] public GameObject rocket;
    [SerializeField] public GameObject BossP2Ball;
    [SerializeField] public GameObject BossP2Wave_1;
    [SerializeField] public GameObject BossP2Wave_2;
    [SerializeField] public Animator BossTransitions;
    private GameObject InstBullet; // Used to operate on instanced objects
    private bool InPosition = false; // Used to check if boss intro sequence is over
    private bool IsDead = false;
    private bool IsPhase2Dead = false;
    private bool IsAttacking = false;
    private int phase = 1;
    private float timer = 0f; // Used to determine the interval which the basic attack will occur
    private float BeamTimer = 0f; // Used to determine the interval which the beam attack will occur
    private float RocketTimer = 0f; // Used to determine the interval which the rocket attack will occur
    private float AttackCooldownTimer = 2; // Used to determine the interval which phase 2 attacks will occur
    private float AttackCooldownDuration = 5; // The time between attacks of phase 2
    private bool ReadyToFire = false; // Used for blast attack of phase 2
    private GameObject BeamTemp;

    private void Awake()
    {
        gameObject.GetComponent<AudioSource>().volume = DoNotDestroy_BGM.volume / 2;
    }

    void Start()
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();
        MaxHealth *= DifficultyMenu.BossHealth[DifficultyMenu.BossHealth_Index];
        Health = MaxHealth;
    }

    private void FixedUpdate()
    {
        if (phase == 1)
            Phase1Boss();
        else
            Phase2Boss();
        if (transform.position.y > 9.75f)
            transform.Translate(-Vector2.up * 0.02f);
        else
            InPosition = true;

    }

    private void Phase1Boss()
    {
        if (InPosition)
        {
            if (timer > 1) // If time between last call is greater than 1s, perform basic attack
            {
                Phase1BasicAttacks();
                timer = 0f;
            }
            else //Add time to the timer
                timer += Time.deltaTime;
            if (BeamTimer > 5 && Health / MaxHealth <= 0.75) // If hp is less than 75%, perform laser attack
            {
                BeamAttack();
                BeamTimer = 0f;
            }
            else
                BeamTimer += Time.deltaTime;
            if (RocketTimer > RocketsFireRate) // If time between last attack is greater than RocketsFireRate, fire a rocket at the player
            {
                RocketAttack();
                RocketTimer = 0f;
            }
            else
                RocketTimer += Time.deltaTime;
        }
    }

    private void Phase1BasicAttacks() 
    {
        if (Health / MaxHealth >= 0.50 || DifficultyMenu.BossDifficulty_Index < 2) // If above 50% hp or difficulty is EASY or NORMAL
        {
            if (DifficultyMenu.BossDifficulty_Index >= 1)
            {
                float rng = 0;
                if (DifficultyMenu.BossDifficulty_Index >= 2) // If difficulty is HARD and above, then add RNG element to the spread attack
                    rng = Random.Range(-15f, 15f);
                for (float i = 90 + rng; i <= 270 + rng; i += 15) // Spread attack with 15 gap between each shot
                    SpreadAttack(i);
            }
            else // If difficulty is EASY
            {
                float rng = Random.Range(90f, 270f);
                for (float i = 90; i <= 270; i += 7)
                    if (!(i >= rng + 15) && !(i <= rng - 15)) // If value of i is within the range of rng +/- 15
                        SpreadAttack(i);
            }
        }
        else // If below 50% hp and difficulty is HARD and above (>=2)
        {
            float rng = Random.Range(90f, 270f);
            for (float i = 90; i <= 270; i += 7) // Gap between the spread attack is smaller
                if (!((i >= rng - 15) && (i <= rng + 15))) //If value of i is outside the range of rng +/ -15
                    SpreadAttack(i);
        }
    }

    //Spawns a bullet and rotate it to the angle parameter
    private void SpreadAttack(float angle)
    {
        InstBullet = Instantiate(DefaultBullet, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 2), Quaternion.identity);
        InstBullet.GetComponent<SpriteRenderer>().color = new Color(255, 255, 0, 255);
        InstBullet.GetComponent<BulletController>().SetInstObject("Boss");
        InstBullet.transform.eulerAngles = Vector3.forward * angle;
    }

    //Spawns two laser beams at specific location
    private void BeamAttack()
    {
        BeamTemp = Instantiate(beam, new Vector2(7f, 0f), beam.transform.rotation);
        BeamTemp.transform.eulerAngles = Vector3.forward * 270;
        BeamTemp = Instantiate(beam, new Vector2(-7f, 0f), beam.transform.rotation);
        BeamTemp.transform.eulerAngles = Vector3.forward * 270;
    }

    //Spawns a rocket at one of two locations
    private void RocketAttack()
    {
        if (Random.Range(0f, 1f) > .50f)
            Instantiate(rocket, new Vector2(3.5f, 4.5f), Quaternion.identity);
        else
            Instantiate(rocket, new Vector2(-3.5f, 4.5f), Quaternion.identity);
    }

    private void Phase2Boss() 
    {
        if (Health <= MaxHealth * 0.50) // When below 50% hp, attack twice as fast
            AttackCooldownDuration = 2.5f;
        if (AttackCooldownTimer > AttackCooldownDuration && !IsAttacking)
        {
            float RandomNum = Random.Range(0, 5);
            Debug.Log("New attack cycle with attack #"+RandomNum);
            int rounded = Mathf.RoundToInt(RandomNum);
            switch (rounded)
            {
                case (0):
                    StartCoroutine(WaveAttack_1());
                    break;
                case (1):
                    StartCoroutine(SpiralAttack());
                    break;
                case (2):
                    StartCoroutine(WaveAttack_2());
                    break;
                case (3):
                    StartCoroutine(SummonAttack());
                    break;
                case (4):
                    StartCoroutine(BlastAttack());
                    break;
                default:
                    break;
            }
            AttackCooldownTimer = 0;
        }
        else
            AttackCooldownTimer += Time.deltaTime;
    }

    IEnumerator WaveAttack_1()  // Wave attacks will fall down from the top of the screen
    {
        IsAttacking = true;
        BossTransitions.SetBool("ArmRaised", true);
        float AttackTimer = 0;
        float AttackDuration = 0;
        while (AttackDuration <= 10)
        {
            yield return new WaitForEndOfFrame();
            if (AttackTimer > 0.3f)
            {
                Instantiate(BossP2Wave_1, new Vector2(Random.Range(-12, 12), 12), Quaternion.identity);
                AttackTimer = 0;
            }
            else
                AttackTimer += Time.deltaTime;
            AttackDuration += Time.deltaTime;
        }
        IsAttacking = false;
        BossTransitions.SetBool("ArmRaised", false);
        AttackCooldownTimer = 0;
    }

    IEnumerator WaveAttack_2() // The boss will rapidly fire wave attacks in a 180 degree area
    {
        IsAttacking = true;
        float AttackDuration = 0;
        BossTransitions.SetBool("IsSlashing", true);
        while (AttackDuration <= 10)
        { 
            yield return new WaitForSeconds(0.1f);
            InstBullet = Instantiate(BossP2Wave_2, new Vector2(0,5), Quaternion.identity);
            InstBullet.transform.eulerAngles = Vector3.forward * Random.Range(-90,90);
            AttackDuration += 0.1f;
        }
        BossTransitions.SetBool("IsSlashing", false);
        yield return new WaitForSeconds(0.6f);
        BossTransitions.SetBool("ArmRaised", true);
        yield return new WaitForSeconds(0.2f);
        BossTransitions.SetBool("ArmRaised", false);
        for (int i = -90; i <= 90; i += 25) // Final instance of this boss attack, instantly fires several fire waves in a 180 degree area
        {
            InstBullet = Instantiate(BossP2Wave_2, new Vector2(0, 5), Quaternion.identity);
            InstBullet.transform.eulerAngles = Vector3.forward * i;
        }
        IsAttacking = false;
        AttackCooldownTimer = 0;
    }

    IEnumerator SpiralAttack() // The boss will summon bullets which will travel in 2 opposite spirals
    {
        IsAttacking = true;
        BossTransitions.SetBool("IsStance", true);
        float AttackDuration = 0;
        while (AttackDuration <= 10)
        {
            yield return new WaitForSeconds(0.3f);
            for (int i = -1; i <= 1; i+=2)
            {
                InstBullet = Instantiate(DefaultBullet, new Vector2(0, 6), Quaternion.identity);
                InstBullet.GetComponent<BulletController>().SetInstObject("BossP2Spiral");
                InstBullet.GetComponent<BulletController>().SetSpiralDirection(i);
            }
            AttackDuration += 0.3f;
        }
        IsAttacking = false;
        BossTransitions.SetBool("IsStance", false);
        AttackCooldownTimer = 0;
    }

    IEnumerator SummonAttack() // The boss will summon 2 yellow orbs which will move 2 random directions firing bullets
    {
        IsAttacking = true;
        BossTransitions.SetBool("IsStance", true);
        float AttackDuration = 0;
        while(AttackDuration <= 10)
        {
            yield return new WaitForSeconds(3f);
            for (int i = -1; i <= 1; i+=2)
            {
                InstBullet = Instantiate(BossP2Ball, new Vector2(0, 6), Quaternion.identity);
                InstBullet.transform.eulerAngles = Vector3.forward * Random.Range(0,360);
                InstBullet.GetComponent<BossP2Ball>().SetIsAttack(true);
            }
            AttackDuration += 3f;
        }
        IsAttacking = false;
        BossTransitions.SetBool("IsStance", false);
        AttackCooldownTimer = 0;
    }

    IEnumerator BlastAttack() // The boss will first charge up a number of bullets in a circle around him, then fire them all at once
    {
        IsAttacking = true;
        BossTransitions.SetBool("IsStance", true);
        float AttackDuration = 0;
        float AttackInterval = 3f;
        while (AttackDuration <= 10)
        {
            yield return new WaitForSeconds(AttackInterval);
            ReadyToFire = false;
            for (int i = 0; i <= 360; i += Random.Range(10, 20))
            {
                InstBullet = Instantiate(DefaultBullet, new Vector2(0,6), Quaternion.identity);
                InstBullet.transform.eulerAngles = Vector3.forward * i;
                InstBullet.transform.Translate(Vector2.up * 3.5f);
                InstBullet.GetComponent<BulletController>().SetInstObject("BossP2BlastWait");
                InstBullet.GetComponent<BulletController>().PlayBlastSpawnSound();
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.2f);
            ReadyToFire = true; // Tells BulletController.cs that the charge up is over and is ready to fire all the bullets
            AttackDuration += AttackInterval;
            AttackInterval -= 0.5f;
        }
        IsAttacking = false;
        BossTransitions.SetBool("IsStance", false);
        AttackCooldownTimer = 0;
    }

    public void OnBulletHit()
    {
        if (Health >= 1 && InPosition && !IsDead)
        {
            Health--;
            GameObject.Find("TextManager").GetComponent<TextController>().IncreaseScore(10 * DifficultyMenu.ScoreMult);
            PlayHitSound();
        }
        else if (InPosition && !IsDead)
        {
            InPosition = false;
            IsDead = true;
            if (phase == 2)
                IsPhase2Dead = true;
            for (int i = 0; i < 10; i++)
            {
                Instantiate(death_explosion, new Vector2(Random.Range(-9, 9), Random.Range(7, 9)), Quaternion.identity);
            }
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(BossDestroyed());
        }
    }

    IEnumerator BossDestroyed()
    {
        yield return new WaitForSeconds(0.667F);
        if (IsPhase2Dead || DifficultyMenu.BossDifficulty_Index != 3)
        {
            GameObject.Find("TextManager").GetComponent<TextController>().IncreaseScore(500 * DifficultyMenu.ScoreMult);
            GameMenu.GetComponent<GameMenu>().GameWon();
            Destroy(gameObject);
            GameObject.Find("SpawnController").GetComponent<SpawnController>().DecreaseNumEnemy();
        }
        else
            StartPhase2();
    }

    private void StartPhase2() 
    {
        phase = 2;
        BossTransitions.SetInteger("phase", phase);
        Health = 1000 * DifficultyMenu.BossHealth[DifficultyMenu.BossHealth_Index];
        MaxHealth = Health;
        gameObject.transform.localScale = new Vector3(10f, 10f, 5f);
        gameObject.transform.position = new Vector2(1.54f, 6f);
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.496746f, 0.4308989f);
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.1622606f, -0.004992545f);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        Instantiate(BossP2Ball, new Vector2(-5,7), Quaternion.identity);
        Instantiate(BossP2Ball, new Vector2(5, 7), Quaternion.identity);
        GameObject.FindGameObjectWithTag("BGM").GetComponent<DoNotDestroy_BGM>().PlayBoss2();
        IsDead = false;
    }

    public bool IsReadyToFire() 
    {
        return ReadyToFire;
    }

    public void PlayHitSound()
    {
        gameObject.GetComponent<AudioSource>().Play(0);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boundary")
        {
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
        }
    }
}
