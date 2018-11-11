using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuAuidoController : MonoBehaviour
{


    public AudioLowPassFilter darkThemeLowPassFilter;
    public AudioLowPassFilter travellerThemeLowPassFilter;

    public void lowPassOn()
    {
        darkThemeLowPassFilter.enabled = true;
        travellerThemeLowPassFilter.enabled = true;
    }

    public void lowPassOff()
    {
        darkThemeLowPassFilter.enabled = false;
        travellerThemeLowPassFilter.enabled = false;
    }
}
