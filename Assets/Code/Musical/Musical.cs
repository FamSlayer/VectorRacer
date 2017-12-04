using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musical : MonoBehaviour
{

    public enum state
    {
        paused,
        increasing,
        decreasing,
    }
    public state my_state;

    [HideInInspector]
    public float increase_rate;
    [HideInInspector]
    public float decrease_rate;
    [HideInInspector]
    public float bpsecond;

    public virtual void Awake()
    {
        bpsecond = TimeKeeper.global.bpm / 60f;
        my_state = state.decreasing;
    }

    public virtual void Start ()
    {
        ObjectKeeper.global.All_Objects.Add(this);
	}

    public virtual void Snap() { my_state = state.decreasing; }

    public virtual void Trigger() { my_state = state.increasing; }

    public virtual void Pause() { my_state = state.paused; }
    
}
