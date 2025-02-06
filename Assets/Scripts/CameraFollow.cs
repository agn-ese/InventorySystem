using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float followSpeed = 20f;
    [SerializeField] private Transform target;

    void Update()
    {
        Vector3 newPosition = new Vector3(target.position.x, target.position.y + 1, -10f);
        transform.position = Vector3.Slerp(transform.position, newPosition, followSpeed*Time.deltaTime);
    }
}
