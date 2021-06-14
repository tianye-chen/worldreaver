using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoNotDestroy_BGM : MonoBehaviour
{
    [SerializeField] public AudioClip Reg;
    [SerializeField] public AudioClip Boss;
    [SerializeField] public AudioClip Boss2;
    [SerializeField] public AudioClip Menu;
    public static float volume = 0.5f;
    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("Persist").Length > 1)
            Destroy(gameObject);
        gameObject.GetComponent<AudioSource>().volume = volume;
        DontDestroyOnLoad(gameObject);
    }

    public static void SetVol(float val) 
    {
        volume = val;
        GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().volume = val;
    }

    public void PlayMenu()
    {
        gameObject.GetComponent<AudioSource>().volume = volume;
        if (gameObject.GetComponent<AudioSource>().clip != Menu)
        {
            gameObject.GetComponent<AudioSource>().clip = Menu;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    public void PlayReg() 
    {
        gameObject.GetComponent<AudioSource>().volume = volume * 2f;
        if (gameObject.GetComponent<AudioSource>().clip != Reg)
        {
            gameObject.GetComponent<AudioSource>().clip = Reg;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    public void PlayBoss() 
    {
        gameObject.GetComponent<AudioSource>().volume = volume;
        if (gameObject.GetComponent<AudioSource>().clip != Boss)
        {
            gameObject.GetComponent<AudioSource>().clip = Boss;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    public void PlayBoss2()
    {
        gameObject.GetComponent<AudioSource>().volume = volume;
        if (gameObject.GetComponent<AudioSource>().clip != Boss2)
        {
            gameObject.GetComponent<AudioSource>().clip = Boss2;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
