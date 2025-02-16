using UnityEngine;

public class MeshPicker : MonoBehaviour
{
    public Camera sceneCam;
    public Material defaultMaterial;  
    public Material selectedMaterial;  
    private GameObject currentObject;  

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = sceneCam.ScreenPointToRay(mousePosition);

        GameObject hitObject = RayIntersectsObject(ray);


        if (hitObject != null)
        {
            if (currentObject != hitObject)
            {
                if (currentObject != null) //reset previous
                {
                    SetObjectMaterial(currentObject, defaultMaterial);
                }

                currentObject = hitObject;
                SetObjectMaterial(currentObject, selectedMaterial);
            }
        }
        else
        {
            // if none
            if (currentObject != null)
            {
                SetObjectMaterial(currentObject, defaultMaterial);
                currentObject = null;
            }
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

    private GameObject RayIntersectsObject(Ray ray)
    {
        foreach (var obj in GameObject.FindObjectsOfType<MeshRenderer>())
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Mesh mesh = meshFilter.sharedMesh;
                Transform objTransform = obj.transform;

                Vector3[] vertices = mesh.vertices;

                for (int i = 0; i < mesh.triangles.Length; i += 3)
                {
                    Vector3 v0 = objTransform.TransformPoint(vertices[mesh.triangles[i]]);
                    Vector3 v1 = objTransform.TransformPoint(vertices[mesh.triangles[i + 1]]);
                    Vector3 v2 = objTransform.TransformPoint(vertices[mesh.triangles[i + 2]]);

                    RaycastHit hit;
                    if (RayIntersectsTriangle(ray, v0, v1, v2, out hit))
                    {
                        return obj.gameObject;
                    }
                }
            }
        }

        return null;
    }

    //ray-triangle, chapter 6.2.1
    private bool RayIntersectsTriangle(Ray ray, Vector3 v0, Vector3 v1, Vector3 v2, out RaycastHit hit)
    {
        hit = new RaycastHit();

        Vector3 edge1 = v1 - v0;
        Vector3 edge2 = v2 - v0;
        Vector3 h = Vector3.Cross(ray.direction, edge2);
        float a = Vector3.Dot(edge1, h);

        // parallel
        if (Mathf.Abs(a) < 1e-6f)
            return false;

        float f = 1.0f / a;
        Vector3 s = ray.origin - v0;
        float u = f * Vector3.Dot(s, h);

        if (u < 0.0f || u > 1.0f)
            return false;

        Vector3 q = Vector3.Cross(s, edge1);
        float v = f * Vector3.Dot(ray.direction, q);

        if (v < 0.0f || u + v > 1.0f)
            return false;

        // get distance
        float t = f * Vector3.Dot(edge2, q);
        if (t > 1e-6f) // intersection
        {
            hit.point = ray.origin + ray.direction * t;
            hit.normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));
            return true;
        }

        return false;
    }


}
