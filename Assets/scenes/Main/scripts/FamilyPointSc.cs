using System.Linq;
using UnityEngine;
using static PlayerSc;

public class FamilyPointSc : MonoBehaviour
{
    public int familyPointIndex;
    public Transform FinaPosition;
    private FamilyManagerSc familyManagerScript;
    void Start()
    {
        // 自分とその子孫にある LookAreaPairSc コンポーネントを全部取得
        Transform child = GetComponentsInChildren<Transform>()
    .FirstOrDefault(c => c.gameObject != this.gameObject);
        if (child != null)
        {
            FinaPosition = child.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initialize(FamilyManagerSc _familyManagerSc)
    {
        familyManagerScript = _familyManagerSc;
    }

    void OnTriggerEnter(Collider other)
    {
        // playerがfamilyAreaに到達。
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("FamilyPointSc");
            familyManagerScript.OnReachedFamilyArea(familyPointIndex );

        }
    }
}
