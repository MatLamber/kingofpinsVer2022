using System;
using Cinemachine;
using MoreMountains.Tools;
using UnityEngine;

public class VirtualCameraManagers : MonoBehaviour, MMEventListener<TravelingPhaseStarted>, MMEventListener<TravelingPhaseEnded>, MMEventListener<PuzzlePhaseStarted>, MMEventListener<PuzzlePhaseEnded> // < add your events here>
{
    [SerializeField] private CinemachineVirtualCamera topPartCamera;
    [SerializeField] private CinemachineVirtualCamera bottomPartCamera;
    [SerializeField] private ParallaxBackground_0 parallax;

    private void Start()
    {

    }


    public void EnableParallaxMovement()
    {
        parallax.Camera_Move = true;
    }

    public void DisableParallaxMovement()
    {
        parallax.Camera_Move = false;
    }

    public void SetTopCameraAsMain()
    {
        topPartCamera.Priority = 1;
        bottomPartCamera.Priority = 0;
    }


    public void SetBottomCameraAsMain()
    {
        topPartCamera.Priority = 0;
        bottomPartCamera.Priority = 1;
    }

    public void OnMMEvent(TravelingPhaseStarted eventType)
    {
        SetBottomCameraAsMain();
        EnableParallaxMovement();
    }
    
    
    public void OnMMEvent(TravelingPhaseEnded eventType)
    {
        DisableParallaxMovement();
    }
    
    
    public void OnMMEvent(PuzzlePhaseStarted eventType)
    {
        SetTopCameraAsMain();
    }
    
    public void OnMMEvent(PuzzlePhaseEnded eventType)
    {
        SetBottomCameraAsMain();
    }

    private void OnEnable()
    {
        this.MMEventStartListening<TravelingPhaseStarted>();
        this.MMEventStartListening<TravelingPhaseEnded>();
        this.MMEventStartListening<PuzzlePhaseStarted>();
        this.MMEventStartListening<PuzzlePhaseEnded>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<TravelingPhaseStarted>();
        this.MMEventStopListening<TravelingPhaseEnded>();
        this.MMEventStopListening<PuzzlePhaseStarted>();
        this.MMEventStopListening<PuzzlePhaseEnded>();
    }



}

internal class CinemachineCamera
{
}
