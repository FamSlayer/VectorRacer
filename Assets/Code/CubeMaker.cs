using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMaker : MonoBehaviour
{
    public GameObject expansion_cube;
    public int cube_count;
    public float x_min, x_max, y_min, y_max, z_min, z_max;

    void Awake()
    {
        //int cube_count = Random.Range(50, 250);
        for(int i = 0; i < cube_count; i++)
        {
            GameObject c = Instantiate(expansion_cube, transform);
            c.transform.localPosition = new Vector3(Random.Range(x_min, x_max), Random.Range(y_min, y_max), Random.Range(z_min, z_max));
        }
    }
    
}
