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

    public override void Awake()
    {
        base.Awake();
        curr_t = 0;
        lr = GetComponent<LineRenderer>();
        PlotPoints(curr_t);

        increase_rate = 4 * max_scale * bpsecond / (TimeKeeper.global.expand_beats);
        decrease_rate = 4 * max_scale * bpsecond / (TimeKeeper.global.shrink_beats);

        curr_scale = 0;
    }

    public override void Start()
    {
        base.Start();
        ObjectKeeper.global.Strummers.Add(this);
    }

	void Update ()
    {
        switch(my_state)
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
        List<Vector3> points = new List<Vector3>();
        float t = t_start;
        while(t <= distance + t_start)
        {
            if(t > 0)
            {
                float y = curr_scale / 2f * Formula(t);

                //print(new Vector3(curr_t, y, 0f));
                points.Add(new Vector3(t - t_start, y, 0));
            }
            t += dt;
        }

        Vector3[] lr_pts = points.ToArray();
        lr.positionCount = lr_pts.Length;
        lr.SetPositions(lr_pts);

    }

    private float Formula(float x)
    {
        x += starting_x;
        return Mathf.Sin(Mathf.PI * x);// / (Mathf.Pow(x, 1.5f));
    }
}
