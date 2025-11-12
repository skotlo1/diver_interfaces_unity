using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class BatteryBar : MonoBehaviour
{
    private Slider slider;
    public InformationManager iM;
    
    [Header("WARNING: Low Battery")]
    public GameObject warningPanel;
    public float warningThreshold = 15f;
    
    [Header("Bar Colors")]
    public Image fillImage;
    public Color normalColor = Color.green;
    public Color warningColor = Color.yellow;
    public Color criticalColor = Color.red;

    void Start()
    {
        slider = GetComponent<Slider>();
        SetMaxBatteryLevel(iM.GetMaxBatteryLevel());
        
        if (warningPanel != null)
            warningPanel.SetActive(false);
    }

    public void SetMaxBatteryLevel(float maxLevel)
    {
        slider.maxValue = maxLevel;

        slider.value = maxLevel;
    }

    public void SetBatteryLevel(float level)
    {
        slider.value = level;
        
        UpdateBatteryColor(level);
    }

    private void UpdateBatteryColor(float level)
    {
        if (fillImage == null) return;
        
        float percent = (level / slider.maxValue) * 100f;
        
        if (percent <= 5f)
        {
            fillImage.color = criticalColor;
        }
        else if (percent <= warningThreshold)
        {
            fillImage.color = warningColor;
        }
        else
        {
            fillImage.color = normalColor;
        }
    }

    private void Update()
    {
        float status = iM.GetBatteryLevel();

        SetBatteryLevel(status);
        
        float percent = (status / iM.GetMaxBatteryLevel()) * 100f;
        
        if (warningPanel != null)
        {
            if (percent <= warningThreshold)
            {
                warningPanel.SetActive(true);
            }
            else
            {
                warningPanel.SetActive(false);
            }
        }
    }
}