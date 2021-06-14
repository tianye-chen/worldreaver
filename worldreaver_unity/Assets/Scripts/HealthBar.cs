using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject Boss;
    [SerializeField] Slider SliderObj;
    [SerializeField] TextMeshProUGUI HealthText;
    // Start is called before the first frame update
    void Start()
    {
        if (Boss == null)
            Boss = GameObject.FindGameObjectWithTag("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        if (Boss != null)
        {
            SliderObj.value = Boss.GetComponent<BossController>().Health / Boss.GetComponent<BossController>().MaxHealth;
            HealthText.text = "" + Boss.GetComponent<BossController>().Health;
        } else
            HealthText.text = "0";
    }
}
