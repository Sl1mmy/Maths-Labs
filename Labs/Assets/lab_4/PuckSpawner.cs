using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject puckPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnPuck();
        }
    }

    void SpawnPuck()
    {
        Instantiate(puckPrefab, Vector3.zero, Quaternion.identity);
    }
}