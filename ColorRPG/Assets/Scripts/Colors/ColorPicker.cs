using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    [SerializeField]
    private RectTransform selector;

    List<Image> panels;

    private Sequence pickSequence;

    public Color CurrentColor { get; set; }
    public Color MixingColor { get; set; }
    public Color SelectedColor { get; set; }


    
    private void OnEnable()
    {
        SelectedColor = Color.white;    
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
            panels[i].color = ColorMixer.MixColor(CurrentColor, MixingColor, strength, 1-strength);
        }

        //DOTween.To(() => selector.anchorMin, x => selector.anchorMin = x, new Vector2(0, selector.anchorMin.y), 1);
        pickSequence = DOTween.Sequence();
        pickSequence.Append(selector.DOAnchorMin(new Vector2(0, selector.anchorMin.y), 1).SetEase(Ease.Unset));
        pickSequence.Join(selector.DOAnchorMax(new Vector2(.02f, selector.anchorMax.y), 1).SetEase(Ease.Unset)); 
        pickSequence.Append(selector.DOAnchorMin(new Vector2(.98f, selector.anchorMin.y), 1).SetEase(Ease.Unset));
        pickSequence.Join(selector.DOAnchorMax(new Vector2(1.0f, selector.anchorMax.y), 1).SetEase(Ease.Unset));
        pickSequence.SetLoops(-1);
        pickSequence.Loops();


    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && pickSequence.IsActive())
        {
            StartCoroutine(MakeSelection());    
        }
        
    }

    private IEnumerator MakeSelection()
    {
        pickSequence.Kill();
        float middle = (selector.anchorMax.x - selector.anchorMin.x) / 2.0f + selector.anchorMin.x;
        float step = 1.0f / numPanels;
        int index = (int)(middle / step);
        yield return new WaitForSeconds(.2f);
        SelectedColor = panels[index].color;
    }

}
