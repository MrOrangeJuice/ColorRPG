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
        float[] weights = new float[manager.characters.Count];
        float sum = 0;
        for(int i = 0; i < weights.Length;i++)
        {
            weights[i] = ColorMixer.ColorDistance(combat.color, manager.characters[i].color);   
            sum += weights[i];
        }
        for(int i = 0; i < weights.Length; i++)
        {
            weights[i] /= sum;
        }

        return manager.characters[RandomIndexFromWeights(weights)];
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

    /// <summary>
    ///Gets a random index based on weights from an array
    /// </summary>
    public int RandomIndexFromWeights(float[] weights)
    {
        float totalWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            totalWeight += weights[i];
        }
        float roll = Random.value * totalWeight;
        float num = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            num += weights[i];
            if (num >= roll)
            {
                return i;
            }
        }
        //shouldn't run 
        return 0;
    }

}
