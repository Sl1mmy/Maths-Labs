using UnityEngine;

public class ObjectPicker : MonoBehaviour
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
            SelectObject(); // Hover selection
        }
    }

    private void SelectObject()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = ScreenToWorld(mousePosition);
        Vector3 origin = sceneCam.transform.position;
        Vector3 direction = (worldPosition - origin).normalized;

        GameObject hitObject = CheckManualIntersection(origin, direction);

        if (selectionOnClick)
        {
            if (hitObject != null && selectedObject != hitObject) // object hit
            {
                ChangeSelection(hitObject);
            }
            else if (hitObject == null) // no object
            {
                DeselectObject();
            }
        }
        else
        {
            if (hitObject != selectedObject)
            {
                ChangeSelection(hitObject);
            }
        }
    }

    private Vector3 ScreenToWorld(Vector3 screenPos)
    {
        screenPos.z = sceneCam.nearClipPlane;
        return sceneCam.ScreenToWorldPoint(screenPos);
    }

    private GameObject CheckManualIntersection(Vector3 origin, Vector3 direction)
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
                if (LinePlaneIntersection(origin, direction, plane, out Vector3 intersectionPoint))
                {
                    if (bounds.Contains(intersectionPoint))
                    {
                        float distance = Vector3.Distance(origin, intersectionPoint);
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

        // Direction, Point
        planes[0] = new Plane(Vector3.up, center + Vector3.up * extents.y);
        planes[1] = new Plane(Vector3.down, center - Vector3.up * extents.y);
        planes[2] = new Plane(Vector3.forward, center + Vector3.forward * extents.z);
        planes[3] = new Plane(Vector3.back, center - Vector3.forward * extents.z);
        planes[4] = new Plane(Vector3.right, center + Vector3.right * extents.x);
        planes[5] = new Plane(Vector3.left, center - Vector3.right * extents.x);

        return planes;
    }

    private bool LinePlaneIntersection(Vector3 origin, Vector3 direction, Plane plane, out Vector3 intersectionPoint)
    {
        float denominator = Vector3.Dot(plane.normal, direction);
        intersectionPoint = Vector3.zero;

        if (Mathf.Abs(denominator) > 1e-6f) // Prevent division by zero
        {
            float t = -(Vector3.Dot(plane.normal, origin) + plane.distance) / denominator;
            if (t >= 0)
            {
                intersectionPoint = origin + t * direction;
                return true;
            }
        }

        return false;
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

    private void SetObjectMaterial(GameObject obj, Material mat)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = mat;
        }
    }
}
