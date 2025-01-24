using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleChecker : MonoBehaviour
{
    private Transform centerCube;
    private Vector3 selfPos; 

    private float maxAngle;

    private bool voidCubes;

    void Update()
    {
        // Check angles in real time
        CheckAnglesForAllAxes();


        if (Input.GetKeyDown(KeyCode.W))
        {
            resetCubes();
            Debug.Log("Pressed W : reset objects");
        }
    }

    public void Initialize(float maximumAngle, Transform centerCubeTransform, bool toggleVoidCubes)
    {
        maxAngle = maximumAngle;
        centerCube = centerCubeTransform;
        selfPos = transform.position;
        voidCubes = toggleVoidCubes;

    }

    public void CheckAnglesForAllAxes()
    {
        bool isAngleValid = false;

        // Check each axis and if one is valid, change the color
        isAngleValid |= CheckAngle("x");
        isAngleValid |= CheckAngle("y");
        isAngleValid |= CheckAngle("z");

        if (!isAngleValid)
        {
            if (voidCubes)
            {
                GetComponent<Renderer>().enabled = false;
            } else
            {
                GetComponent<Renderer>().enabled = true;
                GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    public bool CheckAngle(string axis)
    {
        // negative axes to color the cubes with positive coordinates
        Vector3 Cpos;
        switch (axis.ToLower())
        {
            case "x":
                Cpos = -centerCube.TransformDirection(Vector3.right) * maxAngle; // Local X axis
                break;
            case "y":
                Cpos = -centerCube.TransformDirection(Vector3.up) * maxAngle; // Local Y axis
                break;
            case "z":
                Cpos = -centerCube.TransformDirection(Vector3.forward) * maxAngle; // Local Z axis
                break;
            default:
                Debug.LogError("Invalid axis specified! Use 'x', 'y', or 'z'.");
                return false;
        }

        // Calculate vectors AB and BC
        Vector3 AB = centerCube.position - selfPos;
        Vector3 BC = Cpos - centerCube.position;

        // Calculate the angle between AB and BC
        float angle = GetAngle(AB, BC);

        if (angle < maxAngle)
        {
            GetComponent<Renderer>().enabled = true;
            // Change the cube's color based on the axis
            if (axis == "x")
            {
                GetComponent<Renderer>().material.color = Color.red;
            }
            else if (axis == "y")
            {
                GetComponent<Renderer>().material.color = Color.green;
            }
            else if (axis == "z")
            {
                GetComponent<Renderer>().material.color = Color.blue;
            }

            return true;
        }
        else
        {
            return false; // Invalid angle
        }
    }

    public float GetAngle(Vector3 AB, Vector3 BC)
    {
        // Calculate the dot product of AB and BC
        float dotProduct = Vector3.Dot(AB, BC);

        // Calculate the magnitudes of AB and BC
        float magnitudeAB = AB.magnitude;
        float magnitudeBC = BC.magnitude;

        // Calculate the cosine of the angle
        float cosTheta = dotProduct / (magnitudeAB * magnitudeBC);

        // Clamp the value of cosTheta to avoid errors due to floating point precision issues
        cosTheta = Mathf.Clamp(cosTheta, -1f, 1f);

        // Calculate the angle in radians and convert to degrees
        return Mathf.Acos(cosTheta) * Mathf.Rad2Deg;
    }

    public void resetCubes()
    {
        Destroy(gameObject);

    }
}
