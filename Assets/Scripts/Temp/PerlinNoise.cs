using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    [SerializeField] private int width = 256;
    [SerializeField] private int height = 256;
    [SerializeField] private float scale = 20f; //bepaald hoe zoomed in je bent met de perlin noise

    [SerializeField] private float offsetX = 100f;
    [SerializeField] private float offsetY = 100f;


    private void Update()
    {
        var renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    private Texture2D GenerateTexture()
    {
        var texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(x, y, GenerateColer(x,y));
            }
        }

        texture.Apply();
        return texture;
    }

    private Color GenerateColer(int x, int y)
    {
        var xCoord = (float)x / width * scale + offsetX;
        var yCoord = (float)y / height * scale + offsetY;

        var sample = Mathf.PerlinNoise(xCoord, yCoord);

        return new Color (sample,sample,sample);
    }
}
