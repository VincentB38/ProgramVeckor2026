using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);

    void LateUpdate()
    {

        transform.position = player.position + offset;

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
}