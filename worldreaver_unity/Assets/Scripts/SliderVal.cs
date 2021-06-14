using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SliderVal : MonoBehaviour
{
    [SerializeField] public Slider VolSlider;
    // Start is called before the first frame update
    void Start()
    {
        VolSlider = GameObject.Find("Slider").GetComponent<Slider>();
        VolSlider.value = DoNotDestroy_BGM.volume;
        VolSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public void OnValueChanged() 
    {
        DoNotDestroy_BGM.SetVol(VolSlider.value);
    }
}
