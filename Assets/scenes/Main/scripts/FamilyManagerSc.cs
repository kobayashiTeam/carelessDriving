using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class FamilyManagerSc : MonoBehaviour
{
    //Course3にまつわる作業
    public Material darkSkybox;
    public Light fieldLight;
    public GameObject FinaObject;
    public PlayerSc playerSc;
    public AudioSource finaSound;

    public FamilyPointSc[] familyPoints;


    void Start()
    {
        // 自分とその子孫にある LookAreaPairSc コンポーネントを全部取得
        familyPoints = GetComponentsInChildren<FamilyPointSc>()
                .Where(c => c.gameObject != this.gameObject).ToArray();

        // familyPoints の各要素に対して初期化を行う
        foreach (FamilyPointSc familyPoint in familyPoints)
        {
            familyPoint.initialize(this);
        }
    }

    public void OnReachedFamilyArea(int currentFamilyIndex)
    {
        if ((currentFamilyIndex+1)>= familyPoints.Length) return;
        finaSound.Play();
        int nextFamilyIndex = currentFamilyIndex + 1;
        Transform finaPos=FinaObject.transform;
        FinaObject.transform.position =
            familyPoints[nextFamilyIndex].FinaPosition.position;
        FinaObject.transform.rotation =
            familyPoints[nextFamilyIndex].FinaPosition.rotation;

        return;
    }
}
