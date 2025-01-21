using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeGenerator : MonoBehaviour
{
    // Public variables to adjust in the Unity Inspector
    public float radius = 5f; // Radius around the object
    public int numberOfCubes = 5; // Number of cubes to generate

    void Update()
    {
        // When Spacebar is pressed, spawn cubes
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCubes();
        }
    }

    void SpawnCubes()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            // Generate a random position within the specified radius around this object
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * radius;

            // Instantiate a cube at the random position
            Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), randomPosition, Quaternion.identity);
        }
    }
}
