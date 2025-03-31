using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class moveCamera : MonoBehaviour
{
    public Transform[] controlPoints = new Transform[13];

    public float camSpeed;
    private float t = 0;

    public int currentMode = 0;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentMode = (currentMode + 1) % 3;
            Debug.Log("Switched to: " + currentMode);
        }

        t += Time.deltaTime * camSpeed;
        if (t > 1.0f) t = 0.0f; // loop animation

        Vector3 nextPosition = Vector3.zero;

        switch (currentMode)
        {
            case (int)CameraMode.Linear:
                nextPosition = LinearInterpolation(t);
                break;

            case (int)CameraMode.Bezier:
                nextPosition = CubicBezier(t);
                break;

            case (int)CameraMode.CatmullRom:
                nextPosition = CatmullRom(t);
                break;
        }

        transform.position = nextPosition;
        transform.LookAt(nextPosition + (nextPosition - transform.position).normalized);
    }


    Vector3 LinearInterpolation(float t)
    {
        int segment = Mathf.FloorToInt(t * (controlPoints.Length - 1)); // Find the segment index
        float localT = (t * (controlPoints.Length - 1)) - segment; // Normalize t for this segment

        Transform p0 = controlPoints[segment];
        Transform p1 = controlPoints[(segment + 1) % controlPoints.Length]; // Loop back at the end

        return (1 - localT) * p0.position + localT * p1.position;
    }

    Vector3 CubicBezier(float t)
    {
        int segmentCount = (controlPoints.Length - 1) / 3; // Number of segments
        int segment = Mathf.FloorToInt(t * segmentCount); // Current segment
        float localT = (t * segmentCount) - segment; // Normalize t within segment

        // Select four consecutive points in groups of 3
        int i = segment * 3; // Move through control points in steps of 3

        Transform P0 = controlPoints[i % controlPoints.Length];
        Transform P1 = controlPoints[(i + 1) % controlPoints.Length];
        Transform P2 = controlPoints[(i + 2) % controlPoints.Length];
        Transform P3 = controlPoints[(i + 3) % controlPoints.Length];

        return Mathf.Pow(1 - localT, 3) * P0.position +
               3 * Mathf.Pow(1 - localT, 2) * localT * P1.position +
               3 * (1 - localT) * Mathf.Pow(localT, 2) * P2.position +
               Mathf.Pow(localT, 3) * P3.position;
    }

    Vector3 CatmullRom(float t)
    {
        int segmentCount = controlPoints.Length - 1; // Number of segments
        int segment = Mathf.FloorToInt(t * segmentCount); // Current segment
        float localT = (t * segmentCount) - segment; // Normalize t within segment

        // Get four points, ensuring the last segment loops back
        int i = segment;
        Transform P0 = controlPoints[(i - 1 + controlPoints.Length) % controlPoints.Length];
        Transform P1 = controlPoints[i % controlPoints.Length];
        Transform P2 = controlPoints[(i + 1) % controlPoints.Length];
        Transform P3 = controlPoints[(i + 2) % controlPoints.Length];

        float t2 = localT * localT;
        float t3 = t2 * localT;

        return 0.5f * (
            (2 * P1.position) +
            (-P0.position + P2.position) * localT +
            (2 * P0.position - 5 * P1.position + 4 * P2.position - P3.position) * t2 +
            (-P0.position + 3 * P1.position - 3 * P2.position + P3.position) * t3
        );
    }
}

enum CameraMode { Linear = 0, Bezier = 1, CatmullRom = 2}
