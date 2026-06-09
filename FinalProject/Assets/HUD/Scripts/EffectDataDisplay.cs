using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Reflection;

public class EffectDataDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    [SerializeField] private GameObject tooltip_;
    [SerializeField] private Vector3 offset_ = new Vector3(10, -10);
    public GameObject effectObject;

    private RectTransform tooltipRect_;

    [SerializeField] public int effect_index_;    
    [SerializeField] private EffectSystem effectManager_;
    [SerializeField] private UI_Manager ui;
    [SerializeField] private Image image;

    
    void Awake()
    {
        
        ui = GameObject.Find("GameManager").GetComponent<UI_Manager>();
        effectManager_ = GameObject.FindWithTag("Player").GetComponent<EffectSystem>();
        tooltip_ = ui.ToolTip;
        // tooltip_ = GameObject.Find("Tooltip");
        tooltipRect_ = tooltip_.GetComponent<RectTransform>();
        tooltip_.SetActive(false);

        image = this.GetComponent<Image>();
    }

    [SerializeField] private bool updates = false;
    void Update()
    {
        if(image.sprite == null)
        {
            image.sprite = effectManager_.effects[effect_index_].icon;
        }

        if (updates && tooltip_.activeSelf)
        {
            float w = tooltipRect_.rect.width;
            float h = tooltipRect_.rect.height;
            tooltipRect_.position = Input.mousePosition + new Vector3(-w/2, h / 2, 0) + offset_;
            
            var effect = effectManager_.effects[effect_index_];
            if (effect != null)
            {
                tooltip_.transform.GetChild(0).GetComponent<TMP_Text>().text = effect.name;
                tooltip_.transform.GetChild(1).GetComponent<TMP_Text>().text = effect.description;
                tooltip_.transform.GetChild(2).GetComponent<TMP_Text>().text = 
                effect.cooldown.ToString("F1") + " s";
            }
        }
    }
    private void OnDestroy()
    {
        tooltip_.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData e)
    {
        updates = true;
        tooltip_.SetActive(true);

        Color c = image.color;
        c.a = 1f; 
        image.color = c;
    }

    public void OnPointerExit(PointerEventData e)
    {
        updates = false;
        tooltip_.SetActive(false);
    
        Color c = image.color;
        c.a = 0.5f; // 0 = transparent, 1 = fully opaque
        image.color = c;
    
    }
}
