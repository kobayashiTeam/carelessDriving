using UnityEngine;
using UnityEngine.Rendering;

public class LightingManagerSc : MonoBehaviour
{
    public Material nightSkybox;
    public Material daySkybox;
    public Light fieldLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeToNightLighting()
    {

        //ライトを消す
        fieldLight.enabled = false;
        // Skyboxを変更
        RenderSettings.skybox = nightSkybox;
        //skyboxからの環境光を消す
        RenderSettings.ambientLight = Color.black;
        //環境リフレクション
        // Skyboxではなく、カスタムリフレクションを使用する設定に変更
        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
        // カスタムのCubemapをセット
        RenderSettings.customReflectionTexture = null;

        // Skyboxを変更したら、反映のためにライティングを更新する
        DynamicGI.UpdateEnvironment();
    }

    public void changeToDayLighting()
    {
        //ライトをつける
        fieldLight.enabled = true;
        // Skyboxを変更
        RenderSettings.skybox = daySkybox;
        //環境光の設定をスカイボックス参考に
        RenderSettings.ambientMode = AmbientMode.Skybox;
        //環境リフレクションもスカイボックス
        //RenderSettings.defaultReflectionMode =;
        RenderSettings.defaultReflectionMode = 
            UnityEngine.Rendering.DefaultReflectionMode.Skybox;
        // カスタムのCubemapをセット
        //RenderSettings.customReflectionTexture =;

        // Skyboxを変更したら、反映のためにライティングを更新する
        DynamicGI.UpdateEnvironment();
    }
}
