using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class CombatUIManager : MonoBehaviour
{
    public Dictionary<Combat, CombatUI> uiCards = new Dictionary<Combat, CombatUI>();
    public GameObject uiCardPrefab;
    [SerializeField]
    private CombatManager manager;
    [SerializeField]
    private TextMeshProUGUI damageText;
    
    public ColorPicker picker;

    public GameObject caveBackground;
    public GameObject forestBackground;
    
    public void AddUICard(Combat c)
    {
        
        CombatUI ui = Instantiate(uiCardPrefab, transform).GetComponent<CombatUI>();
        uiCards.Add(c, ui);
        ui.nameText.text = c.name;
        ui.healthText.text = c.health.ToString();
        ui.attackButton.onClick.AddListener(delegate { manager.BeginAttackSelection(c.transform); });

    }
    public void SetTurnUI(Combat up)
    {
        foreach(CombatUI ui in uiCards.Values)
        {
            if(uiCards.ContainsKey(up) && uiCards[up] == ui)
            {
                ui.actionPanel.SetActive(true);
            }
            else
            {
                ui.actionPanel.SetActive(false);
            }

        }
        
    }

    public void UpdateHealth(Combat c)
    {
        uiCards[c].healthText.text = c.health.ToString();  
    }

    public IEnumerator UpdateDamageText(int damage)
    {
        damageText.text = damage + " damage!";
        yield return new WaitForSeconds(.4f);
        damageText.text = "";
    }

    public void EnableColorPicker(Color current, Color mixing)
    {
        picker.MixingColor = mixing;
        picker.CurrentColor = current;
        picker.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void DisableColorPicker()
    {
        picker.SelectedColor = Color.white;
        picker.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Start()
    {
        if (EquipmentManager.instance != null)
        {
            GameObject background = EquipmentManager.instance.currentTheme == Color.green ? forestBackground : caveBackground;
            background.gameObject.SetActive(true);  
        }
    }
}
