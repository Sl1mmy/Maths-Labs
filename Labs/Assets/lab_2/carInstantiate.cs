// APPLIQUER CE SCRIPT A L'OBJET VIDE 'robot' 

using UnityEngine;

public class carInstantiate : MonoBehaviour
{
    void Start()
    {
        SpawnCar();
    }

    void SpawnCar()
    {
        GameObject car = gameObject; // Utiliser le GameObject existant dans la scene

        // creer body
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.transform.parent = car.transform;
        body.name = "body";
        body.transform.localPosition = new Vector3(0, 0.5f, 0);
        body.transform.localScale = new Vector3(1.75f, 0.6f, 3);

        // creer roues
        float wheelOffsetX = 1f;
        float wheelOffsetY = 0.25f;
        float wheelOffsetZ = 1f;
        float wheelRadius = 0.8f;

        Vector3[] wheelPositions = {
            new Vector3(-wheelOffsetX, wheelOffsetY, wheelOffsetZ), // Front Left
            new Vector3(wheelOffsetX, wheelOffsetY, wheelOffsetZ),  // Front Right
            new Vector3(-wheelOffsetX, wheelOffsetY, -wheelOffsetZ), // Rear Left
            new Vector3(wheelOffsetX, wheelOffsetY, -wheelOffsetZ)  // Rear Right
        };

        string[] wheelNames = { "frontLeftWheel", "frontRightWheel", "rearLeftWheel", "rearRightWheel" };

        for (int i = 0; i < wheelPositions.Length; i++)
        {
            GameObject wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            wheel.name = wheelNames[i];
            wheel.transform.parent = car.transform;
            wheel.transform.localPosition = wheelPositions[i];
            wheel.transform.localScale = new Vector3(wheelRadius, 0.15f, wheelRadius);
            wheel.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
}