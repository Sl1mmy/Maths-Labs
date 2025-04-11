using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainStripGenerator))]
public class TerrainStripGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainStripGenerator generator = (TerrainStripGenerator)target;

        EditorGUI.BeginChangeCheck();

        generator.heightMapImage = (Texture2D)EditorGUILayout.ObjectField("Height Map", generator.heightMapImage, typeof(Texture2D), false);
        generator.heightMultiplier = EditorGUILayout.FloatField("Height Multiplier", generator.heightMultiplier);
        generator.heightCurve = EditorGUILayout.CurveField("Height Curve", generator.heightCurve);
        generator.levelOfDetail = EditorGUILayout.IntSlider("Level of Detail", generator.levelOfDetail, 0, 6);

        if (EditorGUI.EndChangeCheck())
        {
            if (generator.heightMapImage != null)
            {
                generator.GenerateAndApplyMesh();
                EditorUtility.SetDirty(generator);
            }
        }

        if (GUILayout.Button("Regenerate Mesh"))
        {
            generator.GenerateAndApplyMesh();
        }
    }
}
