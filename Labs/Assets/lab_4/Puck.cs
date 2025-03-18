using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Puck : MonoBehaviour
{
    [SerializeField]
    Vector3 position;

    [SerializeField]
    float direction;

    [SerializeField]
    float speed;

    float radius = 0.2f;

    // Boundaries of the map
    private float minX = -20f, maxX = 20f;
    private float minZ = -10f, maxZ = 10f;

    void Start()
    {
        // Settings for attacker position
        position = new Vector3(Random.Range(8f, 19f), 0.1f, Random.Range(-9f, 9f));
        direction = Random.Range(190f, 350f);

        speed = Random.Range(3f, 15f);

        SetPuck(position, direction);
    }

    void Update()
    {
        position = GetNewPosition(position);

        CheckWallCollision();
        CheckAllPuckCollisions();

        transform.position = position;
    }

    void SetPuck(Vector3 position, float direction)
    {
        GameObject tempPuck = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        MeshFilter meshFilter = tempPuck.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = tempPuck.GetComponent<MeshRenderer>();

        if (meshFilter && meshRenderer)
        {
            gameObject.AddComponent<MeshFilter>().mesh = meshFilter.mesh;
            gameObject.AddComponent<MeshRenderer>().material = meshRenderer.material;
        }

        Destroy(tempPuck);

        transform.localScale = new Vector3(1f, radius, 1f);
        transform.position = position;
        transform.rotation = Quaternion.Euler(0f, direction, 0f);
        GetComponent<Renderer>().material.color = Color.black;
    }

    Vector3 GetNewPosition(Vector3 previousPosition)
    {
        Vector3 directionVector = new Vector3(Mathf.Sin(direction * Mathf.Deg2Rad), 0, Mathf.Cos(direction * Mathf.Deg2Rad));
        return previousPosition + directionVector * speed * Time.deltaTime;
    }

    void CheckWallCollision()
    {
        // Check wall collision and reflect puck
        if (position.z >= maxZ || position.z <= minZ) {
            direction = CalculateReflectionDirection(direction, Vector3.forward * Mathf.Sign(position.z));
            transform.rotation = Quaternion.Euler(0f, direction, 0f);
        }

        if (position.x >= maxX || position.x <= minX) {
            direction = CalculateReflectionDirection(direction, Vector3.right * Mathf.Sign(position.x));
            transform.rotation = Quaternion.Euler(0f, direction, 0f);
        }
    }

    private void CheckAllPuckCollisions()
    {
        Puck[] allPucks = FindObjectsOfType<Puck>(); // Find all Puck instances in the scene

        foreach (Puck otherPuck in allPucks)
        {
            if (otherPuck != this)
            {
                CheckPuckCollision(otherPuck);
            }
        }
    }

    void CheckPuckCollision(Puck otherPuck)
    {
        // Calculate the vector between the two pucks
        Vector3 directionToOther = otherPuck.position - position;

        // Calculate the distance between the pucks
        float distance = directionToOther.magnitude;

        if (distance < radius + otherPuck.radius) // Collide
        {
            // Normalized direction
            Vector3 collisionNormal = directionToOther.normalized;

            // Reflection
            direction = CalculateReflectionDirection(direction, collisionNormal);
            transform.rotation = Quaternion.Euler(0f, direction, 0f);

            otherPuck.direction = CalculateReflectionDirection(otherPuck.direction, collisionNormal);
            otherPuck.transform.rotation = Quaternion.Euler(0f, otherPuck.direction, 0f);

            // Adjust the pucks' positions so they're not overlapping
            float overlap = radius + otherPuck.radius - distance;
            Vector3 adjustment = collisionNormal * overlap / 2f;

            position -= adjustment;
            otherPuck.position += adjustment;
        }
    }

    // Reflect the direction based on the normal of the collision
    float CalculateReflectionDirection(float puckDirection, Vector3 wallNormal)
    {
        Vector3 directionVector = new Vector3(Mathf.Sin(puckDirection * Mathf.Deg2Rad), 0, Mathf.Cos(puckDirection * Mathf.Deg2Rad));
        Vector3 reflectedDirection = directionVector - 2 * Vector3.Dot(directionVector, wallNormal) * wallNormal; // Sphere-Plane formula
        //Vector3 reflectedDirection = Vector3.Reflect(directionVector, wallNormal); // Built in method

        return Mathf.Atan2(reflectedDirection.x, reflectedDirection.z) * Mathf.Rad2Deg;
    }
}