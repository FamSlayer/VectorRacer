using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : Musical
{
    public float min_random_speed;
    public float max_random_speed;
    public Vector3 super_rot_axis;

    [HideInInspector]
    public float maximum_speed;
    [HideInInspector]
    public float base_speed;
    
    private float x, y, z;
    private Vector3 rot_axis;
    private float curr_speed;

    private float rot_increase_rate;
    private float rot_decrease_rate;

    bool update = true;

    public override void Awake()
    {
        base.Awake();
        transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        
        //max_scale = .25f;

        x = Random.Range(-4f, 4f);
        y = Random.Range(-30, -10f);
        z = Random.Range(-8, 8f);
        rot_axis = new Vector3(x, y, z);
        
        base_speed = Random.Range(min_random_speed, max_random_speed);
        maximum_speed = 40f * base_speed;
        curr_speed = base_speed;
        
        increase_rate = 2 * (maximum_speed - base_speed) * bpsecond / (TimeKeeper.global.expand_beats);
        decrease_rate = .25f * (maximum_speed - base_speed) * bpsecond / (TimeKeeper.global.shrink_beats);

        //rot_increase_rate = 2 * (maximum_speed - base_speed) * bpsecond / (TimeKeeper.global.expand_beats);
        //rot_decrease_rate = .25f * (maximum_speed - base_speed) * bpsecond / (TimeKeeper.global.shrink_beats);

    }

    public override void Start()
    {
        base.Start();
        /*base.base.Start();
        ObjectKeeper.global.All_Objects.Add(this);*/
        ObjectKeeper.global.Rotators.Add(this);
    }


    void Update()
    {
        /*
        if (!update)
        {
            update = true;
            return;
        }
        */
        switch (my_state)
        {
            case state.paused:
                return;
            case state.increasing:
                curr_speed = Mathf.Lerp(curr_speed, maximum_speed, increase_rate * Time.deltaTime);
                //curr_speed = Mathf.Lerp(curr_speed, maximum_speed, rot_increase_rate * Time.deltaTime);
                transform.Rotate(super_rot_axis, curr_speed * Time.deltaTime);
                break;
            case state.decreasing:
                curr_speed = Mathf.Lerp(curr_speed, base_speed, decrease_rate * Time.deltaTime);
                //curr_speed = Mathf.Lerp(curr_speed, base_speed, rot_decrease_rate * Time.deltaTime);
                transform.Rotate(rot_axis, curr_speed * Time.deltaTime);
                break;

        }
    }
    
    /*
    public override void Update ()
    {
        base.Update();
        
        switch(my_state)
        {
            case state.paused:
                return;
            case state.increasing:
                curr_speed = Mathf.Lerp(curr_speed, maximum_speed, rot_increase_rate * Time.deltaTime);
                transform.Rotate(super_rot_axis, curr_speed * Time.deltaTime);
                break;
            case state.decreasing:
                curr_speed = Mathf.Lerp(curr_speed, base_speed, rot_decrease_rate * Time.deltaTime);
                transform.Rotate(rot_axis, curr_speed * Time.deltaTime);
                break;
                
        }
    }
    */

    public override void Snap()
    {
        base.Snap();
        curr_speed = maximum_speed;
    }


}
