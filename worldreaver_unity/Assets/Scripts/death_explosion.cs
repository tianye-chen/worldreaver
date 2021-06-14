using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death_explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.667F);
        Destroy(gameObject);
    }
}
