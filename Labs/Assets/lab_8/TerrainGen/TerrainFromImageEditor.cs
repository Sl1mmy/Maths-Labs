using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainFromImage))]
public class TerrainFromImageEditor : Editor
{
    private TerrainFromImage terrainFromImage;

    private void OnEnable()
    {
        terrainFromImage = (TerrainFromImage)target;
    }

    public override void OnInspectorGUI()
    {
        // Draw default inspector (all serialized fields)
        DrawDefaultInspector();

        // Check if any value has been changed in the inspector
        if (GUI.changed)
        {
            // Call CreateTerrain() when the user is done editing the values
            terrainFromImage.CreateTerrain();
        }
    }
}
