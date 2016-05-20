using UnityEngine;
using System.Collections;
using System;

public class TextureUtility : MonoBehaviour
{

    public static Texture2D Crop(Texture2D input)
    {
        return Crop(input, input.height, input.height);
    }

    public static Texture2D Crop(Texture2D input, int width, int height)
    {
        var output = new Texture2D(width, height);
        var pixels = input.GetPixels(input.width / 2 - width / 2, input.height / 2 - height / 2, width, height);
        output.SetPixels(0, 0, width, height, pixels);
        output.Apply();
        return output;
    }
}
