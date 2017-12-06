using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftFollow : MonoBehaviour
{

    public GameObject target;
    public float horiz_follow_dist;
    public float vert_follow_dist;

    public float lerp_speed;

    Vector3 offset;
    Vector3 curr_pos;

    Quaternion orig_rot;
    void Awake()
    {
        orig_rot = transform.rotation;
        offset = -target.transform.forward * horiz_follow_dist + Vector3.up * vert_follow_dist;
        curr_pos = target.transform.position + offset;
    }

    void LateUpdate ()
    {

        Debug.DrawLine(transform.position, transform.position + transform.forward * 3, Color.blue);

        offset = -target.transform.forward * horiz_follow_dist + Vector3.up * vert_follow_dist;
        /*
        float offset_dist = offset.magnitude;
        float curr_dist = Vector3.Distance(transform.position, target.transform.position);

        float ratio = Mathf.Abs((curr_dist - offset_dist) / offset_dist);
        //print("camera value: " + ratio);

        float new_lerp = lerp_speed * (1 + ratio);
        //print(new_lerp);
        curr_pos = Vector3.Lerp(curr_pos, target.transform.position + offset, new_lerp * Time.deltaTime);
        transform.position = curr_pos;
        //transform.position = Vector3.Lerp(transform.position, , lerp_speed * Time.deltaTime);
        */
        transform.position = target.transform.position + offset;



        Quaternion q = orig_rot * Quaternion.AngleAxis(target.transform.eulerAngles.y, target.transform.up);
        transform.rotation = Quaternion.Euler(target.transform.eulerAngles.x + 25f, target.transform.eulerAngles.y, target.transform.eulerAngles.z);

        //transform.Rotate(target.transform.up, transform.eulerAngles.y - target.transform.eulerAngles.y);

        Debug.DrawLine(transform.position, transform.position + transform.forward * 3, Color.green);
    }
}
