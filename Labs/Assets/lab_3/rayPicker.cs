using UnityEngine;

public class RayPicker : MonoBehaviour
{
    public Camera sceneCam;
    public Material defaultMaterial;
    public Material selectedMaterial;
    public bool selectionOnClick = true;

    private GameObject selectedObject;

    void Update()
    {
        if (selectionOnClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectObject();
            }
        }
        else
        {
            SelectObject(); // Hover
        }
    }

    private void SelectObject()
    {
        Ray ray = sceneCam.ScreenPointToRay(Input.mousePosition);
        GameObject hitObject = CheckRayIntersection(ray);

        if (selectionOnClick)
        {
            // If clicking on an object, select it. If clicking empty space, deselect.
            if (hitObject != null && selectedObject != hitObject)
            {
                ChangeSelection(hitObject);
            }
            else if (hitObject == null)
            {
                DeselectObject();
            }
        }
        else
        {
            // Selection updates as the mouse moves
            if (hitObject != selectedObject)
            {
                ChangeSelection(hitObject);
            }
        }
    }

    private void ChangeSelection(GameObject newSelection)
    {
        if (selectedObject != null)
        {
            SetObjectMaterial(selectedObject, defaultMaterial);
        }

        selectedObject = newSelection;

        if (selectedObject != null)
        {
            SetObjectMaterial(selectedObject, selectedMaterial);
        }
    }

    private void DeselectObject()
    {
        if (selectedObject != null)
        {
            SetObjectMaterial(selectedObject, defaultMaterial);
            selectedObject = null;
        }
    }

    private GameObject CheckRayIntersection(Ray ray)
    {
        GameObject closestObject = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer == null) continue;

            Bounds bounds = renderer.bounds;
            Plane[] objectPlanes = GetObjectPlanes(bounds);

            foreach (var plane in objectPlanes)
            {
                if (RayIntersectsPlane(ray, plane, out Vector3 intersectionPoint))
                {
                    if (bounds.Contains(intersectionPoint))
                    {
                        float distance = Vector3.Distance(ray.origin, intersectionPoint);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestObject = obj;
                        }
                    }
                }
            }
        }

        return closestObject;
    }

    private Plane[] GetObjectPlanes(Bounds bounds)
    {
        Plane[] planes = new Plane[6];

        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        planes[0] = new Plane(Vector3.up, center + Vector3.up * extents.y);     // Top
        planes[1] = new Plane(Vector3.down, center - Vector3.up * extents.y);   // Bottom
        planes[2] = new Plane(Vector3.forward, center + Vector3.forward * extents.z); // Front
        planes[3] = new Plane(Vector3.back, center - Vector3.forward * extents.z); // Back
        planes[4] = new Plane(Vector3.right, center + Vector3.right * extents.x); // Right
        planes[5] = new Plane(Vector3.left, center - Vector3.right * extents.x); // Left

        return planes;
    }

    private bool RayIntersectsPlane(Ray ray, Plane plane, out Vector3 intersectionPoint)
    {
        float denominator = Vector3.Dot(plane.normal, ray.direction);
        intersectionPoint = Vector3.zero;

        if (Mathf.Abs(denominator) > 1e-6f) // Avoid division by zero
        {
            float t = -(Vector3.Dot(plane.normal, ray.origin) + plane.distance) / denominator;
            if (t >= 0)
            {
                intersectionPoint = ray.GetPoint(t);
                return true;
            }
        }

        return false;
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
