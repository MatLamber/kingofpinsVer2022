using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Splash : MonoBehaviour
{
    [SerializeField] private float transitionDelay = 1.5f;
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider.DOValue(1, transitionDelay).OnComplete(() => LoadMainMenu());
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
