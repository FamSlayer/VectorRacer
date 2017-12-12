using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Singleton<Core>
{
    public CarMovement player_car;
    public Camera player_cam;
    public Camera sky_cam;
    //public AudioSource audio;
    public DSPTime beatKeeper;

    public Strummer top_strummer;
    public Strummer bot_strummer;

    bool started = false;

    private void Update()
    {
        if (started)
            return;
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            beatKeeper.BeginPlay();
            started = true;
        }

        float ACK = Input.GetAxis("accelerate");
        if (ACK > 0)
        {
            started = true;
            beatKeeper.BeginPlay();
        }
        
    }
}
