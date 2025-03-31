using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curves : MonoBehaviour
{
    public Vector3 LinearInterpolation(Vector3 P0, Vector3 P1, float t)
    {
        return (1 - t) * P0 + t * P1;
    }

    public Vector3 CubicBezier(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
    {
        float u = 1 - t;
        return u*u*u * P0 + 3 * u*u*t * P1 + 3 * u*t*t * P2 + t*t*t * P3;
    }

    public Vector3 CatmullRom(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2 * P1) +
            (-P0 + P2) * t +
            (2 * P0 - 5 * P1 + 4 * P2 - P3) * t2 +
            (-P0 + 3 * P1 - 3 * P2 + P3) * t3
        );
    }

}
