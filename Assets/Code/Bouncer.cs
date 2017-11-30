using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : Musical
{
    public class Bar
    {
        public float top_height     { get; set; }
        public GameObject go        { get; set; }
        public int id               { get; set; }
        public state my_status  = state.decreasing;
        public float curr_height    = 0f;
        public float curr_scale     = 0f;

        public string GetPrintOut()
        {
            return string.Format("id: {0}  max_h: {1}  state: {2}  goal_h: {3}", id, top_height, my_status.ToString(), curr_height);
        }

        public void SnapToMax()
        {
            //go.transform.localPosition = new Vector3(go.transform.localPosition.x, max_height, go.transform.localPosition.z);
            curr_height = top_height;
            my_status = state.decreasing;
        }
    }
    
    public int num_bars;
    public float bar_spacing;
    public float max_height;
    public GameObject bar_prefab;
    
    private List<Bar> my_bars;
    private int current_bar_index = 0;

    public override void Awake()
    {
        base.Awake();

        float height_diff = max_height / num_bars;
        float first_position = -1 * (num_bars - 1) * (bar_spacing / 2f);
        //print("first position: " + first_position);

        my_bars = new List<Bar>();

        //print("Making bars:");
        for(int i = 0; i < num_bars; i++)
        {
            GameObject new_b = Instantiate(bar_prefab, transform);
            float x_pos = first_position + bar_spacing * i;
            new_b.transform.localPosition = new Vector3(x_pos, 0f, 0f);
            //print("\t[i] - localPos:" + new_b.transform.localPosition);

            float my_height = height_diff * (i + 1);

            my_bars.Add(new Bar
            {
                top_height = my_height,
                go = new_b,
                id = i,
            });
        }
        
    }
    
    public override void Start ()
    {
        base.Start();
        ObjectKeeper.global.Bars.Add(this);
    }

	void Update ()
    {
        if (my_bars.Count == 0)
            return;

        foreach(Bar b in my_bars)
        {
            switch(b.my_status)
            {
                case state.paused:
                    break;
                case state.increasing:
                    float raise_speed = 1 + 4 * b.top_height * bpsecond / (ObjectKeeper.global.expand_beats);
                    b.curr_height = Mathf.Lerp(b.curr_height, b.top_height, raise_speed * Time.deltaTime);
                    break;
                case state.decreasing:
                    float lower_speed = 1 + 4 * b.top_height * bpsecond / (ObjectKeeper.global.shrink_beats);
                    b.curr_height = Mathf.Lerp(b.curr_height, 0f, lower_speed * Time.deltaTime);
                    break;
            }

            b.go.transform.localPosition = new Vector3(b.go.transform.localPosition.x, b.curr_height, b.go.transform.localPosition.z);
        }

	}
    
    public override void Snap()
    {
        // snap the bar of the current index to it's max height
        my_bars[current_bar_index].SnapToMax();
        IncrementIndex();
    }

    public override void Trigger()
    {
        my_bars[current_bar_index].my_status = state.increasing;
    }

    private void IncrementIndex()
    {
        current_bar_index++;
        current_bar_index %= num_bars;
    }
}
