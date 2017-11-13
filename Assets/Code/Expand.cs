using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expand : MonoBehaviour
{

    public float scaling;
    public float period;
    [Range(0f,.5f)]
    public float pop_percentage;
    


    private float period_timer = 0f;
    private float cur_scale;

    void Awake ()
    {
        cur_scale = 1 + scaling;

        transform.localScale = Vector3.one + Vector3.one * cur_scale;
    }

    void Update ()
    {
        //period_timer += Time.deltaTime;
        period_timer = TimeKeeper.global.my_time % period;
        float ratio = period_timer / period;

        float lerp_speed = Time.deltaTime / period * (scaling / pop_percentage);

        /* shrink */
        if (ratio < 1f - pop_percentage)
        {
            //cur_scale = Mathf.Lerp(cur_scale, 0f, Time.deltaTime * ( scaling / (1 - pop_percentage) ) );
            cur_scale = Mathf.Lerp(cur_scale, 0f, lerp_speed / 2f);


            /* snap to 1 */
            /*
            if ( (1f - pop_percentage) - ratio < pop_percentage / 10f)
            {
                cur_scale = 0f;
                print("snapped to min size");
            }
            */
            
        }
        /* expand */
        else
        {
            cur_scale = Mathf.Lerp(cur_scale, scaling, lerp_speed);
            
            /* snap to 1 + scaling */
            /*
            if( 1f - ratio < pop_percentage / 10f)
            {
                cur_scale = scaling;
            }
            */
        }
        

        transform.localScale = Vector3.one + Vector3.one * cur_scale;

	}
}
