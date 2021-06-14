using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MenuTextEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI text; // The text whose allignment will be modified
    [SerializeField] public AudioClip OnClickSound;
    [SerializeField] public AudioClip OnHoverSound;

    private void Awake()
    {
        gameObject.GetComponent<AudioSource>().volume = DoNotDestroy_BGM.volume / 2;    
    }

    public void OnPointerEnter(PointerEventData eventData) // Set text alignment to center on mouse over
    {
        text.alignment = TextAlignmentOptions.Center;
        gameObject.GetComponent<AudioSource>().clip = OnHoverSound;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void OnPointerExit(PointerEventData eventData) // Return text to left alignment when mouse is not over
    {
        text.alignment = TextAlignmentOptions.Left;
    }

    public void PlayOnClickSound() 
    {
        gameObject.GetComponent<AudioSource>().clip = OnClickSound;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void ReturnToOriginPos() 
    {
        text.alignment = TextAlignmentOptions.Left;
    }
}
