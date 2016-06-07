using UnityEngine;
using System.Collections;

public class ImageEffect : MonoBehaviour
{
    public Shader shader;
    public RenderTexture renderTexture;
    Material material;

    void Awake()
    {
        material = new Material(shader);
        if (renderTexture == null)
            renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, renderTexture, material);
        Graphics.Blit(source, destination);
    }
}
