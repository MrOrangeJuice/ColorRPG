using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Combat : MonoBehaviour
{
    public Color color;
    public int health;
    public float speed;
    public string name = "Ben";
    public float attack;   
    private CombatManager manager;
 

    private void Start()
    {

        GetComponent<SpriteRenderer>().color = color;
        manager = GameObject.Find("CombatManager").GetComponent<CombatManager>();   
    }

    public void OnMouseDown()
    {
        if(manager.SelectedCharacter != null)
        {
            manager.EndAttackSelection(this);
        }
        else if (CompareTag("Swatch"))
        {
            manager.BeginAttackSelection(transform);
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    public void SetColor(Color c)
    {
        color = c;
        GetComponent<SpriteRenderer>().color = color;
    }
}
