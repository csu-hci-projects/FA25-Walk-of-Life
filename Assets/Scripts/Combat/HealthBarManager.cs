using UnityEngine;
using UnityEngine.UI;


public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private AttributesManager target;
    [SerializeField] private Slider slider;


    private Image fillImage;


    private void Awake()
    {
        if (slider != null && slider.fillRect != null)
        {
            fillImage = slider.fillRect.GetComponent<Image>();
        }
    }


    private void Start()
    {
        if (target != null && slider != null)
        {
            slider.maxValue = target.maxHealth;
            slider.value = target.health;
        }
    }


    private void Update()
    {
        if (target != null && slider != null)
        {
            slider.value = target.health;


            if (fillImage != null)
            {
                // Hide the green bar completely when health is 0
                fillImage.enabled = target.health > 0;
            }
        }
    }


    public void SetTarget(AttributesManager newTarget)
    {
        target = newTarget;
        if (target != null && slider != null)
        {
            slider.maxValue = target.maxHealth;
            slider.value = target.health;
        }
    }
}
