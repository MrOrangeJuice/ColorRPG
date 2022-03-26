using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private CombatManager manager;
    private Combat combat;
    //TODO: Smarter target selection
    public Combat PickTarget()
    {
        return manager.characters[(int)(Random.value * manager.characters.Count)];
    }

    private void Start()
    {
        combat = GetComponent<Combat>();    
    }

    public IEnumerator TakeTurn()
    {
        yield return new WaitForSeconds(.4f);
        Combat target = PickTarget();
        //Draws a line to the attack target. Little janky, but don't worry about it
        manager.BeginAttackSelection(transform);
        manager.CurrentLine.Deactivate(target.transform.position);
       
        yield return new WaitForSeconds(.5f);
        manager.NoWaitAttack(combat, target);
    }
}
