using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainStripGenerator : MonoBehaviour
{
    public Texture2D heightMapImage;
    public float heightMultiplier = 10f;
    public AnimationCurve heightCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [Range(0, 6)] public int levelOfDetail = 0;

    private void Start()
    {
        GenerateAndApplyMesh();
    }

    public void GenerateAndApplyMesh()
    {
        if (heightMapImage == null)
        {
            Debug.LogError("Heightmap image is missing.");
            return;
        }

        Mesh mesh = GenerateTerrainMesh(heightMapImage, levelOfDetail);
        GetComponent<MeshFilter>().mesh = mesh;

        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mat.mainTexture = heightMapImage;
        GetComponent<MeshRenderer>().material = mat;
    }

    Mesh GenerateTerrainMesh(Texture2D texture, int lod)
    {
        int width = texture.width;
        int height = texture.height;
        Color[] pixels = texture.GetPixels();

        int meshSimplifyIncrement = Mathf.Max(1, lod * 2);
        int meshWidth = (width - 1) / meshSimplifyIncrement + 1;
        int meshHeight = (height - 1) / meshSimplifyIncrement + 1;

        Vector3[] vertices = new Vector3[meshWidth * meshHeight];
        Vector2[] uvs = new Vector2[meshWidth * meshHeight];

        int vIndex = 0;
        for (int y = 0; y < height; y += meshSimplifyIncrement)
        {
            for (int x = 0; x < width; x += meshSimplifyIncrement)
            {
                int pixelIndex = x + y * width;
                float grayscale = pixels[pixelIndex].r;
                float vertexHeight = heightCurve.Evaluate(grayscale) * heightMultiplier;

                float centeredX = x - width / 2f;
                float centeredZ = y - height / 2f;

                vertices[vIndex] = new Vector3(centeredX, vertexHeight, centeredZ);
                uvs[vIndex] = new Vector2((float)x / (width - 1), (float)y / (height - 1));
                vIndex++;
            }
        }

        // Create triangle strip indices
        int stride = meshWidth;
        List<int> indices = new List<int>();
        for (int y = 0; y < meshHeight - 1; y++)
        {
            if (y % 2 == 0)
            {
                for (int x = 0; x < meshWidth; x++)
                {
                    indices.Add(x + y * stride);
                    indices.Add(x + (y + 1) * stride);
                }
            }
            else
            {
                for (int x = meshWidth - 1; x >= 0; x--)
                {
                    indices.Add(x + (y + 1) * stride);
                    indices.Add(x + y * stride);
                }
            }
        }

        // Convert strip to triangles
        List<int> triangles = new List<int>();
        for (int i = 2; i < indices.Count; i++)
        {
            int a = indices[i - 2];
            int b = indices[i - 1];
            int c = indices[i];

            if (a == b || b == c || a == c) continue;

            // Winding order alternates per triangle
            if (i % 2 == 0)
                triangles.AddRange(new int[] { a, b, c });
            else
                triangles.AddRange(new int[] { b, a, c });
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }
}
