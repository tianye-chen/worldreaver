using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [SerializeField] float movement;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] float verticalMove;
    [SerializeField] public GameObject DefaultBullet;
    [SerializeField] public GameObject death_explosion;
    public float Health;
    private float Speed = 10f;
    private float FireRate = 0.080f;
    private float ProjectileSpeed = 5f;
    private GameObject InstBullet;
    private DoNotDestroy_Data Persist;
    private float timer = 0.0f;
    private bool IsDead = false;

    void Start()
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();
        Persist = GameObject.FindGameObjectWithTag("Persist").GetComponent<DoNotDestroy_Data>();
        Health = Persist.PlayerHealth;
    }

    void FixedUpdate()
    {
        movement = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
        if (Input.GetKey("space") && timer > FireRate && !IsDead)
        {
            InstBullet = Instantiate(DefaultBullet, gameObject.transform.position, Quaternion.identity);
            InstBullet.gameObject.GetComponent<BulletController>().SetInstObject("Player");
            InstBullet.gameObject.GetComponent<BulletController>().SetPlayerProjectileSpeed(ProjectileSpeed);
            timer = 0;
        }
        else
            timer += Time.deltaTime;
        rigid.velocity = new Vector2(movement * Speed, verticalMove * Speed);
    }

    public void OnBulletHit() 
    {
        if (Health > 1 && !Persist.Godmode && !IsDead)
            Health -= 1;
        else if (Health <= 1)
        {
            Health -= 1;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Instantiate(death_explosion, gameObject.transform.position, Quaternion.identity);
            IsDead = true;
            StartCoroutine(ShipDestroyed());
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BossProjectile")
        {
            OnBulletHit();
        }
    }

    IEnumerator ShipDestroyed() 
    { 
        yield return new WaitForSeconds(0.667F);
        GameObject.Find("GameMenu").GetComponent<GameMenu>().GameLost();
    }
}
