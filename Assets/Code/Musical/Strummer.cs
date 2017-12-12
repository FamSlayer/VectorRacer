using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Strummer : Musical
{
    public float max_scale;
    [Range(0.001f, 0.25f)]
    public float dt;
    public float progression_rate;
    public float distance;

    [Range(0f, 2f)]
    public float starting_x;

    private float curr_scale;
    private float curr_t;
    LineRenderer lr;
    public Vector3[] my_pts;

    public override void Awake()
    {
        base.Awake();
        ///print("dist / dt = " + distance / dt);
        
        lr = GetComponent<LineRenderer>();

        if (this != Core.global.top_strummer && this != Core.global.bot_strummer)
            return;

        int size = (int)(distance / dt) + 2;
        my_pts = new Vector3[size];
        for (int i = 0; i < my_pts.Length; i++)
        {
            my_pts[i] = new Vector3(0f, 0f, 0f);
        }
        //print("size = " + size);
        curr_t = 0;
        PlotPoints(curr_t);

        increase_rate = 4 * max_scale * bpsecond / (TimeKeeper.global.expand_beats);
        decrease_rate = 6f * max_scale * bpsecond / (TimeKeeper.global.shrink_beats);

        curr_scale = 0;

    }

    public override void Start()
    {
        base.Start();
        ObjectKeeper.global.Strummers.Add(this);
    }

	void Update ()
    {
        if (this != Core.global.top_strummer && this != Core.global.bot_strummer)
        {
            if(starting_x == Core.global.top_strummer.starting_x)
            {
                lr.positionCount = Core.global.top_strummer.lr.positionCount;
                lr.SetPositions(Core.global.top_strummer.my_pts);
            }
            else if(starting_x == Core.global.bot_strummer.starting_x)
            {
                lr.positionCount = Core.global.bot_strummer.lr.positionCount;
                lr.SetPositions(Core.global.bot_strummer.my_pts);
            }
            return;
        }

        switch (my_state)
        {
            case state.paused:
                return;
            case state.increasing:
                curr_scale = Mathf.Lerp(curr_scale, max_scale, increase_rate * Time.deltaTime);
                break;
            case state.decreasing:
                curr_scale = Mathf.Lerp(curr_scale, 0f, decrease_rate * Time.deltaTime);// * curr_scale);
                break;
        }
        PlotPoints(curr_t);
        //curr_t += (Core.global.player_car.curr_speed * 1.25f) * Time.deltaTime;
        curr_t += progression_rate * Time.deltaTime;
        
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


    private void PlotPoints(float t_start)
    {
        //List<Vector3> points = new List<Vector3>();
        
        float t = t_start;
        int index = 0;
        while(t <= distance + t_start)
        {
            if(t > 0)
            {
                float y = curr_scale / 2f * Formula(t);
                //print(new Vector3(curr_t, y, 0f));
                //points.Add(new Vector3(t - t_start, y, 0));
                my_pts[index].x = t - t_start;//
                my_pts[index].y = y;
                my_pts[index].z = 0;//= new Vector3(t - t_start, y, 0);
                index++;
            }
            t += dt;
        }

        //Vector3[] lr_pts = points.ToArray();
        lr.positionCount = my_pts.Length;
        lr.SetPositions(my_pts);

    }

    private float Formula(float x)
    {
        x += starting_x;
        return Mathf.Sin(Mathf.PI * x);// / (Mathf.Pow(x, 1.5f));
    }
}
