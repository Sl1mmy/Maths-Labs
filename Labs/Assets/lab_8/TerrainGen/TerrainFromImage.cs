using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainFromImage : MonoBehaviour
{
    public Texture2D heightMapImage;
    public float heightMultiplier = 50f;
    public AnimationCurve heightCurve;

    [Range(3, 20)]
    public int levelOfDetail = 0;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;


    void Start()
    {
        CreateTerrain();
    }

    public void CreateTerrain()
    {
        if (heightMapImage == null)
        {
            Debug.LogError("No heightmap image assigned.");
            return;
        }

        // Get or assign components here
        if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();

        float[,] heightMap = ConvertTextureToHeightMap(heightMapImage);

        MeshData meshData = MeshGenerator.GenerateTerrainMesh(heightMap, heightMultiplier, heightCurve, levelOfDetail);
        Mesh terrainMesh = meshData.CreateMesh();

        meshFilter.mesh = terrainMesh;

        Material material = new Material(Shader.Find("Unlit/Texture"));
        material.mainTexture = heightMapImage;
        meshRenderer.material = material;
    }


    float[,] ConvertTextureToHeightMap(Texture2D texture)
    {
        int width = texture.width;
        int height = texture.height;
        float[,] heightMap = new float[width, height];

        Color[] pixels = texture.GetPixels();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float grayscale = pixels[y * width + x].r; // r = g = b
                heightMap[x, y] = grayscale;
            }
        }

        return heightMap;
    }
}
