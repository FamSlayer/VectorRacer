using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor;      // do wheels apply motor
        public bool steering;   // do wheels steer
    }

    public int slow_loop_frequency;
    int update_counter = 0;

    public GameObject model_body;
    public bool is_dead = false;

    public List<AxleInfo> axleInfos;

    CarProperties my_props;
    Rigidbody my_rb;

    const float friction = 0.98f;
    const float rot_friction = 0.9f;

    float accel;
    public float h_axis;
    float v_axis;
    public int gear = 1;

    public float curr_speed = 0;
    public float accel_rate = 0;
    Vector3 speed_vector;

    
    public bool on_road = true;
    LayerMask road_layer;
    float grav_scale;
    
    void Awake()
    {
        grav_scale = Physics.gravity.magnitude;

        //base.Awake();
        my_rb = GetComponent<Rigidbody>();
        my_props = GetComponent<CarProperties>();
        road_layer = LayerMask.NameToLayer("Road");
        road_layer = ~road_layer;   // why do you make this suck unity
        print("this is what road_layer is: " + road_layer);

        SetMotorTorque(0f);
        SetSteering(0f);
    }
    
	void Update ()
    {
        //base.Update();

        // RECEIVE INPUT
        if (!my_props.is_off && is_dead == false)
        {
            accel = Input.GetAxis("accelerate");
            h_axis = Input.GetAxis("horizontal");
        }
        // turn the tires
        h_axis = h_axis * h_axis * h_axis * h_axis;
        if (Input.GetAxis("horizontal") < 0)
        {
            h_axis *= -1f;
        }
        
        my_props.l_wheel.transform.localEulerAngles = new Vector3(90f, 0f, h_axis * -20);
        my_props.r_wheel.transform.localEulerAngles = new Vector3(90f, 0f, h_axis * -20);

        if (++update_counter % slow_loop_frequency != 0)
            return;

        SlowUpdate();


    }


    private void FixedUpdate()
    {
        if(!on_road)
        {
            print("NOT ON ROAD, RETURNING OUT OF FIXEDUPDATE()");
            return;
        }
        curr_speed = my_rb.velocity.magnitude;
        float angle = h_axis * my_props.turn_speed;
        SetSteering(angle);
        
        accel = Input.GetAxis("accelerate");
        accel_rate = CalcAccelRate();

        float torque = accel * accel_rate * 200f;
        //torque = accel * my_props.accel_2 * 20f;
        float brake_torque = accel_rate * 1000f;

        if (accel > 0)
        {
            if (my_rb.velocity.magnitude < my_props.speed_4)
            {
                SetMotorTorque(torque);
                SetBrakeTorque(0f);
                //SetSteering(angle);
                print("FORWARD! torque = " + torque);
            }
            else
            {
                SetMotorTorque(0f);
            }
            /*
            if (curr_speed > 0 && Vector3.Dot(my_rb.velocity, transform.forward) < 0f)
            {
                SetBrakeTorque(brake_torque);
                print("we're trying to go forward while going backward");
            }
            */
        }
        else if(accel < 0)
        {
            if(Vector3.Dot(my_rb.velocity, transform.forward) > 0f)
            {
                SetBrakeTorque(brake_torque);
                print("standard regular braking... torque = " + accel_rate * 500f);
            }
            else
            {
                SetMotorTorque(torque);
                SetBrakeTorque(0f);
                //SetSteering(-angle);
                print("reversing... torque = " + torque);
            }
        }
        else
        {
            SetMotorTorque(0f);
            SetBrakeTorque(brake_torque / 5f);
            print("Applying standard 'friction'");
        }



        
        float tipping_angle = curr_speed * my_props.turn_speed * h_axis / -500f;
        model_body.transform.localEulerAngles = new Vector3(tipping_angle, model_body.transform.localEulerAngles.y, model_body.transform.localEulerAngles.z);

        

        // apply stabalizing

    }


    public void SlowUpdate()
    {
        // raycast at the ground and set on_ground accordingly
        RaycastHit hit;
        Vector3 start_pos = transform.position + transform.up * .25f + transform.forward * .2f;
        Vector3 dir = transform.up * -1f;
        float dist = .5f;
        //Debug.DrawLine(start_pos, start_pos + dir * dist, Color.green, 1f);
        on_road = Physics.Raycast(start_pos, dir, out hit, dist, road_layer);
        
        //bool res = Physics.Raycast(start_pos, dir, out hit, dist, road_layer);
        if(on_road)
        {
            //print(hit.collider.gameObject + " and " + res.ToString());
            //print(hit.normal);
            //print(transform.up);
            Debug.DrawLine(hit.point, hit.point + hit.normal * 2f, Color.blue, 3f);

            Physics.gravity = -hit.normal * grav_scale;
        }
        else
        {
            Physics.gravity = Vector3.down * grav_scale;
        }

        //on_road = res;

        //transform.up = hit.normal;
        //Physics.gravity = 

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



/*
    // accelerate
    if (accel > 0)
    {
        //print("accelerating... " + accel);
        if (gear == 1)
        {
            speed += accel* my_props.accel_1 * Time.deltaTime;
            //speed *= friction;
            if (speed > my_props.speed_1)
            {
                gear = 2;
            }
        }
        else if (gear == 2)
        {
            speed += accel* my_props.accel_2 * Time.deltaTime;
            //speed *= friction;
            if (speed > my_props.speed_2)
            {
                speed = my_props.speed_2;
            }
            if (speed<my_props.speed_1)
            {
                gear = 1;
            }
            //if (my_rb.velocity.magnitude < 0.2f && speed > 0)
            //{
            //    gear = 1;
            //    speed = 0f;
            //}
        }



    }
    else if (accel< 0)
    {
        gear = 1;
        if (speed > 0)
        {
            speed += accel* my_props.accel_reverse * Time.deltaTime* 2.5f;
            //speed *= friction;
        }
        else
        {
            speed += accel* my_props.accel_reverse * Time.deltaTime;
            //speed *= friction;
        }
        //print("reversing... " + accel + "   : speed - " + speed);
        if (speed<my_props.speed_reverse* -1f)
        {
            speed = my_props.speed_reverse* -1f;
        }
            
        //if (accel > 0 && my_rb.velocity.magnitude < 0.2f && speed > 0)
        //{
        //    speed = 0f;
        //}
    }
    else
    {
        if (accel< 0.1f)
        {
            speed *= friction* friction;
        }

        if(speed< 50f)
        {
            speed = 0f;
        }
    }

    speed_vector.Set(0, 0, speed);
    disp = Mathf.Sqrt(Mathf.Abs(my_rb.velocity.magnitude + speed));

    //print("disp = " + disp + "   speed: " + speed);
        
    //if (accel < 0.1f)
    //{
    //    speed *= friction;
    //}
    if (my_props.is_off)
    {
        speed *= 0.95f;
    }

    float turn_rate = my_props.turn;
    if (speed< 0)
        turn_rate *= -1f;
    if (disp* h_axis != 0)
    {
        if(disp< 55f)
        {
            turn_rate *= 2f;
        }
        my_rb.AddTorque(Vector3.up* turn_rate * disp * h_axis * Time.deltaTime);
    }

    speed_vector = transform.rotation* speed_vector;
my_rb.AddForce(speed_vector* Time.deltaTime);
*/