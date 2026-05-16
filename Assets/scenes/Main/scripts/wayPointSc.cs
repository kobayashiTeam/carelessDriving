using UnityEngine;

public class wayPointSc : MonoBehaviour
{
    public float maxSpeed;
    public float reachThreShold;
    public float startBrakeThreShold;
    public int wallIndex;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        //wayPointの到達範囲を表示
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, reachThreShold);

        //wayPointの手前で減速開始する範囲を表示
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, startBrakeThreShold);
    }
}
