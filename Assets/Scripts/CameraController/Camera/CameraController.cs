using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movement_speed = 1;
    public float movement_time = 1;

    public Vector3 new_position;

    // Start is called before the first frame update
    void Start()
    {
        new_position = transform.position;
    }
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int x = (int)Mathf.Round(transform.position.x);
            int z = (int)Mathf.Round(transform.position.z);
            Vector3 pos = new Vector3(x, transform.position.y, z);
            Debug.Log($"Position: {pos}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        /*int x = (int)Mathf.Round(transform.position.x);
        int z = (int)Mathf.Round(transform.position.z);
        try
        {
            grid_object.tiles_list[x, z].SetColor(Color.cyan);
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogWarning(e);
        }*/

        void HandleMovement()
        {
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;
            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
            {
                new_position = new Vector3(x, y, (z + 1) * movement_speed); //(transform.forward * movement_speed);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                new_position = new Vector3(x, y, (z - 1) * movement_speed);//(transform.forward * -movement_speed);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                new_position = new Vector3((x + 1) * movement_speed, y, z);//(transform.right * movement_speed);
            }
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
            {
                new_position = new Vector3((x - 1) * movement_speed, y, z); //(transform.right * -movement_speed);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                transform.position = transform.position;
            }

            transform.position = Vector3.Lerp(transform.position, new_position, movement_time * Time.deltaTime);

        }


    }
}
