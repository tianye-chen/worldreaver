using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefaultController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed = 1f;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Vector2 movement;
    [SerializeField] float Health;
    [SerializeField] public GameObject death_explosion;
    [SerializeField] public GameObject DefaultBullet;
    [SerializeField] public float FireRate = 1f;
    private GameObject InstBullet;
    private GameObject Player;
    private float timer = 0.0f;
    private float MovementTimer = 0f;
    private float LocX;
    private float LocY;

    private void Awake()
    {
        gameObject.GetComponent<AudioSource>().volume = DoNotDestroy_BGM.volume / 2;
    }

    void Start()
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();
        Health = DifficultyMenu.EnemyHealth[DifficultyMenu.EnemyHealth_Index];
        LocX = Random.Range(-12, 12);
        LocY = Random.Range(0, 8);
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (timer > FireRate)
        {
            InstBullet = Instantiate(DefaultBullet, gameObject.transform.position, Quaternion.identity);
            InstBullet.GetComponent<SpriteRenderer>().color = new Color(255, 255 ,0, 255);
            InstBullet.GetComponent<BulletController>().SetInstObject("Enemy");
            InstBullet.transform.up = Player.transform.position - InstBullet.transform.position;
            timer = 0.0f;
        }else
            timer += Time.deltaTime;
    }

    void FixedUpdate() 
    {
        if (transform.position.y > 8f)
        {
            transform.Translate(-Vector2.up * 0.02f);
        }
        else
        {
            MovementUpdate();
        }
    }

    void MovementUpdate()
    {
        if (MovementTimer >= 5)
        {
            LocX = Random.Range(-12, 12);
            LocY = Random.Range(0, 8);
            MovementTimer = 0;
        }
        else
            MovementTimer += Time.deltaTime;

        transform.position = Vector2.Lerp(transform.position, new Vector2(LocX,LocY), Time.deltaTime * (speed/10));
    }

    public void OnBulletHit() 
    {
        if (Health > 1)
        {
            Health -= 1;
            PlayHitSound();
        }
        else
        {
            Destroy(gameObject);
            GameObject.Find("TextManager").GetComponent<TextController>().IncreaseScore(50 * DifficultyMenu.ScoreMult);
            GameObject.Find("SpawnController").GetComponent<SpawnController>().DecreaseNumEnemy();
            Instantiate(death_explosion, gameObject.transform.position, Quaternion.identity);
            Player.GetComponent<PlaneController>().Health += (float)DifficultyMenu.PlayerRegen_Index / 2;
        }
    }

    public void PlayHitSound()
    {
        gameObject.GetComponent<AudioSource>().Play(0);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
        }
    }
}
