using UnityEngine;

public class RayPicker : MonoBehaviour
{
    public Camera sceneCam;
    public Material defaultMaterial;
    public Material selectedMaterial;
    private GameObject selectedObject;

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        GameObject hitObject = CheckObjectUnderMouse(mousePos);

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

    private GameObject CheckObjectUnderMouse(Vector2 mousePos)
    {
        GameObject closestObject = null;
        float closestDepth = float.MaxValue;

        foreach (Renderer renderer in FindObjectsOfType<Renderer>())
        {
            if (IsMouseOverObject(renderer, mousePos, out float objectDepth))
            {
                if (objectDepth < closestDepth)
                {
                    closestDepth = objectDepth;
                    closestObject = renderer.gameObject;
                }
            }
        }

        return closestObject;
    }

    private bool IsMouseOverObject(Renderer renderer, Vector2 mousePos, out float depth)
    {
        depth = float.MaxValue;
        Bounds bounds = renderer.bounds;
        Vector3[] worldCorners = new Vector3[8];

        // Get bounding box corners
        worldCorners[0] = bounds.min;
        worldCorners[1] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        worldCorners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        worldCorners[3] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        worldCorners[4] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        worldCorners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        worldCorners[6] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        worldCorners[7] = bounds.max;

        // Project to screen space
        Vector2 minScreen = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 maxScreen = new Vector2(float.MinValue, float.MinValue);
        float nearestZ = float.MaxValue;

        foreach (Vector3 corner in worldCorners)
        {
            Vector3 screenPoint = sceneCam.WorldToScreenPoint(corner);

            if (screenPoint.z > 0) // Ensure it's in front of the camera
            {
                minScreen = Vector2.Min(minScreen, screenPoint);
                maxScreen = Vector2.Max(maxScreen, screenPoint);
                nearestZ = Mathf.Min(nearestZ, screenPoint.z);
            }
        }

        // Check if the mouse is within the screen-space bounding box
        if (mousePos.x >= minScreen.x && mousePos.x <= maxScreen.x &&
            mousePos.y >= minScreen.y && mousePos.y <= maxScreen.y)
        {
            depth = nearestZ;
            return true;
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
