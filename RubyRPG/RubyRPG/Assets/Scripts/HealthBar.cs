using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider HPbar;
    private RubyController ruby;
    void Start()
    {
        ruby = gameObject.GetComponent<RubyController>();
    }
    void Update()
    {
        HPbar.value = (float)ruby.currentHealth / (float)ruby.maxHealth;
        
    }

   
}

