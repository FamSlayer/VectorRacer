using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOffSkyCamera : SlowLoop
{
    Vector3 screenPoint = Vector3.zero;

    public override void SlowUpdate ()
    {
        screenPoint = Core.global.sky_cam.WorldToViewportPoint(transform.position);
        if(screenPoint.y > 1.1f || screenPoint.x < -.1f || screenPoint.z < 0f)
        {
            DestroyImmediate(gameObject);
        }
    }
}
