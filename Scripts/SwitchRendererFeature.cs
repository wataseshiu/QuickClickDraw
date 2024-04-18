using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SwitchRendererFeature : SingletonMonoBehaviour<SwitchRendererFeature>
{
    [SerializeField] private ScriptableRendererFeature fullScreenPassRendererFeature;

    public void SwitchFullScreenRendererFeatureOn()
    {
        fullScreenPassRendererFeature.SetActive(true);
    }
    public void SwitchFullScreenRendererFeatureOff()
    {
        fullScreenPassRendererFeature.SetActive(false);
    }
}
