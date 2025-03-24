using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastChecker : MonoBehaviour
{
    public Material redMaterial;
    public Material greenMaterial;
    public Material blueMaterial;
    public List<GameObject> cubes; // List of cubes in the scene

    private void Update()
    {
        CheckAxesForCubes();
    }

    public void appendCube(GameObject cube)
    {
        cubes.Add(cube);
    }

    private void CheckAxesForCubes()
    {
        HashSet<GameObject> touchedCubes = new HashSet<GameObject>();

        touchedCubes.UnionWith(CheckAxis(this.transform.right, redMaterial));  // X-axis
        touchedCubes.UnionWith(CheckAxis(this.transform.up, greenMaterial));   // Y-axis
        touchedCubes.UnionWith(CheckAxis(this.transform.forward, blueMaterial)); // Z-axis

        // Make untouched cubes invisible
        foreach (var cube in cubes)
        {
            if (!touchedCubes.Contains(cube))
            {
                MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.enabled = false; // Make cube invisible
                }
            }
        }
    }

    private HashSet<GameObject> CheckAxis(Vector3 direction, Material material)
    {
        HashSet<GameObject> touchedCubes = new HashSet<GameObject>();

        foreach (var cube in cubes)
        {
            if (IsCubeOnAxis(transform.position, direction, cube)) // Using the position of the central cube (this)
            {
                MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material = material;
                    renderer.enabled = true; // Make sure the cube is visible if it intersects with the axis
                }
                touchedCubes.Add(cube);
            }
        }
        return touchedCubes;
    }

    private bool IsCubeOnAxis(Vector3 origin, Vector3 direction, GameObject cube)
    {
        MeshFilter meshFilter = cube.GetComponent<MeshFilter>();
        if (meshFilter == null) return false;

        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Triangle3 triangle = new Triangle3(
                cube.transform.TransformPoint(vertices[triangles[i]]),
                cube.transform.TransformPoint(vertices[triangles[i + 1]]),
                cube.transform.TransformPoint(vertices[triangles[i + 2]])
            );

            if (RayIntersectsTriangle(origin, direction, triangle) != null)
            {
                return true;
            }
        }
        return false;
    }

    public static Vector3? RayIntersectsTriangle(Vector3 rayOrigin, Vector3 rayVector, Triangle3 triangle)
    {
        const float epsilon = 1e-6f;

        Vector3 edge1 = triangle.B - triangle.A;
        Vector3 edge2 = triangle.C - triangle.A;
        Vector3 rayCrossE2 = Vector3.Cross(rayVector, edge2);
        float det = Vector3.Dot(edge1, rayCrossE2);

        if (det > -epsilon && det < epsilon)
            return null; // This ray is parallel to this triangle.

        float invDet = 1.0f / det;
        Vector3 s = rayOrigin - triangle.A;
        float u = invDet * Vector3.Dot(s, rayCrossE2);

        if ((u < 0 && Mathf.Abs(u) > epsilon) || (u > 1 && Mathf.Abs(u - 1) > epsilon))
            return null;

        Vector3 sCrossE1 = Vector3.Cross(s, edge1);
        float v = invDet * Vector3.Dot(rayVector, sCrossE1);

        if ((v < 0 && Mathf.Abs(v) > epsilon) || (u + v > 1 && Mathf.Abs(u + v - 1) > epsilon))
            return null;

        // Compute t to find intersection point on the line
        float t = invDet * Vector3.Dot(edge2, sCrossE1);

        if (t > epsilon) // Ray intersection
        {
            return rayOrigin + rayVector * t;
        }
        else // Line intersection but not a ray intersection
        {
            return null;
        }
    }
}

public struct Triangle3
{
    public Vector3 A, B, C;

    public Triangle3(Vector3 a, Vector3 b, Vector3 c)
    {
        A = a;
        B = b;
        C = c;
    }
}

