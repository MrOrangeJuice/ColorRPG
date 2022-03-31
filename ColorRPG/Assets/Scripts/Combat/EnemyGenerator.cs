using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Round}

[System.Serializable]
public struct EnemyInfo
{
    public EnemyType type;
    public Sprite sprite;
    public int healthMin;
    public int healthMax;
    public float attackMin;
    public float attackMax;
    public float speedMin;
    public float speedMax;

}
public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private List<EnemyInfo> infoList;
    [SerializeField]
    private List<Combat> enemies;
    void Awake()
    {
        enemies = new List<Combat>();
        foreach(Transform child in transform)
        {
           enemies.Add(child.GetComponent<Combat>());
        }
        GenerateEnemies();
    }

    public void GenerateEnemies()
    {
        foreach(Combat c in enemies)
        {
            EnemyInfo data = infoList[Random.Range(0, infoList.Count)]; 
            c.attack = Random.Range(data.attackMin, data.attackMax);
            c.speed = Random.Range(data.speedMin, data.speedMax);
            c.health = Random.Range(data.healthMin, data.healthMax);

            //Set color based on theme regardless of where scene is being run
            Color theme = EquipmentManager.instance != null ? EquipmentManager.instance.currentTheme : Color.blue;
            //Mixing in a smidge of white to lighten the color up a bit
            c.SetColor(ColorMixer.MixColor(theme, ColorMixer.MixColor(ColorMixer.GetRandomColor(), Color.white, .7f,.3f), .25f, .75f));
            c.gameObject.SetActive(true);

            
          
        }
    }

   
}
