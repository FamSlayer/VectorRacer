using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOffSkyCamera : SlowLoop
{
    Vector3 screenPoint = Vector3.zero;

    public override void SlowUpdate ()
    {
        screenPoint = Core.global.sky_cam.WorldToViewportPoint(transform.position);
        //bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        
        if(screenPoint.y > 1.1f || screenPoint.x < -.1f || screenPoint.z < 0f)
        {
            //ObjectKeeper.global.Remove(gameObject);
            Destroy(gameObject);
            /*
            deleting = true;
            Expand e = gameObject.GetComponent<Expand>();
            if(e != null)
            {
                Destroy(e);
            }
            Rotator r = gameObject.GetComponent<Rotator>();
            if(r != null)
            {
                Destroy(r);
            }
            */
            //print(screenPoint + "  -- deleting");
        }
        else
        {
            //print(screenPoint + "  -- leaving");
        }
    }
}
