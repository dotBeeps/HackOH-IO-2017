using UnityEngine;

[ExecuteInEditMode]
public class VolumetricImageEffect : MonoBehaviour
{
    public float exposure=0.6f;
    public float decay = 0.95f;
    public float density = 0.96f;
    public float weight = 0.4f;
    public float clamp = 1f;
    public int samples = 16;

    private Material material;

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("Hidden/VolumetricLightApproximation"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material == null)
        {
            material = new Material(Shader.Find("Hidden/VolumetricLightApproximation"));
        }

         material.SetFloat("fExposure", exposure);
        material.SetFloat("fDecay", density);
        material.SetFloat("fDensity", density);
        material.SetFloat("fWeight", weight);
        material.SetFloat("fClamp", clamp);
        material.SetInt("fSamples", samples);

        Graphics.Blit(source, destination, material);
    }
}