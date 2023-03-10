using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtBar : MonoBehaviour
{

    // Can barının azalması ve dolması için gerekli slider
    public Slider slider;
    // Can barının can seviyesine göre rengini ayarlayan özellik
    public Gradient gradient;
    // Can barının can seviyesine göre rengini ayarlamak için olan image
    public Image fill;

    // Can barının maksimum değeri ve doluyken olan rengi
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    // Can barının o anki değeri ve rengi
    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
