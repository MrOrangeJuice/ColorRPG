using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SelectionLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public bool active = false;
    public void Activate(Vector3 endPoint, Color color)
    {
        if(lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.startColor = color;    
        lineRenderer.endColor = color;
        lineRenderer.SetPosition(0, endPoint);
        active = true;  
    }

    public void Deactivate(Vector3 endPoint)
    {
        active = false;
        lineRenderer.SetPosition(1, endPoint);
    }

    public void Update()
    {
        if(active)
        {
            lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
