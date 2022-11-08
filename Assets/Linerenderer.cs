using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linerenderer : MonoBehaviour
{
    LineRenderer lr;
    Transform[] ConnectingPoints;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    public void SetupLine(Transform[] Points)
    {
        
        lr.positionCount = Points.Length;
        this.ConnectingPoints = Points;

    }
    private void Update()
    {
        if (ConnectingPoints[0] == null) 
        {
            lr.gameObject.SetActive(false);
            return;
        } 
            for (int i = 0; i < ConnectingPoints.Length; i++)
            {
                lr.SetPosition(i, ConnectingPoints[i].position);
            }
        
    }
}
