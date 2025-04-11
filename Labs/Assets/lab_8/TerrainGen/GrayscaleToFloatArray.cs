using UnityEngine;

public class GrayscaleToFloatArray : MonoBehaviour
{
    public Texture2D grayscaleImage;

    public float[,] ConvertToFloatArray(Texture2D texture)
    {
        int width = texture.width;
        int height = texture.height;

        float[,] floatMap = new float[width, height];
        Color[] pixels = texture.GetPixels();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color pixel = pixels[y * width + x];

                // R = G = B since grayscale
                float grayscaleValue = pixel.r;

                floatMap[x, y] = grayscaleValue;
            }
        }

        return floatMap;
    }

    void Start()
    {
        if (grayscaleImage != null)
        {
            float[,] map = ConvertToFloatArray(grayscaleImage);
            Debug.Log("Grayscale map loaded!");
        }
        else
        {
            Debug.LogWarning("Please assign a grayscale PNG to the script.");
        }
    }
}
