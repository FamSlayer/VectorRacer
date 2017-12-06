using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{

    public bool is_dead = false;


    CarProperties my_props;
    Rigidbody my_rb;

    const float friction = 0.98f;
    const float rot_friction = 0.9f;

    float accel;
    //float brake;
    public float h_axis;
    float v_axis;
    public int gear = 1;
    float brake_timer = 0f;
    float off_timer = -1f;

    float disp;
    public float speed = 0f;
    Vector3 speed_vector;


    Vector3 camera_curr_pos;
    Quaternion camera_curr_rot;


    void Start ()
    {
        my_rb = GetComponent<Rigidbody>();
        my_props = GetComponent<CarProperties>();
	}
	

	void Update ()
    {
        //Debug.DrawLine(transform.position, transform.position + transform.forward * 4, Color.red);
        /*
        if(my_rb.velocity.magnitude <= 0.25f && accel == 0)
        {
            speed /= 2f;// my_rb.velocity.magnitude;
            gear = 1;
        }
        */

        // RECEIVE INPUT
        accel = h_axis = 0f;    //accel = brake = h_axis = 0f;
        if (!my_props.is_off && is_dead == false)
        {
            accel = Input.GetAxis("accelerate");
            //brake = Input.GetAxis("brake");
            h_axis = Input.GetAxis("horizontal");
            //print("accel: " + accel + "   brake : " + brake + "   h_axis: " + h_axis);
        }


        // FUCK WITH H_AXIS AND TURN TIRES
        if (h_axis > 0)
        {
            h_axis = Mathf.Pow(h_axis, 4f);
        }
        else
        {
            h_axis = -Mathf.Pow(h_axis, 4f);
        }

        my_props.l_wheel.transform.localEulerAngles = new Vector3(90f, 0f, h_axis * -20);
        my_props.r_wheel.transform.localEulerAngles = new Vector3(90f, 0f, h_axis * -20);

        //print("speed before if statement... " + speed);

        /* accelerate */
        if (accel > 0)
        {
            //print("accelerating... " + accel);
            if (gear == 1)
            {
                speed += accel * my_props.accel_1 * Time.deltaTime;
                //speed *= friction;
                if (speed > my_props.speed_1)
                {
                    gear = 2;
                }
            }
            else if (gear == 2)
            {
                speed += accel * my_props.accel_2 * Time.deltaTime;
                //speed *= friction;
                if (speed > my_props.speed_2)
                {
                    speed = my_props.speed_2;
                }
                if (speed < my_props.speed_1)
                {
                    gear = 1;
                }
                /*
                if (my_rb.velocity.magnitude < 0.2f && speed > 0)
                {
                    gear = 1;
                    speed = 0f;
                }
                */
            }



        }
        else if (accel < 0)
        {
            gear = 1;
            if (speed > 0)
            {
                speed += accel * my_props.accel_reverse * Time.deltaTime * 2.5f;
                //speed *= friction;
            }
            else
            {

                speed += accel * my_props.accel_reverse * Time.deltaTime;
                //speed *= friction;
            }
            //print("reversing... " + accel + "   : speed - " + speed);
            if (speed < my_props.speed_reverse * -1f)
            {
                speed = my_props.speed_reverse * -1f;
            }
            /*
            if (accel > 0 && my_rb.velocity.magnitude < 0.2f && speed > 0)
            {
                speed = 0f;
            }
            */
        }
        else
        {
            if (accel < 0.1f)
            {
                speed *= friction * friction;
            }

            if(speed < 50f)
            {
                speed = 0f;
            }
        }

        speed_vector.Set(0, 0, speed);
        disp = Mathf.Sqrt(Mathf.Abs(my_rb.velocity.magnitude + speed));

        //print("disp = " + disp + "   speed: " + speed);
        /*
        if (accel < 0.1f)
        {
            speed *= friction;
        }*/
        if (my_props.is_off)
        {
            speed *= 0.95f;
        }

        float turn_rate = my_props.turn;
        if (speed < 0)
            turn_rate *= -1f;
        if (disp * h_axis != 0)
        {
            if(disp < 55f)
            {
                turn_rate *= 2f;
            }
            my_rb.AddTorque(Vector3.up * turn_rate * disp * h_axis * Time.deltaTime);
        }

        speed_vector = transform.rotation * speed_vector;
        my_rb.AddForce(speed_vector * Time.deltaTime);


    }

}
