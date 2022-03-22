using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField]
    private float multiplierMin = .5f;
    public Combat SelectedCharacter { get; set; }
    [SerializeField]
    private GameObject linePrefab;
    private SelectionLine currentLine;

    [SerializeField]
    private CombatUIManager uiManager;

    private List<Combat> turnOrder;
    private int turn;

    public List<Combat> enemies;
    public List<Combat> characters;
    public List<Combat> swatches;

    private Combat currentTurn;

    public SelectionLine CurrentLine
    {
        get { return currentLine; }
    }
    public void BeginAttackSelection(Transform attackerTransform)
    {
        SelectedCharacter = attackerTransform.GetComponent<Combat>();
        SelectionLine line = Instantiate(linePrefab).GetComponent<SelectionLine>();
        line.transform.position = Vector3.zero;
        line.Activate(attackerTransform.position, SelectedCharacter.color);
        currentLine = line;
    }

    public float ComputeMultiplier(Color color , Color other)
    {
        float sqrDist = Mathf.Pow(color.r - other.r, 2) + Mathf.Pow(color.g - other.g, 2) + Mathf.Pow(color.b - other.b, 2);
        sqrDist /= 3;
        Debug.Log(1 - sqrDist);
        return Mathf.Clamp(1 - sqrDist, multiplierMin, 1);
    }
    

    public void EndAttackSelection(Combat defender)
    {
        if (!defender.CompareTag("Swatch"))
        {
            Attack(SelectedCharacter, defender);
        }
        else
        {
            StoreInSwatch(SelectedCharacter, defender);
            SelectedCharacter = null;
            Destroy(currentLine.gameObject);
            currentLine = null;
        }
    }

    public void Attack(Combat attacker, Combat defender)
    {
        defender.health -= (int)(attacker.attack * ComputeMultiplier(attacker.color, defender.color));
        if (defender.health <= 0)
        {
            defender.health = 0;
            defender.Die();
            turnOrder.Remove(defender);
            if(!defender.CompareTag("Enemy"))
            {
                characters.Remove(defender);
            }
            else
            {
                enemies.Remove(defender);
            }
        }

        if (!defender.CompareTag("Enemy"))
        {
            uiManager.UpdateHealth(defender);
        }
        if(attacker.CompareTag("Swatch"))
        {
            attacker.SetColor(Color.white);
            attacker.attack = 0;
        }

      
        EndTurn();
    }

    public void SetAttackOrder()
    {
        turnOrder.Clear();  
        foreach(Combat c in enemies)
        {
            if(c.gameObject.activeSelf)
            {
                turnOrder.Add(c);
            }
        }
        foreach (Combat c in characters)
        {
            if (c.gameObject.activeSelf)
            {
                turnOrder.Add(c);
            }
        }

        turnOrder.Sort((p, q) =>  q.speed.CompareTo(p.speed));
    }

    public void Start()
    {
        turn = 0;
        turnOrder = new List<Combat>();

        //Set the lists
        foreach(Transform child in transform.Find("Swatches"))
        {
            swatches.Add(child.GetComponent<Combat>());
        }
        foreach(Transform child in transform.Find("Enemies"))
        {
            enemies.Add(child.GetComponent<Combat>());
        }
        foreach(Transform child in transform.Find("Characters"))
        {
            characters.Add(child.GetComponent<Combat>());
            uiManager.AddUICard(child.GetComponent<Combat>());
        }
        SetAttackOrder();
        BeginTurn();
    }

    public void BeginTurn()
    {
        currentTurn = turnOrder[turn];
        uiManager.SetTurnUI(turnOrder[turn]);
        Vector3 offset = turnOrder[turn].CompareTag("Enemy") ? new Vector3(0,-.5f,0) : new Vector3(0, .5f, 0);
        currentTurn.transform.position += offset;
        if(currentTurn.CompareTag("Enemy"))
        {
            StartCoroutine(currentTurn.GetComponent<Enemy>().TakeTurn());
        }
    }

    public void EndTurn()
    {

        if(enemies.Count == 0)
        {
            Debug.Log("Player wins");
            return;
        }
        else if(characters.Count == 0)
        {
            Debug.Log("Player Loses");
            return;
        }
        if (CurrentLine != null)
        {
            Destroy(currentLine.gameObject);
            currentLine = null;
        }
        Vector3 offset = currentTurn.CompareTag("Enemy") ? new Vector3(0, .5f, 0) : new Vector3(0, -.5f, 0);
        currentTurn.transform.position += offset;

        turn++;
        if(turn >= turnOrder.Count )
        {
            StartRound();
        }
        else
        {
            BeginTurn();
        }

    }

    public void StartRound()
    {
        turn = 0;
        SetAttackOrder();
        BeginTurn();
    }

    public void StoreInSwatch(Combat attacker, Combat swatch)
    {
        //Open swatch
        if (swatch.attack == 0)
        {
            swatch.attack = attacker.attack;
            swatch.SetColor(attacker.color);
        }
        else
        {
            swatch.attack += attacker.attack;
            swatch.SetColor(ColorMixer.MixColor(swatch.color, attacker.color, .5f, .5f));
        }
        currentLine.Deactivate(swatch.transform.position);
    }

}