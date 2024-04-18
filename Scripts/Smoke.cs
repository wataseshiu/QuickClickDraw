using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Substance.Game;
public class Smoke : MonoBehaviour
{
    private float _smokeValue = 0.0f;
    [SerializeField] private float smokeSpeed = 0.01f;
    public SubstanceGraph smokeGraph;

    // Update is called once per frame
    private void Update()
    {
        _smokeValue += smokeSpeed;
        smokeGraph.SetInputFloat("disorder", _smokeValue);
        smokeGraph.QueueForRender();
        smokeGraph.RenderAsync();
    }
}
