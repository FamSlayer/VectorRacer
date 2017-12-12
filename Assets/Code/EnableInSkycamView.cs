using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInSkycamView : SlowLoop
{
    Vector3 screenPoint;
    
    Expand my_expand;
    Rotator my_rotator;
    
    public override void Awake()
    {
        my_expand = GetComponent<Expand>();
        my_rotator = GetComponent<Rotator>();
        base.Awake();
    }

    public override void SlowUpdate()
    {
        screenPoint = Core.global.sky_cam.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0f && screenPoint.x < 1.05f && screenPoint.y > -.05f && screenPoint.y < 1.05f;
        my_expand.enabled = my_rotator.enabled = onScreen;
        //my_expand.enabled = onScreen;
        //my_rotator.enabled = onScreen;
    }
}
