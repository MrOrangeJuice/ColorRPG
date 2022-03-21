using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    [SerializeField]
    private GameObject panelPrefab;
    [SerializeField]
    private int numPanels;
    [SerializeField]
    private float minStrength;
    [SerializeField]
    private float maxStrength;

    private Transform panelParent;

    List<Image> panels;

    private Color CurrentColor { get; set; }
    private Color MixingColor { get; set; }


    public Color current;
    public Color mix;
    private void OnEnable()
    {
        CurrentColor = current;
        MixingColor = mix;
        if(panelParent == null)
        {
            panelParent = transform.GetChild(0);
        }
        //Make the panels if they do not exist
        if (panelParent.childCount != numPanels)
        {
            panels = new List<Image>();
            for (int i = 0; i < numPanels; i++)
            {
                panels.Add(Instantiate(panelPrefab, panelParent).GetComponent<Image>());   
            }
        }

        for(int i = 0; i < numPanels; i++)
        {
            int middle = numPanels / 2;
            float step = (maxStrength - minStrength) / middle;
            float strength = i <= middle ? i * step + minStrength : maxStrength - step * (i - middle) ;
            panels[i].color = ColorMixer.MixColor(CurrentColor, MixingColor, 1 - strength, strength);
        }


    }
}
