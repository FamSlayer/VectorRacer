using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float min_speed;
    public float max_speed;

    public float speed;

    Quaternion axis;

    float x, y, z;

    public Vector3 rot_axis;

    private void Awake()
    {
        speed = Random.Range(min_speed, max_speed);
        print(speed);
        x = Random.Range(-4f, 4f);
        y = Random.Range(-30, -5f);
        z = Random.Range(4f, 8f);
        rot_axis = new Vector3(x, y, z);
    }


    void Update ()
    {
        transform.Rotate(rot_axis, speed * Time.deltaTime);
	}
}
