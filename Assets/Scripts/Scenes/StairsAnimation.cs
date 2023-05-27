using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsAnimation : MonoBehaviour
{
    public float speed = 1.0f;
    public float movementDistance = 2.0f; // Adjust the distance the stairs will move

    private Vector3 originalPosition;
    private float movementProgress = 0.0f;
    private bool isMovingDown = true;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (isMovingDown)
        {
            movementProgress += speed * Time.deltaTime;

            // Calculate the new position using Lerp
            Vector3 newPosition = originalPosition + Vector3.down * movementDistance * movementProgress +
                Vector3.left * movementDistance * movementProgress;

            // Apply the new position
            transform.position = newPosition;

            if (movementProgress >= 1.0f)
            {
                isMovingDown = false;
                movementProgress = 0.0f;
            }
        }
        else
        {
            movementProgress += speed * Time.deltaTime;

            // Calculate the new position using Lerp
            Vector3 newPosition = originalPosition + Vector3.up * movementDistance * movementProgress +
                Vector3.right * movementDistance * movementProgress;

            // Apply the new position
            transform.position = newPosition;

            if (movementProgress >= 1.0f)
            {
                isMovingDown = true;
                movementProgress = 0.0f;
            }
        }
    }
}
