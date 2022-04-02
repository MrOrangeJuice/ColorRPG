using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public List<Combat> turnOrder;
    private int turn;

    public List<Combat> enemies;
    public List<Combat> characters;
    public List<Combat> swatches;

    private Combat currentTurn;
    [SerializeField]
    private EnemyGenerator enemyGenerator;
    private int rounds;

    private Csv csv;

    public SelectionLine CurrentLine
    {
        get { return currentLine; }
        set
        {
            if(currentLine != null)
            {
                Destroy(CurrentLine.gameObject);
            }
            currentLine = value;
        }
    }
    public void BeginAttackSelection(Transform attackerTransform)
    {
        SelectedCharacter = attackerTransform.GetComponent<Combat>();
        SelectionLine line = Instantiate(linePrefab).GetComponent<SelectionLine>();
        line.transform.position = Vector3.zero;
        line.Activate(attackerTransform.position, SelectedCharacter.color);
        CurrentLine = line;
    }

    public float ComputeMultiplier(Color color , Color other)
    {
        float sqrDist = Mathf.Pow(color.r - other.r, 2) + Mathf.Pow(color.g - other.g, 2) + Mathf.Pow(color.b - other.b, 2);
        sqrDist /= 3;
        float value = Mathf.Clamp(1 - 2 * sqrDist, multiplierMin, 1);
        Debug.Log(value);
        return value;
    }
    

    public void EndAttackSelection(Combat defender)
    {
        if (SelectedCharacter.CompareTag("Swatch"))
        {
            NoWaitAttack(SelectedCharacter, defender);
        }
        else if(!defender.CompareTag("Swatch"))
        {
            StartCoroutine(Attack(SelectedCharacter, defender));
        }
        else
        {
            StartCoroutine(StoreInSwatch(SelectedCharacter, defender));
            SelectedCharacter = null;
            Destroy(currentLine.gameObject);
            currentLine = null;
        }
        
    }

    public IEnumerator Attack(Combat attacker, Combat defender)
    {
     
        if(!attacker.CompareTag("Swatch") && !attacker.CompareTag("Enemy"))
        {
            uiManager.EnableColorPicker(attacker.color, Color.white);
        }

        yield return new WaitUntil(() => uiManager.picker.SelectedColor != Color.white);
        float mult = ComputeMultiplier(attacker.color, defender.color);
        csv.AddRow(csv.Rows.Count.ToString());
        csv.Rows[(csv.Rows.Count - 1).ToString()].Add(ColorMixer.ColorDistance(attacker.color, defender.color));
        csv.Rows[(csv.Rows.Count-1).ToString()].Add(mult);

        int damage = (int)(attacker.attack * mult);
        defender.health -= damage;
        if (defender.health <= 0)
        {
            defender.health = 0;
            defender.Die();
          
            //Make sure we aren't skipping a turn
            Debug.Log("Turn: " + turn + " Index of: " + turnOrder.IndexOf(defender));
            if(turnOrder.IndexOf(defender) < turn)
            {
                turn--;
            }
            turnOrder.Remove(defender);

            if (!defender.CompareTag("Enemy"))
            {
                characters.Remove(defender);
            }
            else
            {
                enemies.Remove(defender);
            }
        }
        StartCoroutine(uiManager.UpdateDamageText(damage));
        if (!defender.CompareTag("Enemy"))
        {
            uiManager.UpdateHealth(defender);
        }
        if(attacker.CompareTag("Swatch"))
        {
            attacker.SetColor(Color.white);
            attacker.attack = 0;
        }

        uiManager.DisableColorPicker();
        EndTurn();
    }

    public void NoWaitAttack(Combat attacker, Combat defender)
    {
        int damage = (int)(attacker.attack * ComputeMultiplier(attacker.color, defender.color));
        defender.health -= damage;
        if (defender.health <= 0)
        {
            defender.health = 0;
            defender.Die();
            turnOrder.Remove(defender);
            if (!defender.CompareTag("Enemy"))
            {
                characters.Remove(defender);
            }
            else
            {
                enemies.Remove(defender);
            }
        }
        StartCoroutine(uiManager.UpdateDamageText(damage));
        if (!defender.CompareTag("Enemy"))
        {
            uiManager.UpdateHealth(defender);
        }
        if (attacker.CompareTag("Swatch"))
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

    public void Awake()
    {
        
        turnOrder = new List<Combat>();
        rounds = 0;

        //Logging
        csv = Logger.Log.CreateCsv("ColorDistance", "Color");
        csv.AddLabels(new List<string>() { " ", "Distance", "Multiplier" });


        //Set the lists
        foreach(Transform child in transform.Find("Swatches"))
        {
            swatches.Add(child.GetComponent<Combat>());
        }
        for (int i = 0; i < transform.Find("Characters").childCount; i++)
        {
            
            Combat c = transform.Find("Characters").GetChild(i).GetComponent<Combat>();
            if (EquipmentManager.instance != null)
            {
                if (EquipmentManager.instance.playerHealth[i] > 0)
                {
                    c.color = EquipmentManager.instance.currentColors[i];
                    c.health = EquipmentManager.instance.playerHealth[i];
                }
                else
                {
                    c.gameObject.SetActive(false);
                }
            }
           
            characters.Add(c);
            uiManager.AddUICard(c);
        }
        BeginFight();
    }

    public void BeginFight()
    {
        if (rounds < 4)
        {
            foreach (Transform child in transform.Find("Enemies"))
            {
                enemies.Add(child.GetComponent<Combat>());
            }
            rounds++;
            turnOrder.Clear();
            turn = 0;
            enemyGenerator.GenerateEnemies();   
            SetAttackOrder();
            BeginTurn();
        }
        else
        {

            for (int i = 0; i < transform.Find("Characters").childCount; i++)
            {
                Combat c = transform.Find("Characters").GetChild(i).GetComponent<Combat>();
                EquipmentManager.instance.playerHealth[i] = c.health;
            }
            SceneManager.LoadScene("DestinyScene");
        }
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

      
        if(characters.Count == 0)
        {
            SceneManager.LoadScene("Loser");
            return;
        }
        if (CurrentLine != null)
        {
            Destroy(currentLine.gameObject);
            currentLine = null;
        }
        Vector3 offset = currentTurn.CompareTag("Enemy") ? new Vector3(0, .5f, 0) : new Vector3(0, -.5f, 0);
        currentTurn.transform.position += offset;
        SelectedCharacter = null;
        turn++;
        if (enemies.Count == 0)
        {
            Debug.Log("Player wins");
            BeginFight();
        }
        else if (turn >= turnOrder.Count )
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

    public IEnumerator StoreInSwatch(Combat attacker, Combat swatch)
    {
        uiManager.EnableColorPicker(attacker.color, swatch.color);
        yield return new WaitUntil(() => uiManager.picker.SelectedColor != Color.white);
        swatch.attack += attacker.attack * .75f;
        swatch.SetColor(uiManager.picker.SelectedColor);
        uiManager.DisableColorPicker();
        uiManager.uiCards[attacker].actionPanel.SetActive(false);

    }

    private void OnDisable()
    {
        Debug.Log("disabled");
        Logger.Log.WriteAllLogFiles();
    }


}
