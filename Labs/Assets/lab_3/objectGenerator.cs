using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [Range(15.0f, 75.0f)]
    public float radius = 25f;

    [Range(2.0f, 50.0f)]
    public float maxAngle = 15f;

    [Range(10.0f, 1000.0f)]
    public int numberOfCubes = 5;

    public GameObject objectPrefab;

    public bool ToggleVoidCubes;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCubes();
            Debug.Log("Pressed Space : Spawning " + numberOfCubes + " objects");
        }
    }

    public void SpawnCubes()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * radius;

            GameObject cube = Instantiate(objectPrefab, randomPosition, Quaternion.identity);

            cube.AddComponent<BoxCollider>();
        }
    }
}
