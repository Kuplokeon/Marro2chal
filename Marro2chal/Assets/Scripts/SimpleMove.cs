using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public bool moveForward;
    public Vector3 movement;
    public float movementSpeed;

    // Update is called once per frame
    void Update()
    {
        Vector3 finalMovement = movement;
        if (moveForward)
        {
            finalMovement = transform.forward;
        }
        finalMovement *= movementSpeed * Time.deltaTime;

        transform.position += finalMovement;
    }
}
