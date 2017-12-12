using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle

    public int slow_update_frequency;
    int update_counter = 0;

    private Rigidbody my_rb;
    private CarProperties my_props;

    public float curr_speed;
    public int gear = 1;
    public float accel_rate;
    public bool on_road;

    LayerMask road_layer;
    float accel;
    public float h_axis;

    float grav_scale;

    void Awake()
    {
        grav_scale = Physics.gravity.magnitude;
        //base.Awake();
        my_rb = GetComponent<Rigidbody>();
        my_props = GetComponent<CarProperties>();
        
        road_layer = LayerMask.NameToLayer("Road");
        road_layer = ~road_layer;

        SetMotorTorque(0);
        SetBrakeTorque(0);
        SetSteering(0);

    }
    
    void FixedUpdate()
    {

        if (++update_counter % slow_update_frequency == 0)
        {
            SlowUpdate();
        }


        // TURNING
        h_axis = Input.GetAxis("horizontal");
        if (h_axis > 0)
            h_axis *= h_axis * h_axis * h_axis;
        else
            h_axis *= -h_axis * h_axis * h_axis;
        SetSteering(h_axis * my_props.turn_speed);


        SetMotorTorque(0);
        SetBrakeTorque(0);


        curr_speed = my_rb.velocity.magnitude;

        accel = Input.GetAxis("accelerate");
        accel_rate = CalcAccelRate();
        float torque = accel * accel_rate * 1000f * 100f;
        float brake_torque = 5000f;

        //print("accel: " + accel + "   torque: " + torque + "   brake_torque: " + brake_torque);
        if (accel == 0)
        {
            SetMotorTorque(0f);
            SetBrakeTorque(brake_torque);
        }
        else if(accel > 0)
        {
            if(curr_speed < my_props.speed_5)
            {
                SetMotorTorque(torque);
            }
            else
            {
                SetMotorTorque(0f);
            }
            SetBrakeTorque(0f);
        }
        else if(accel < 0)
        {
            if(Vector3.Dot(my_rb.velocity, transform.forward) > 0)
            {
                SetBrakeTorque(brake_torque);
            }
            else
            {
                SetBrakeTorque(0f);
            }
            SetMotorTorque(torque);
        }

        //print(accel * accel_rate);
    }
    
    public void SlowUpdate()
    {
        // raycast at the ground and set on_ground accordingly
        RaycastHit hit;
        Vector3 start_pos = transform.position + transform.up * .25f + transform.forward * .2f;
        Vector3 dir = transform.up * -1f;
        float dist = .7f;
        
        bool on_road = Physics.Raycast(start_pos, dir, out hit, dist, road_layer);
        if (on_road)
        {
            Physics.gravity = -hit.normal * grav_scale * 1.1f;
        }
        else
        {
            //Physics.gravity = Vector3.down * grav_scale;
            //print("good bye sweet prince");
        }
    }
    
    private void SetMotorTorque(float m)
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = m;
                axleInfo.rightWheel.motorTorque = m;
            }
        }
    }

    private void SetBrakeTorque(float b)
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.brakeTorque = b;
                axleInfo.rightWheel.brakeTorque = b;
            }
        }
    }

    private void SetSteering(float s)
    {
        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = s;
                axleInfo.rightWheel.steerAngle = s;
            }
        }
    }


    private float CalcAccelRate()
    {
        float r = my_props.accel_1;
        if (accel > 0)
        {
            if (curr_speed >= my_props.speed_4)
            {
                r = my_props.accel_5;
                gear = 5;
            }
            else if (curr_speed >= my_props.speed_3)
            {
                r = my_props.accel_4;
                gear = 4;
            }
            else if (curr_speed >= my_props.speed_2)
            {
                r = my_props.accel_3;
                gear = 3;
            }
            else if (curr_speed >= my_props.speed_1)
            {
                r = my_props.accel_2;
                gear = 2;
            }
        }
        else if (accel < 0)
        {
            r = my_props.accel_reverse;
        }
        return r;
    }

}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}