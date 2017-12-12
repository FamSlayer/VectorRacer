using UnityEngine;

public class CarProperties : MonoBehaviour
{
    public GameObject l_wheel;
    public GameObject r_wheel;


    //public WheelCollider front_right;
    //public WheelCollider front_left;
    //public WheelCollider back_right;
    //public WheelCollider back_left;


    public bool is_off = true;

    // top speed at 1st gear
    public float speed_1;
    // top speed at 2nd gear
    public float speed_2;
    // top speed in 3rd gear
    public float speed_3;
    // top speed in 4th gear
    public float speed_4;
    // top speed in 5th gear
    public float speed_5;
    //top speed in reverse
    public float speed_reverse;

    //acceleration rate at 1st gear
    public float accel_1;
    //acceleration rate at 2nd gear
    public float accel_2;
    //acceleration rate in 3rd gear
    public float accel_3;
    //acceleration rate in 4th gear
    public float accel_4;
    //acceleration rate in 5th gear
    public float accel_5;
    //acceleration in reverse
    public float accel_reverse;

    //turning speed 
    public float turn_speed;
}
