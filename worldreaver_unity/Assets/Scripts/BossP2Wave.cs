using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossP2Wave : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rigid;
    public float speed = 10f;

    private void Awake()
    {
        gameObject.GetComponent<AudioSource>().volume = DoNotDestroy_BGM.volume / 2;
    }
    void Start()
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (gameObject.name == "waveattack_1(Clone)")
            rigid.AddForce(-transform.up * speed);
        else
            transform.Translate(-Vector2.up * 3 / speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlaneController>().OnBulletHit();
            Destroy(gameObject);
        }
        else if ((collision.gameObject.tag == "Boundary" && gameObject.transform.position.y < 5) || (gameObject.name == "waveattack_2(Clone)" && collision.gameObject.tag == "Boundary"))
            StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1F);
        Destroy(gameObject);
    }
}
