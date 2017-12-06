using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarProperties : MonoBehaviour
{
    public GameObject l_wheel;
    public GameObject r_wheel;

    public bool is_off = true;

    //top speed at 1st gear
    public float speed_1;
    //top speed at 2nd gear
    public float speed_2;
    //top speed in reverse
    public float speed_reverse;

    //acceleration rate at 1st gear
    public float accel_1;
    //acceleration rate at 2nd gear
    public float accel_2;
    //acceleration in reverse
    public float accel_reverse;

    //turning speed 
    public float turn;
}
