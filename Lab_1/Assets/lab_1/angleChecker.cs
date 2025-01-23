using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleChecker : MonoBehaviour
{
    private Vector3 centerCubePos;
    private Vector3 selfPos;

    private Vector3 Xpos;
    private Vector3 Ypos;
    private Vector3 Zpos;

    private float maxAngle;

    void Update()
    {
        // Disables White cubes on 'T' press
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleWhiteCubes();
        }

        // Recheck angles on 'S' press
        if (Input.GetKeyDown(KeyCode.S))
        {
            CheckAnglesForAllAxes();
        }
    }

    public void Initialize(float maximumAngle, Vector3 centerCubePosition, Vector3 Xposition, Vector3 Yposition, Vector3 Zposition)
    {
        maxAngle = maximumAngle;
        centerCubePos = centerCubePosition;
        Xpos = Xposition;
        Ypos = Yposition;
        Zpos = Zposition;
        selfPos = transform.position;

        // Check all 3 axes at once
        CheckAnglesForAllAxes();
    }

    public void CheckAnglesForAllAxes()
    {
        bool isAngleValid = false;

        // Check each axis and if one is valid, change the color
        isAngleValid |= CheckAngle("x");
        isAngleValid |= CheckAngle("y");
        isAngleValid |= CheckAngle("z");

        // If the angle is valid for at least one axis, leave it colored, else make it white
        if (!isAngleValid)
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }


    public bool CheckAngle(string axis)
    {
        // Determine which axis to use (C)
        Vector3 Cpos;
        switch (axis.ToLower())
        {
            case "x":
                Cpos = Xpos;
                break;
            case "y":
                Cpos = Ypos;
                break;
            case "z":
                Cpos = Zpos;
                break;
            default:
                Debug.LogError("Invalid axis specified! Use 'x', 'y', or 'z'.");
                return false;
        }


        Vector3 AB = centerCubePos - selfPos;
        Vector3 BC = Cpos - centerCubePos;

        // Calculate the angle between AB and BC
        float angle = getAngle(AB, BC);
        //float angle = Vector3.Angle(AB, BC); // Built in method to get angle


        if (angle < maxAngle)
        {
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
            return false; // invalid angle
        }
    }

    public float getAngle(Vector3 AB, Vector3 BC)
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


    public void ToggleWhiteCubes()
    {
        if (GetComponent<Renderer>().material.color == Color.white)
        {
            gameObject.SetActive(false);
        }
    }
}
