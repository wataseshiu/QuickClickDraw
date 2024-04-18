using System;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace VirtualCameraDirector
{
    public class VirtualCameraDirector: SingletonMonoBehaviour<VirtualCameraDirector>
    {
        [SerializeField]CinemachineVirtualCamera[] virtualCameras;
        
        public void SetActiveVirtualCamera(string virtualCameraName)
        {
            virtualCameras.First(virtualCamera => virtualCamera.Name == virtualCameraName)
                .Priority = 999;
        }
    }
}