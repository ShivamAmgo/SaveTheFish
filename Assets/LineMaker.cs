using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMaker : MonoBehaviour
{
    [SerializeField] Transform[] Points;
    [SerializeField] Linerenderer linerenderer;
    private void Start()
    {
        linerenderer.SetupLine(Points);
    }
}
