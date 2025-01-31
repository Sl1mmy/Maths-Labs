// APPLIQUER CE SCRIPT A L'OBJET VIDE 'robot' 

using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Transform frontLeftWheel;
    private Transform frontRightWheel;

    public float turnAngle = 30f;
    private float currentTurnAngle = 0f;
    private float targetTurnAngle = 0f;

    public float maxSpeed = 10f;
    private float currentSpeed = 0f;

    public float acceleration = 40f;
    public float deceleration = 50f;
    public float baseTurnRadius = 5f; // Rayon de base a basse vitesse
    public float turnSmoothness = 5f; // Controle la vitesse a laquelle la direction s'adapte

    void Start()
    {
        frontLeftWheel = transform.Find("frontLeftWheel");
        frontRightWheel = transform.Find("frontRightWheel");
        Debug.Log("left wheel: " + frontLeftWheel);
        Debug.Log("right wheel: " + frontRightWheel);
    }

    void Update()
    {
        float turnInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            turnInput = -1f;
            //Debug.Log("Turn Left!");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turnInput = 1f;
            //Debug.Log("Turn Right!");
        }

        // Ajuster en douceur l'angle de direction en fonction de l'input
        targetTurnAngle = turnInput * turnAngle;

        // Adoucir l'angle de direction vers le target
        currentTurnAngle = Mathf.LerpAngle(currentTurnAngle, targetTurnAngle, turnSmoothness * Time.deltaTime);

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
                currentSpeed -= deceleration * Time.deltaTime;
            else if (currentSpeed < 0)
                currentSpeed += deceleration * Time.deltaTime;

            if (Mathf.Abs(currentSpeed) < 0.1f)
                currentSpeed = 0f;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // Update la rotation des roues
        if (frontLeftWheel != null && frontRightWheel != null)
        {
            frontLeftWheel.localRotation = Quaternion.Euler(0, currentTurnAngle, 90);
            frontRightWheel.localRotation = Quaternion.Euler(0, currentTurnAngle, 90);
        }

        MoveCar();
    }


    void MoveCar()
    {
        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            // Ajuste le rayon par rapport a la vitesse
            float turnRadiusAdjusted = baseTurnRadius / (1 + Mathf.Abs(currentSpeed) / maxSpeed);

            // Applique la rotation par rapport au nouveau rayon
            if (currentTurnAngle != 0)
            {
                float turnSpeed = currentSpeed / turnRadiusAdjusted;
                transform.Rotate(0, currentTurnAngle * Time.deltaTime * turnSpeed, 0);
            }

            // Avance/recule la voiture dans la direction actuelle
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        }
    }
}
