using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slitherer : Musical
{

    public float forward_ms;
    public float min_movespeed;
    public float max_movespeed;

    private float curr_horizontal_speed;
    private float curr_vertical_speed;

    private bool slide_right = true;

    public override void Awake()
    {
        base.Awake();

        curr_horizontal_speed = min_movespeed;
        curr_vertical_speed = min_movespeed;

        increase_rate = 4 * (max_movespeed - min_movespeed) * bpsecond / (TimeKeeper.global.expand_beats);
        decrease_rate = 4 * (max_movespeed - min_movespeed) * bpsecond / (TimeKeeper.global.shrink_beats);

    }

    public override void Start()
    {
        base.Start();
        ObjectKeeper.global.Slitherers.Add(this);
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 move_vector;
        switch(my_state)
        {
            case state.paused:
                return;
            case state.increasing:
                if(slide_right)
                {
                    curr_horizontal_speed = Mathf.Lerp(curr_horizontal_speed, max_movespeed, increase_rate * Time.deltaTime);
                    curr_vertical_speed = Mathf.Lerp(curr_vertical_speed, min_movespeed, decrease_rate * Time.deltaTime);
                }
                else
                {
                    curr_vertical_speed = Mathf.Lerp(curr_vertical_speed, max_movespeed, increase_rate * Time.deltaTime);
                    curr_horizontal_speed = Mathf.Lerp(curr_horizontal_speed, min_movespeed, decrease_rate * Time.deltaTime);
                }
                break;
            case state.decreasing:
                curr_horizontal_speed = Mathf.Lerp(curr_horizontal_speed, min_movespeed, decrease_rate * Time.deltaTime);
                curr_vertical_speed = Mathf.Lerp(curr_vertical_speed, min_movespeed, decrease_rate * Time.deltaTime);
                break;
        }
        
        move_vector = new Vector3(curr_horizontal_speed, -curr_vertical_speed, forward_ms);
        transform.position += move_vector * Time.deltaTime;
	}


    public override void Snap()
    {
        base.Snap();
        slide_right = !slide_right;
    }

    /*
    public override void Trigger()
    {
        base.Trigger();
    }
    */

}
