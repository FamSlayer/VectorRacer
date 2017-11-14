using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeKeeper : Singleton<TimeKeeper> {

    [HideInInspector]
    public float my_time = 0f;

	void Update ()
    {
        my_time += Time.deltaTime;
	}
}
