using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChanger : ColorChanger
{

    Light myLight;

	public override void Start ()
    {
        base.Start();
        ObjectKeeper.global.ColorSwitchers.Add(this);
        myLight = GetComponent<Light>();
    }
	

	public override void Update ()
    {
        base.Update();
        myLight.color = curr_color;
	}
}
