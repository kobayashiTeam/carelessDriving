using UnityEngine;

public class PhoneScript : MonoBehaviour
{
    public Material phoneMaterial;
    public Texture2D texture1;
    public Texture2D texture2;
    public Texture2D texture3;
    public Texture2D texture4;
    public Texture2D texture5;
    public Texture2D texture6;
    public Texture2D texture7;
    public Texture2D texture8;
    public Texture2D texture9;
    public Texture2D texture10;

    public Texture2D[] textures;
    void Start()
    {
        textures = new Texture2D[10] { texture1, texture2, texture3, texture4, 
            texture5, texture6, texture7, texture8, texture9, texture10 };

        phoneMaterial.SetTexture("_EmissionMap", null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeTexture(int currentTextureIndex)
    {
        phoneMaterial.SetTexture("_EmissionMap", textures[currentTextureIndex]);
        phoneMaterial.EnableKeyword("_EMISSION");
        phoneMaterial.SetColor("_EmissionColor", Color.white);
    }

    public void setOffPhoneScreen()
    {
        phoneMaterial.DisableKeyword("_EMISSION");
        phoneMaterial.SetColor("_EmissionColor", Color.black);
        phoneMaterial.SetTexture("_EmissionMap", null);
    }
}
