using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletController : MonoBehaviour
{
    [SerializeField] float verticalMove;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Sprite BossP2Ball_Sprite;
    [SerializeField] Sprite RedBullet_Sprite;
    [SerializeField] GameObject BossP2;
    [SerializeField] public AudioClip BlastSpawn_Sound;
    [SerializeField] public AudioClip BlastShoot_Sound;
    private string instObject;
    private float EnemyProjectileSpeed = 1;
    private float PlayerProjectileSpeed = 1;
    private float SpiralMove = 0;
    private float SpiralSpeed= 0;
    private int SpiralDir = 1;
    private bool BlastSpawn = true;

    private void Awake()
    {
        gameObject.GetComponent<AudioSource>().volume = DoNotDestroy_BGM.volume / 2;
    }
    void Start()
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();
        if (SceneManager.GetActiveScene().buildIndex == 3)
            BossP2 = GameObject.FindGameObjectWithTag("Boss");
    }

    void FixedUpdate()
    {
        switch (instObject)
        {
            case ("Enemy"):
                transform.Translate(Vector2.up * EnemyProjectileSpeed/10);
                break;
            case ("Player"):
                transform.Translate(Vector2.up * PlayerProjectileSpeed/10);
                break;
            case ("Boss"):
                transform.Translate(Vector2.up * EnemyProjectileSpeed/10);
                break;
            case ("BossP2Spiral"):
                gameObject.GetComponent<SpriteRenderer>().sprite = BossP2Ball_Sprite;
                gameObject.GetComponent<Renderer>().material.color = Color.HSVToRGB(0, 0.45f, 1);
                gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.2498093f, 0.2494715f);
                gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.005095348f, 0.004776936f);
                gameObject.transform.localScale = new Vector3(2, 2, 0);
                SpiralSpeed += Time.deltaTime + Time.deltaTime * EnemyProjectileSpeed / 20 ;

                float x = Mathf.Cos(SpiralSpeed) * SpiralMove; // (Cos(SpiralSpeed), Sin(SpiralSpeed)) will create a circle pattern
                float y = Mathf.Sin(SpiralSpeed) * SpiralMove; // Multiplying it with SpiralMove will create a spiral pattern

                transform.Translate(new Vector2(x * SpiralDir, -y * SpiralDir)); // Move the bullet to along the path of the pattern
                SpiralMove += 0.0005f; // Slightly increasing the value to create a spiral pattern
                if (gameObject.transform.position.y <= -15)
                    Destroy(gameObject);
                break;
            case ("BossP2BlastWait"):
                gameObject.GetComponent<SpriteRenderer>().sprite = RedBullet_Sprite;
                if (BlastSpawn)
                {
                    BlastSpawn = false;
                    StartCoroutine(BlastAttackWait());
                }
                break;
            case ("BossP2BlastMove"):
                transform.Translate(Vector2.up * EnemyProjectileSpeed);
                if (transform.position.x > 20 || transform.position.x < -20 || transform.position.y > 20 || transform.position.y < -20)
                    Destroy(gameObject);
                break;
            default:
                transform.Translate(Vector2.up * 0.1f);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag) {
            case ("Enemy"):
                if (instObject == "Player")
                {
                    collision.gameObject.GetComponent<EnemyDefaultController>().OnBulletHit();
                    Destroy(gameObject);
                }
                break;
            case ("Player"):
                if (instObject != "Player")
                {
                    collision.gameObject.GetComponent<PlaneController>().OnBulletHit();
                    Destroy(gameObject);
                }
                break;
            case ("Boss"): 
                if (instObject == "Player"){
                    collision.gameObject.GetComponent<BossController>().OnBulletHit();
                    Destroy(gameObject);
                }
                break;
            case ("Boundary"):
                if (instObject != "BossP2Spiral")
                    StartCoroutine(Wait());
                break;
            default:
                break;
        }
    }

    public void SetInstObject(string obj) 
    {
        instObject = obj;
    }

    public void SetSpiralDirection(int val)
    {
        SpiralDir = val;
    }

    public void SetPlayerProjectileSpeed(float val) 
    {
        PlayerProjectileSpeed = val;
    }

    public void PlayBlastSpawnSound() 
    {
        gameObject.GetComponent<AudioSource>().volume = DoNotDestroy_BGM.volume / 2;
        gameObject.GetComponent<AudioSource>().clip = BlastSpawn_Sound;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void PlayBlastShootSound()
    {
        gameObject.GetComponent<AudioSource>().clip = BlastShoot_Sound;
        gameObject.GetComponent<AudioSource>().volume = DoNotDestroy_BGM.volume / 8;
        gameObject.GetComponent<AudioSource>().Play();
    }

    IEnumerator BlastAttackWait() 
    {
        yield return new WaitUntil(() => BossP2.GetComponent<BossController>().IsReadyToFire());
        instObject = "BossP2BlastMove";
        PlayBlastShootSound();
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
