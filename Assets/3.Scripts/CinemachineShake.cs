using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{

    /// <summary>
    /// Юуны түрүүнд синемашин виртуал камер байх ёстой түүн дээрээ noise мултичаннелперлинтэй байх
    /// </summary>
    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera virtualCamera;
    private float shakeTimer;

    void Awake()
    {
        Instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = 0f;

            }

        }
       


    }




    #region ShakeCamera
    /// <summary>
    /// Виртуал камераг сэгсрэх
    /// </summary>
    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = 2;
        shakeTimer = .1f;
    }

    /// <summary>
    /// Виртуал камераг сэгсрэх
    /// </summary>
    /// <param name="instensity">хэр хүчтэй чичиргээ өгөх</param>
    public void ShakeCamera(float instensity)
    {
        CinemachineBasicMultiChannelPerlin perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = instensity;
        shakeTimer = .1f;
    }

    /// <summary>
    /// Виртуал камераг сэгсрэх
    /// </summary>
    /// <param name="instensity">хэр хүчтэй чичиргээ өгөх</param>
    /// <param name="time">хэр удаан чичиргэх</param>
    public void ShakeCamera(float instensity, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = instensity;
        shakeTimer = time;
    }

    #endregion


}
