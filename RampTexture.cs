using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName="RampTexture.asset", menuName="Ramp Texture", order=310)]
public class RampTexture : ScriptableObject
{
    public int width = 256;
    public int heightPerRamp = 2;

    public Gradient[] gradients = new Gradient[]{};

    public int count
    {
        get {return gradients.Length;}
    }

    public Texture2D texture = null;

#if UNITY_EDITOR
    public void UpdateTexture()
    {
        texture = null;
        string assetPath = AssetDatabase.GetAssetPath(this);
        var assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
        foreach (var asset in assets)
        {
            if (asset is Texture2D)
            {
                texture = asset as Texture2D;
                break;
            }
        }

        if (texture == null)
        {
            texture = new Texture2D(4, 4, TextureFormat.ARGB32, false);
            texture.name = "RampTexture4x4";
            AssetDatabase.AddObjectToAsset(texture, this);
            
        }
        
        if (width <= 0 || heightPerRamp <= 0)
        {
            texture.Reinitialize(width, heightPerRamp * count, TextureFormat.ARGB32, false);
            texture.name = "RampTexture4x4";
        }
        else
        {
            texture.Reinitialize(width, heightPerRamp*count, TextureFormat.ARGB32, false);
            texture.name = $"RampTexture{width}x{heightPerRamp*count}";
            Color[] colors = new Color[width * heightPerRamp * count];
            for (int i=0; i<count; ++i)
            {
                var gradient = gradients[i];
                for (int j=0; j<width; ++j)
                {
                    float t = (j+0.5f) / width;
                    Color color = gradient.Evaluate(t);         
                    for (int k=0; k<heightPerRamp; ++k)
                    {
                        colors[(i*heightPerRamp+k)*width+j] = color;
                    }
                }
            }
            texture.SetPixels(colors);
        }
        texture.Apply();
        AssetDatabase.SaveAssets();
    }
#endif
}