using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenManager : MonoBehaviour
{
    public static FullscreenManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = true;
    }
}
