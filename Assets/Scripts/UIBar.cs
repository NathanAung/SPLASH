using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI valText;
    public bool HPBar = true;
    PlayerDamagable damagable;
    PlayerController controller;
    
    
    private void Awake(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null){
            Debug.Log("No player found for UI");
        }
        if(HPBar){
            damagable = player.GetComponent<PlayerDamagable>();
            valText.text = damagable.HP + "/" + damagable.maxHP;
        }
        else{
            controller = player.GetComponent<PlayerController>();
            valText.text = (int)controller.stateTimer + "/" + (int)controller.stateTime;
        }
    }

    private void OnEnable(){
        if(HPBar){
            damagable.HealthChanged.AddListener(OnValueChanged);
        }
        else{
            controller.TimerChanged.AddListener(OnValueChanged);
        }
    }

    private void OnDisable(){
        if(HPBar){
             damagable.HealthChanged.RemoveListener(OnValueChanged);
        }
        else{
            controller.TimerChanged.RemoveListener(OnValueChanged);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(HPBar){
            slider.value = SliderPercentage(damagable.HP, damagable.maxHP);
        }
        else{
            slider.value = SliderPercentage(controller.stateTimer, controller.stateTime);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float SliderPercentage(float currentVal, float maxVal){
        return currentVal/maxVal;
    }

    private void OnValueChanged(float newVal, float maxVal)
    {
        slider.value = SliderPercentage(newVal, maxVal);
        valText.text = (int)newVal + "/" + maxVal;
    }
}
