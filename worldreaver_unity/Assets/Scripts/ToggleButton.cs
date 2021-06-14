using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] bool state = false;
    [SerializeField] GameObject handle;
    public RawImage toggle_bg;

    // Start is called before the first frame update
    void Start()
    {
        handle = GameObject.Find("ToggleHandle");
        toggle_bg.color = new Color(255, 0, 0);
        if (GameObject.FindGameObjectWithTag("Persist").GetComponent<DoNotDestroy_Data>().Godmode)
        {
            handle.transform.position = new Vector3(handle.transform.position.x + 1.5f, handle.transform.position.y, handle.transform.position.z);
            state = true;
            toggle_bg.color = new Color(0, 255, 0);
        }
    }

    public void OnMouseUp()
    {
        if (!state)
        {
            handle.transform.position = new Vector3(handle.transform.position.x + 1.5f, handle.transform.position.y, handle.transform.position.z);
            state = true;
            toggle_bg.color = new Color(0, 255, 0);
        }
        else 
        {
            handle.transform.position = new Vector3(handle.transform.position.x - 1.5f, handle.transform.position.y, handle.transform.position.z);
            state = false;
            toggle_bg.color = new Color(255, 0, 0);
        }
        GameObject.FindGameObjectWithTag("Persist").GetComponent<DoNotDestroy_Data>().Godmode = state;
    }
}
