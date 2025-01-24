using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    [Range(10.0f, 60.0f)]
    public float rotationSpeed = 30f; // Rotation speed in degrees per second
    [Range(60.0f, 120.0f)]
    public float spinSpeed = 90f; // Spin speed in degrees per second

    private bool isRotating = false;   

    private Quaternion originalRotation;  // Store the initial rotation

    void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isRotating)
            {
                ResetRotation();
                Debug.Log("Pressed R : Reset rotation");
            }
            else
            {
                StartRotation();
                Debug.Log("Pressed R : Started rotation");
            }
        }

        if (isRotating)
        {
            RotateAndSpinCube();
        }
    }

    void StartRotation()
    {
        isRotating = true;
    }

    void ResetRotation()
    {
        isRotating = false;
        transform.rotation = originalRotation; // Reset
    }

    void RotateAndSpinCube()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime, Space.World);

        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.Self);
    }
}
