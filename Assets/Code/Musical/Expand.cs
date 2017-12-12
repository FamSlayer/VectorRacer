using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expand : Musical
{
    public float max_scale;
    public float curr_scale;

    bool update = true;

    public override void Awake ()
    {
        base.Awake();
        increase_rate = 6f * max_scale * bpsecond / (TimeKeeper.global.expand_beats);
        decrease_rate = 12f * max_scale * bpsecond / (TimeKeeper.global.shrink_beats);

    }
    public override void Start()
    {
        base.Start();
        ObjectKeeper.global.Cubes.Add(this);
        //print("My name is expand");
    }

    public virtual void Update ()
    {
        /*
        if(!update)
        {
            update = true;
            return;
        }
        */
        switch(my_state)
        {
            case state.paused:
                return;
            case state.increasing:
                curr_scale = Mathf.Lerp(curr_scale, max_scale, increase_rate * Time.deltaTime);
                break;
            case state.decreasing:
                curr_scale = Mathf.Lerp(curr_scale, 0f, decrease_rate * Time.deltaTime);
                break;
        }
        transform.localScale = Vector3.one + Vector3.one * curr_scale;
	}

    public override void Snap()
    {
        base.Snap();
        curr_scale = max_scale;
    }

    public override void Trigger()
    {
        base.Trigger();
    }
    
}









/* OLD UPDATE FUNCTION BEFORE STATE MACHINE
        //period_timer += Time.deltaTime;
        period_timer = TimeKeeper.global.my_time % period;
        float ratio = period_timer / period;

        float lerp_speed = 3 * Time.deltaTime * (max_scale / pop_percentage);


        // shrink
        if (ratio < 1f - pop_percentage)
        {
            //curr_scale = Mathf.Lerp(curr_scale, 0f, Time.deltaTime * ( max_scale / (1 - pop_percentage) ) );
            curr_scale = Mathf.Lerp(curr_scale, 0f, lerp_speed / 2f);


            // snap to 1
            //
            //if ( (1f - pop_percentage) - ratio < pop_percentage / 10f)
            //{
            //    curr_scale = 0f;
            //    print("snapped to min size");
            //}
            //
            
        }
        // expand
        else
        {
            curr_scale = Mathf.Lerp(curr_scale, max_scale, lerp_speed);
            
            // snap to 1 + max_scale
            //
            //if( 1f - ratio < pop_percentage / 10f)
            //{
            //    curr_scale = max_scale;
            //}
            //
        }
        

        transform.localScale = Vector3.one + Vector3.one * curr_scale;
*/
