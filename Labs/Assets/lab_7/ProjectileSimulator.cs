using System.Collections.Generic;
using UnityEngine;

public class ProjectileSimulator : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float projectileRadius = 0.5f;

    [Header("Launch Parameters")]
    [Range(0f, 90f)] public float angleElevation = 45f;  // Angle vertical (par rapport au sol, sur l'axe Y)
    [Range(0f, 360f)] public float angleAzimuth = 0f;    // Angle horizontal dans le plan XZ
    public float launchSpeed = 20f;

    [Header("Physics Settings")]
    public float gravity = 9.8f;

    [Header("Targets")]
    public List<Transform> targets;
    public float targetRadius = 1f;

    private GameObject activeProjectile;
    private Vector3 initialVelocity;
    private Vector3 initialPosition;
    private float time;
    private bool isLaunched = false;

    void Update()
    {
        if (isLaunched && activeProjectile != null)
        {
            time += Time.deltaTime;
            Vector3 newPos = ComputeProjectilePosition(initialPosition, initialVelocity, time);
            activeProjectile.transform.position = newPos;

            // Collision check
            foreach (var target in targets)
            {
                if (AreSpheresColliding(newPos, projectileRadius, target.position, targetRadius))
                {
                    Debug.Log("Collision with : " + target.name);
                    isLaunched = false;
                }
            }

            // Stop simulation if falls below ground level (Y axis)
            if (newPos.y < 0f)
            {
                Debug.Log("Projectile touched ground");
                isLaunched = false;
            }
        }

        // Press Space to launch
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchProjectile();
        }
    }

    void LaunchProjectile()
    {
        if (activeProjectile != null)
        {
            Destroy(activeProjectile);
        }

        activeProjectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        initialPosition = spawnPoint.position;
        initialVelocity = ComputeInitialVelocity(launchSpeed, angleAzimuth, angleElevation);
        time = 0f;
        isLaunched = true;
        Debug.Log("Projectile launched");
    }

    Vector3 ComputeInitialVelocity(float speed, float angleAzimuthDeg, float angleElevationDeg)
    {
        float azimuthRad = Mathf.Deg2Rad * angleAzimuthDeg;
        float elevationRad = Mathf.Deg2Rad * angleElevationDeg;

        float vx = speed * Mathf.Cos(elevationRad) * Mathf.Cos(azimuthRad);
        float vz = speed * Mathf.Cos(elevationRad) * Mathf.Sin(azimuthRad);
        float vy = speed * Mathf.Sin(elevationRad); // Y is vertical

        return new Vector3(vx, vy, vz);
    }

    Vector3 ComputeProjectilePosition(Vector3 pos0, Vector3 vel0, float t)
    {
        Vector3 gravityVec = new Vector3(0, -gravity, 0); // Gravity on Y
        return pos0 + vel0 * t + 0.5f * gravityVec * t * t;
    }

    bool AreSpheresColliding(Vector3 pos1, float r1, Vector3 pos2, float r2)
    {
        float dist = Vector3.Distance(pos1, pos2);
        return dist <= (r1 + r2);
    }

    void OnDrawGizmos()
    {
        if (spawnPoint == null) return;

        Gizmos.color = Color.green;
        Vector3 pos0 = spawnPoint.position;
        Vector3 vel0 = ComputeInitialVelocity(launchSpeed, angleAzimuth, angleElevation);

        for (float t = 0; t < 5f; t += 0.1f)
        {
            Vector3 point = ComputeProjectilePosition(pos0, vel0, t);
            Gizmos.DrawSphere(point, 0.05f);
        }
    }
}
