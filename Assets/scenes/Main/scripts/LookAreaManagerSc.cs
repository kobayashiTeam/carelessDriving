using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class LookAreaManagerSc : MonoBehaviour
{
    public LookAreaPairSc[] lookAreaPairs;
    private Transform playerTransform;
    void Start()
    {
        // 自分とその子孫にある LookAreaPairSc コンポーネントを全部取得
        lookAreaPairs = GetComponentsInChildren<LookAreaPairSc>(true)
                .Where(c => c.gameObject != this.gameObject)
                .ToArray();

        StartCoroutine(activateLookAreasCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    private IEnumerator activateLookAreasCoroutine()
    {
        foreach (var lookAreaPair in lookAreaPairs)
        {
            lookAreaPair.gameObject.SetActive(true);
            // 次の LookAreaPairSc を有効化するまで少し待つ
            yield return new WaitForSeconds(1f);
        }
        yield return null;
    }

    public IEnumerator deactivateLookAreasCoroutine()
    {
        foreach (var lookAreaPair in lookAreaPairs)
        {
            lookAreaPair.gameObject.SetActive(false);
            Debug.Log("deactivate!");
            // 次の LookAreaPairSc を有効化するまで少し待つ
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }

    public void updateGirlsRotation(Transform playerPos)
    {
        playerTransform = playerPos;
        //キャラクタがPlayerのほうを見る
        foreach (var lookAreaPair in lookAreaPairs)
        {
            Vector3 GirlToPlayer = playerTransform.position -
                lookAreaPair.lookTarget.transform.position;
            GirlToPlayer.y = 0f;
            GirlToPlayer.Normalize();
            lookAreaPair.lookTarget.transform.rotation =
                Quaternion.LookRotation(GirlToPlayer, Vector3.up);
        }
    }
}
