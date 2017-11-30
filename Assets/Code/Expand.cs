﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expand : Musical
{
    public float max_scale;
    private float cur_scale;

    public override void Awake ()
    {
        base.Awake();
        increase_rate = 4 * max_scale * bpsecond / (ObjectKeeper.global.expand_beats);// / (TimeKeeper.global.bpm / 60f);
        decrease_rate = 4 * max_scale * bpsecond / (ObjectKeeper.global.shrink_beats);// / (TimeKeeper.global.bpm / 60f);
        //print(expand_speed);
        //print(shrink_speed);

        cur_scale = max_scale;

        transform.localScale = Vector3.one + Vector3.one * cur_scale;

    }
    public override void Start()
    {
        base.Start();
        ObjectKeeper.global.Cubes.Add(this);
    }

    void Update ()
    {
        switch(my_state)
        {
            case state.paused:
                return;
            case state.increasing:
                cur_scale = Mathf.Lerp(cur_scale, max_scale, increase_rate * Time.deltaTime);
                break;
            case state.decreasing:
                cur_scale = Mathf.Lerp(cur_scale, 0f, decrease_rate * Time.deltaTime);
                break;
        }

        transform.localScale = Vector3.one + Vector3.one * cur_scale;
	}

    public override void Snap()
    {
        base.Snap();
        cur_scale = max_scale;
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
            //cur_scale = Mathf.Lerp(cur_scale, 0f, Time.deltaTime * ( max_scale / (1 - pop_percentage) ) );
            cur_scale = Mathf.Lerp(cur_scale, 0f, lerp_speed / 2f);


            // snap to 1
            //
            //if ( (1f - pop_percentage) - ratio < pop_percentage / 10f)
            //{
            //    cur_scale = 0f;
            //    print("snapped to min size");
            //}
            //
            
        }
        // expand
        else
        {
            cur_scale = Mathf.Lerp(cur_scale, max_scale, lerp_speed);
            
            // snap to 1 + max_scale
            //
            //if( 1f - ratio < pop_percentage / 10f)
            //{
            //    cur_scale = max_scale;
            //}
            //
        }
        

        transform.localScale = Vector3.one + Vector3.one * cur_scale;
*/
