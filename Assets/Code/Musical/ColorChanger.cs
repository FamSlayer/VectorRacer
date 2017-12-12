using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : Musical
{
    public List<Color> Colors;
    private int colors_index = 0;

    public Color curr_color;

    public override void Awake()
    {
        base.Awake();

        curr_color = Colors[colors_index];

        increase_rate = .25f * bpsecond / (TimeKeeper.global.expand_beats);
        decrease_rate = 2f * bpsecond / (TimeKeeper.global.shrink_beats);        // this isn't used! (should it be though..?)
    }
    
    public virtual void Update ()
    {
        /*
		switch(my_state)
        {
            case state.paused:
                return;
            case state.increasing:
                //curr_color = Color.Lerp(curr_color, Colors[colors_index], increase_rate * Time.deltaTime);
                break;
            case state.decreasing:

                break;
        }
        */
	}

    public override void Snap()
    {
        base.Snap();
        curr_color = Colors[colors_index];
    }

    public override void Trigger()
    {
        base.Trigger();
        colors_index = (colors_index + 1) % Colors.Count;
    }

}
