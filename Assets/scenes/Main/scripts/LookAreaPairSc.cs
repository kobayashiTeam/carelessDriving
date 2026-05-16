using System.Linq;
using UnityEngine;

public class LookAreaPairSc : MonoBehaviour
{
    public GameObject lookArea;
    public GameObject lookTarget;
    public int lookAreaIndex;
    void Start()
    {
        // 自分とその子孫にある LookAreaPairSc コンポーネントを全部取得
        LookAreaSc child = GetComponentsInChildren<LookAreaSc>()
    .FirstOrDefault(c => c.gameObject != this.gameObject);
        if (child != null)
        {
            lookAreaIndex = child.lookAreaIndex;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
