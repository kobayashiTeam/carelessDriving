using System;
using UnityEngine;

public class EnemySc : MonoBehaviour
{
    private GameObject[] waypoints;
    public WayPointsManagerSc wayPointsManagerSc;
    public int currentWaypointIndex;
    Rigidbody rb;
    private bool isBrakeMode = false;//減速開始領域にいるかどうか
    public GameEnums.CourseIndex currentCourseIndex =
        GameEnums.CourseIndex.Course1;//現在のコースインデックス

    [Header("車の設定")]
    public float accelPower;// 前進速度
    public float steerSpeed;// Y軸回転速度
    public float angleThreshold;// 方向転換開始角度閾値
    public float inCourseMaxSpeed;// 最大速度
    public float outCourseMaxSpeed;// コース外最大速度

    //enemyゴール時
    public event Action OnEnemyGoal;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        //自前のwayPoints配列に受け継ぐ
        waypoints = new GameObject[wayPointsManagerSc.waypoints[(int)currentCourseIndex].Length];
        for (int i = 0; i < wayPointsManagerSc.waypoints[(int)currentCourseIndex].Length; i++)
        {
            waypoints[i] = wayPointsManagerSc.waypoints[(int)currentCourseIndex][i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkBrakeMode();
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.currentState == GameState.Playing)
        {
            moveCar();
        }

        if(GameManager.instance.currentState == GameState.GameOver)
        {
            //減速
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, 0.1f);
        }
    }

    void checkBrakeMode()
    {
        //減速開始領域に入ったかどうか
        float distanceToWaypoint = Vector3.Distance(transform.position,
            waypoints[currentWaypointIndex].transform.position);
        if (distanceToWaypoint < waypoints[currentWaypointIndex].
            GetComponent<wayPointSc>().startBrakeThreShold)
        {
            isBrakeMode = true;
        }
        else
        {
            isBrakeMode = false;
        }
    }

    void moveCar()
    {
        Vector3 dir = (waypoints[currentWaypointIndex].transform.position -
            transform.position).normalized;
        dir.y = 0; //水平面のみでの方向ベクトルにする

        // Y軸回転（左右）
        Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        Quaternion smoothRotation = Quaternion.RotateTowards(rb.rotation, targetRotation, 
            steerSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(smoothRotation);
        // 前進
        float angleDiff=Vector3.Angle(transform.forward, dir);
        if (angleDiff < angleThreshold)
        {
            rb.AddForce(transform.forward * accelPower, ForceMode.Force);
        }
        //速度制限
        if (rb.linearVelocity.magnitude > inCourseMaxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * inCourseMaxSpeed;
        }
        //減速開始領域にいれば、規定の最大速度までLerp的に減速
        if (isBrakeMode)
        {
            float currentMaxSpeed = waypoints[currentWaypointIndex].
                GetComponent<wayPointSc>().maxSpeed;

            if (rb.linearVelocity.magnitude > currentMaxSpeed)
            {
                rb.linearVelocity = Vector3.Lerp(rb.linearVelocity,
                    rb.linearVelocity.normalized * currentMaxSpeed,
                    0.1f);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // レイヤー名で判定
        if (other.gameObject.layer == LayerMask.NameToLayer("CheckPoint"))
        {
            wayPointSc wayPointScript = other.gameObject.GetComponent<wayPointSc>();
            if (wayPointScript.wallIndex == currentWaypointIndex)
            {
                currentWaypointIndex++;
            }

            //コースの最後の waypoint に到達した場合の処理
            if (currentWaypointIndex > waypoints.Length-1)
            {
                currentWaypointIndex = 0;
                currentCourseIndex++;
                //最期のコースでなければwayPointの配列を更新
                if (currentCourseIndex<=GameEnums.CourseIndex.Course3)
                {
                    reassignWaypointsArray();
                }
                if (currentCourseIndex > GameEnums.CourseIndex.Course3)
                {
                    OnEnemyGoal?.Invoke();
                }
            }
            
        }
    }

    public void reassignWaypointsArray()
    {
        waypoints = new GameObject[wayPointsManagerSc.
                    waypoints[(int)currentCourseIndex].Length];
        for (int i = 0; i < wayPointsManagerSc.
            waypoints[(int)currentCourseIndex].Length; i++)
        {
            waypoints[i] = wayPointsManagerSc.
                waypoints[(int)currentCourseIndex][i];
        }
    }
}
