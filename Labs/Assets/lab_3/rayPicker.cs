using UnityEngine;

public class RayPicker : MonoBehaviour
{
    public Camera sceneCam;  
    public Material defaultMaterial;
    public Material selectedMaterial;  
    private GameObject selectedObject;

    void Update()
    {
        Ray ray = sceneCam.ScreenPointToRay(Input.mousePosition);

        GameObject hitObject = CheckRayIntersection(ray);

        if (hitObject != null)
        {
            if (selectedObject != hitObject)
            {
                if (selectedObject != null)
                {
                    SetObjectMaterial(selectedObject, defaultMaterial);
                }

                selectedObject = hitObject;
                SetObjectMaterial(selectedObject, selectedMaterial);
            }
        }
        else
        {
            if (selectedObject != null)
            {
                SetObjectMaterial(selectedObject, defaultMaterial);
                selectedObject = null;
            }
        }
    }

    private GameObject CheckRayIntersection(Ray ray)
    {
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                Bounds bounds = renderer.bounds;

                // Create planes for all sides of the object
                Plane[] objectPlanes = GetObjectPlanes(obj, bounds);

                foreach (var plane in objectPlanes)
                {
                    if (RayIntersectsPlane(ray, plane, out Vector3 intersectionPoint))
                    {
                        if (IsPointInObjectBounds(obj, intersectionPoint))
                        {
                            return obj;
                        }
                    }
                }
            }
        }
        return null;
    }

    private Plane[] GetObjectPlanes(GameObject obj, Bounds bounds)
    {
        Plane[] planes = new Plane[6];

        Vector3 position = obj.transform.position;

        // Top Plane (Y+)
        planes[0] = new Plane(Vector3.up, position + Vector3.up * bounds.extents.y);

        // Bottom Plane (Y-)
        planes[1] = new Plane(Vector3.down, position + Vector3.down * bounds.extents.y);

        // Front Plane (Z+)
        planes[2] = new Plane(Vector3.forward, position + Vector3.forward * bounds.extents.z);

        // Back Plane (Z-)
        planes[3] = new Plane(Vector3.back, position + Vector3.back * bounds.extents.z);

        // Left Plane (X-)
        planes[4] = new Plane(Vector3.left, position + Vector3.left * bounds.extents.x);

        // Right Plane (X+)
        planes[5] = new Plane(Vector3.right, position + Vector3.right * bounds.extents.x);

        return planes;
    }

    private bool RayIntersectsPlane(Ray ray, Plane plane, out Vector3 intersectionPoint)
    {
        float t = 0;
        intersectionPoint = Vector3.zero;
        if (plane.Raycast(ray, out t) && t >= 0)
        {
            intersectionPoint = ray.GetPoint(t);  
            return true;
        }
        return false;
    }

    private bool IsPointInObjectBounds(GameObject obj, Vector3 point)
    {
        Bounds bounds = obj.GetComponent<Renderer>().bounds;
        return bounds.Contains(point);
    }

    private void SetObjectMaterial(GameObject obj, Material mat)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = mat;
        }
    }
}
