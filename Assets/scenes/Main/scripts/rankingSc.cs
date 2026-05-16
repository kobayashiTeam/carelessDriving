using TMPro;
using UnityEngine;

public class rankingSc : MonoBehaviour
{
    TextMeshProUGUI textMeshPro;
    public GameObject playerObject;//playerオブジェクト自体
    public GameObject[] enemyObjects;//enemyオブジェクト自体の配列
    public WayPointsManagerSc wayPointsManagerSc;
    private PlayerSc playerScript;//スクリプト
    private EnemySc[] enemyScripts;//スクリプトの配列
    private int playerRank=0;
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        playerScript = playerObject.GetComponent<PlayerSc>();
        enemyScripts = new EnemySc[enemyObjects.Length];
        for (int i = 0; i < enemyObjects.Length; i++)
        { 
            enemyScripts[i] = enemyObjects[i].GetComponent<EnemySc>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.currentState!=GameState.GameOver)
        checkRank();
        updateRanking();
    }

    void updateRanking()
    {
        textMeshPro.text = "Rank: " + playerRank.ToString();
    }

    void checkRank()
    {
        playerRank = 1;

        //異なるcourse
        foreach (var singleEnemy in enemyScripts)
        {
            if ((int)singleEnemy.currentCourseIndex > (int)playerScript.currentCourseIndex)
            {
                playerRank++;
            }
        }

        //同じcourse内のwaypointIndex
        foreach (var singleEnemy in enemyScripts)
        {
            if (singleEnemy.currentCourseIndex==playerScript.currentCourseIndex) {
                if (singleEnemy.currentWaypointIndex > playerScript.currentWaypointIndex)
                {
                    playerRank++;
                }
            }
        }
        //同じcourse内の同じwayPointIndex内の法線距離での順位
        Vector3 wallNormal = wayPointsManagerSc.waypoints[(int)playerScript.currentCourseIndex]
            [playerScript.currentWaypointIndex].transform.forward;
        Vector3 wallToPlayer = playerObject.transform.position -
            wayPointsManagerSc.waypoints[(int)playerScript.currentCourseIndex]
            [playerScript.currentWaypointIndex].transform.position;

        float signedDistance = Vector3.Dot(wallToPlayer, wallNormal);

        //checkWallからの法線距離を比較して順位を決定
        for(int i = 0; i < enemyScripts.Length; i++)
        {
            if (enemyScripts[i].currentCourseIndex != playerScript.currentCourseIndex) continue;
            if(enemyScripts[i].currentWaypointIndex == playerScript.currentWaypointIndex)
            {
                Vector3 wallToEnemy = enemyObjects[i].transform.position -
            wayPointsManagerSc.waypoints[(int)playerScript.currentCourseIndex]
            [playerScript.currentWaypointIndex].transform.position;
                float enemySignedDistance = Vector3.Dot(wallToEnemy, wallNormal);
                if (enemySignedDistance < signedDistance)
                {
                    playerRank++;
                }
            }
        }

    }
}
