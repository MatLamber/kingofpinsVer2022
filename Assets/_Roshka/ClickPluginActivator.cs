using System;
using Tabtale.TTPlugins;
using UnityEngine;

public class ClickPluginActivator : MonoBehaviour
{
    private void Awake()
    {
        TTPCore.Setup();
    }
}
