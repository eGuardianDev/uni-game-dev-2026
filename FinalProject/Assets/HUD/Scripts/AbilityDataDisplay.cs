using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AbilityDataDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltip_;
    [SerializeField] private Vector2 offset_ = new Vector2(10, -10);

    private RectTransform tooltipRect_;

    [SerializeField] private int ability_index_;    
    [SerializeField] private AbilityManager abilityManager_;
    void Start()
    {
        abilityManager_ = GameObject.FindWithTag("Player").GetComponent<AbilityManager>();
        // tooltip_ = GameObject.Find("Tooltip");
        tooltipRect_ = tooltip_.GetComponent<RectTransform>();
        tooltip_.SetActive(false);
    }

    [SerializeField] private bool updates = false;
    void Update()
    {
       if (updates && tooltip_.activeSelf)
        {
            float w = tooltipRect_.rect.width;
            float h = tooltipRect_.rect.height;
            tooltipRect_.position = Input.mousePosition + new Vector3(-w/2, h / 2, 0);
            
            var ability = abilityManager_.abilities[ability_index_];
            if (ability != null)
            {
                tooltip_.transform.GetChild(0).GetComponent<TMP_Text>().text = ability.Name;
                tooltip_.transform.GetChild(1).GetComponent<TMP_Text>().text = ability.Description;
                tooltip_.transform.GetChild(2).GetComponent<TMP_Text>().text = 
                ability.Damage.ToString()+"/"+ability.ManaCost.ToString()+"/"+ability.CoolDown.ToString();
            }
        }
    }

    public void OnPointerEnter(PointerEventData e)
    {
        updates = true;
        tooltip_.SetActive(true);
    }

    public void OnPointerExit(PointerEventData e)
    {
        updates = false;
        tooltip_.SetActive(false);
    }
}