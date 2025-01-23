using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public float radius = 25f;
    public float maxAngle = 15f;
    public int numberOfCubes = 5;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCubes();
        }
    }

    void SpawnCubes()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * radius;

            GameObject cube = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), randomPosition, Quaternion.identity);

            AngleChecker angleChecker = cube.AddComponent<AngleChecker>();

            angleChecker.Initialize(
                maxAngle, // Maximum allowed angle
                transform.position, // Center cube position

                // Negative value to detect positive positions
                new Vector3(-radius, 0, 0), // Xpos
                new Vector3(0, -radius, 0), // Ypos
                new Vector3(0, 0, -radius)  // Zpos
            ); 
        }
    }
}
