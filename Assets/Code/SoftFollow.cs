using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftFollow : MonoBehaviour
{
    public GameObject target;
    public bool follow_pos;
    public bool follow_rot;
    public float horiz_follow_dist;
    public float vert_follow_dist;

    //public float lerp_speed;

    Vector3 offset;
    Vector3 curr_pos;

    Quaternion orig_rot;
    void Awake()
    {
        orig_rot = transform.rotation;
        offset = -target.transform.forward * horiz_follow_dist + Vector3.up * vert_follow_dist;
        curr_pos = target.transform.position + offset;
    }

    void Update ()
    {
        if(follow_pos)
        {
            offset = -target.transform.forward * horiz_follow_dist + target.transform.up * vert_follow_dist;
            Debug.DrawLine(target.transform.position, target.transform.position + offset, Color.blue);

            transform.position = target.transform.position + offset;
        }

        if(follow_rot)
        {
            transform.up = target.transform.up;
            transform.Rotate(target.transform.localEulerAngles.x + 25f, target.transform.eulerAngles.y, 0f);
        }
    }
}
