using UnityEngine;

[ExecuteInEditMode]
public class HueShift : MonoBehaviour
{
    public Material effectMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, effectMaterial);
    }
}