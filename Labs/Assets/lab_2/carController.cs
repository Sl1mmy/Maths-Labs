// APPLIQUER CE SCRIPT A L'OBJET VIDE 'robot' 

using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Transform frontLeftWheel;
    private Transform frontRightWheel;
    private Transform rearLeftWheel;
    private Transform rearRightWheel;

    public float turnAngle = 30f;
    private float currentTurnAngle = 0f;
    private float targetTurnAngle = 0f;

    public float maxSpeed = 10f;
    private float currentSpeed = 0f;

    public float acceleration = 40f;
    public float deceleration = 50f;
    public float baseTurnRadius = 5f;
    public float turnSmoothness = 5f;

    public float wheelRadius = 0.8f;

    private float wheelRotationAngle = 0f;

    void Start()
    {
        frontLeftWheel = transform.Find("frontLeftWheel");
        frontRightWheel = transform.Find("frontRightWheel");
        rearLeftWheel = transform.Find("rearLeftWheel");
        rearRightWheel = transform.Find("rearRightWheel");

        if (!frontLeftWheel || !frontRightWheel || !rearLeftWheel || !rearRightWheel)
        {
            Debug.LogError("One or more wheels not found! Make sure they are correctly named in the hierarchy.");
        }
    }

    void Update()
    {
        float turnInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            turnInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turnInput = 1f;
        }

        // Ajuster en douceur l'angle de direction
        targetTurnAngle = turnInput * turnAngle;
        currentTurnAngle = Mathf.LerpAngle(currentTurnAngle, targetTurnAngle, turnSmoothness * Time.deltaTime);

        // Acceleration / Deceleration
        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }
        else
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= deceleration * Time.deltaTime;
            }
            else if (currentSpeed < 0)
            {
                currentSpeed += deceleration * Time.deltaTime;
            }

            if (Mathf.Abs(currentSpeed) < 0.1f)
            {
                currentSpeed = 0f;
            }
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        MoveCar();
        RotateWheels();
    }

    void MoveCar()
    {
        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float turnRadiusAdjusted = baseTurnRadius / (1 + Mathf.Abs(currentSpeed) / maxSpeed);

            if (currentTurnAngle != 0)
            {
                float turnSpeed = currentSpeed / turnRadiusAdjusted;
                transform.Rotate(0, currentTurnAngle * Time.deltaTime * turnSpeed, 0);
            }

            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        }
    }

    void RotateWheels()
    {
        if (!frontLeftWheel || !frontRightWheel || !rearLeftWheel || !rearRightWheel) return;

        // Calcule le roulement des roues par rapport au deplacement
        float distanceTraveled = currentSpeed * Time.deltaTime;
        wheelRotationAngle += (distanceTraveled * 360f) / (2f * Mathf.PI * wheelRadius);

        Quaternion turnRotation = Quaternion.Euler(0, currentTurnAngle, 0);
        Quaternion rollRotation = Quaternion.Euler(wheelRotationAngle, 0, 90);

        frontLeftWheel.localRotation = turnRotation * rollRotation;
        frontRightWheel.localRotation = turnRotation * rollRotation;

        rearLeftWheel.localRotation = rollRotation;
        rearRightWheel.localRotation = rollRotation;
    }
}
