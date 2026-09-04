using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class MaterialData
{
    public Texture2D baseTex;
    public Texture2D normalTex;
    public Texture2D specularTex;

    public bool emissionEnabled;
    public float emission;
}

public class MaterialBuilder
{
    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    private static readonly int NormalMap = Shader.PropertyToID("_NormalMap");
    private static readonly int MaskMap = Shader.PropertyToID("_MaskMap");
    private static readonly int EmissiveColorMapProp = Shader.PropertyToID("_EmissiveColorMap");
    private static readonly int EmissiveColorProp    = Shader.PropertyToID("_EmissiveColor");

    public Material ApplyMaps(MaterialData data)
    {
        // Debug.Log(BaseMap + " : " + data.baseTex.name);
        Material material = new(Shader.Find("HDRP/Lit"));
        // Debug.Log(string.Join(',', material.GetPropertyNames(MaterialPropertyType.Float)));
        material.mainTexture = data.baseTex;

        if (data.normalTex)
            material.SetTexture(NormalMap, data.normalTex);
        if (data.baseTex)
            material.SetTexture(MaskMap, data.specularTex);

        if (data.emissionEnabled)
        {
            material.EnableKeyword( "_EMISSION");
            // Color finalIntensityColor = Color.white * data.emission;
            // material.SetColor(EmissiveColorProp, finalIntensityColor);
            material.SetTexture(EmissiveColorMapProp, data.baseTex);
            
            
            // 2. Use HDMaterial to set the Base Emissive Color tint
            // HDMaterial.SetEmissiveColor(material, Color.white);
            //
            // // 3. Use HDMaterial to cleanly pass EV100 units
            // // Options include: EmissiveIntensityUnit.EV100 or EmissiveIntensityUnit.Nits
            // HDMaterial.SetEmissiveIntensity(material, 100, EmissiveIntensityUnit.EV100);
            //
            // // 4. CRITICAL: Re-validate the material structure
            // // This synchronizes hidden keywords and passes so the changes render correctly.
            // HDMaterial.ValidateMaterial(material);
        }
        
        return material;
    }

    public Texture2D[] GetTextures(string texturePath)
    {
        var textures = Resources.LoadAll<Texture2D>(texturePath);
        // Debug.Log(textures.Length);
        return textures;
    }
}