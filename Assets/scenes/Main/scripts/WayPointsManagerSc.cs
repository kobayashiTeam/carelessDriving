using UnityEngine;

public class WayPointsManagerSc : MonoBehaviour
{
    public GameObject[] waypoints1;
    public GameObject[] waypoints2;
    public GameObject[] waypoints3;

    public GameObject[][] waypoints;

    private void Awake()
    {
        waypoints = new GameObject[][]
        {
            waypoints1,
            waypoints2,
            waypoints3
        };
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
