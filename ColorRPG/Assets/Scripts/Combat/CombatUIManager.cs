using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CombatUIManager : MonoBehaviour
{
    public Dictionary<Combat, CombatUI> uiCards = new Dictionary<Combat, CombatUI>();
    public GameObject uiCardPrefab;
    [SerializeField]
    private CombatManager manager;

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
}
