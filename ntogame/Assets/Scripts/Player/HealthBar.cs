using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    [SerializeField] Gradient Gradient;
    [SerializeField] Image Fill;
    [SerializeField] float smooth = 0.6f;
    private float refVel;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = GameManager.MaxHealth;
        slider.value = slider.maxValue;
    }

    void Update()
    {
        slider.value = Mathf.SmoothDamp(slider.value, GameManager.Health, ref refVel, smooth);
        Fill.color = Gradient.Evaluate(slider.normalizedValue);
    }
}
