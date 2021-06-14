using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour
{
    // Start is called before the first frame update
    private float timer = 0;
    void Start()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 2.4f)
        {
            Destroy(gameObject);
        }
        else if (timer > 1f)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        timer += Time.deltaTime;
    }
}
