using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSwitcher : Singleton<CameraSwitcher>
{
    [SerializeField]
    private CinemachineVirtualCamera[] _cameras;

    public void ChangeCamera(int x)
    {
        foreach (var t in _cameras)
        {
            t.Priority = 1;
        }
        
        if(x>_cameras.Length) return;

        _cameras[x].Priority = 10;
    }
}
