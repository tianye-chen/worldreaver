using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UNUSED
public class ShieldBubble : MonoBehaviour
{
    [SerializeField] public GameObject player;
    private float timer = 0f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject.FindGameObjectWithTag("Persist").GetComponent<DoNotDestroy_Data>().Godmode = true;
    }

    void Update()
    {
        gameObject.transform.position = player.transform.position;
    }

    private void FixedUpdate()
    {
        if (timer >= 0.5f)
        {
            GameObject.FindGameObjectWithTag("Persist").GetComponent<DoNotDestroy_Data>().Godmode = false;
            Destroy(gameObject);
        }
        else
            timer += Time.deltaTime;
    }
}
