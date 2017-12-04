using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowLoop : MonoBehaviour
{
    public int slow_loop_frequency = 60;
    int update_counter = 0;

    public virtual void Awake()
    {
        update_counter = Random.Range(0, slow_loop_frequency);
        SlowUpdate();
    }

	void Update ()
    {
        update_counter++;
        if (update_counter < slow_loop_frequency)
            return;

        SlowUpdate();
        update_counter = 0;
    }


    public virtual void SlowUpdate()
    {

    }
}
