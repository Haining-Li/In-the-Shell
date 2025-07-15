using UnityEngine;
using System.Collections;

public class URPTransparencyController : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    private Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        SetMaterialTransparent(material);
        SetAlpha(material, 1f); 
    }

    public void FadeTo(float targetAlpha)
    {
        StartCoroutine(FadeCoroutine(targetAlpha));
    }

    private IEnumerator FadeCoroutine(float targetAlpha)
    {
        float startAlpha = material.GetColor("_Color").a;
        float time = 0f;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            SetAlpha(material, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(material, targetAlpha);
    }

    private void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Surface", 1); // Transparent
        mat.SetFloat("_Blend", 0);   // Alpha blending
        mat.SetFloat("_ZWrite", 0);
        mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    private void SetAlpha(Material mat, float alpha)
    {
        Color color = mat.GetColor("_Color");
        color.a = Mathf.Clamp01(alpha);
        mat.SetColor("_Color", color);
    }
}
