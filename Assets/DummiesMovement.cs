using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummiesMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 pos1;
    [SerializeField]
    private Vector3 pos2;
    [SerializeField]
    private int maxSpeed = 3;
    private float speed = 0;

    //move from one point to another, faster and faster
    void Update()
    {
        gameObject.transform.position = Vector3.Lerp(pos1, pos2, Mathf.PingPong(Time.timeSinceLevelLoad * speed, 1.0f));

        if (speed < maxSpeed)
        {
            speed = speed + Time.deltaTime / 15;
        }
        else
        {
            speed = maxSpeed;
        }

    }


}
