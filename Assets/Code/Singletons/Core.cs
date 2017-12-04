using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Singleton<Core>
{
    public Camera player_cam;
    public Camera sky_cam;
    //public AudioSource audio;
    public DSPTime beatKeeper;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            beatKeeper.BeginPlay();
        }
    }
}
