using System.Linq;
using TMPro;
using UnityEngine;

public class MessageManagerSc : MonoBehaviour
{
    public TextMeshProUGUI messageTextOb;
    public MessageSc[] messages;
    public PlayerSc playerSc;
    private int lastWayPointIndex=-1;
    void Start()
    {
        messages = GetComponentsInChildren<MessageSc>(true)
                .Where(c => c.gameObject != this.gameObject)
                .ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentState != GameState.Playing) return;
        int current=playerSc.currentWaypointIndex;
        if (current != lastWayPointIndex)
        {
            lastWayPointIndex = current;
            handlePlayerReachWayPoint();
        }
        
    }

    public void handlePlayerReachWayPoint()
    {
        foreach (var message in messages)
        {
            if (message.courseIndex == playerSc.currentCourseIndex &&
                message.wayPointIndex == playerSc.currentWaypointIndex)
            {
                
                //ここでメッセージを表示
                if (GameManager.instance.currentLanguage ==
                    GameEnums.LanguageIndex.English)
                {
                    messageTextOb.text = message.EngMessage;
                }
                else if (GameManager.instance.currentLanguage ==
                    GameEnums.LanguageIndex.Japanese)
                {
                    messageTextOb.text = message.JapMessage;
                }
            }
            
        }
    }

    public void clearText()
    {
        messageTextOb.text = "";
    }
}
