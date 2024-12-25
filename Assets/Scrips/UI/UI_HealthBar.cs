using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private CharacterStats stats;
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        stats = GetComponentInParent<CharacterStats>();
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();

        entity.onFlipped += FilpUI;
        stats.onCurrentHealthChange += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void Update()
    {
        
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = stats.getMaxHealthValue();
        slider.value = stats.getCurrentHealthValue();
    }

    private void FilpUI()
    {
        myTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        entity.onFlipped -= FilpUI;
        stats.onCurrentHealthChange -= UpdateHealthUI;
    }
}
