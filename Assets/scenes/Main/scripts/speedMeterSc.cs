using TMPro;
using UnityEngine;

public class speedMeterSc : MonoBehaviour
{
    public Rigidbody playerRb;
    private TextMeshProUGUI textMeshPro;
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        updateSpeed();
    }

    void updateSpeed()
    {
        textMeshPro.text = "Speed: " + playerRb.linearVelocity.magnitude.ToString("F1");
    }
}
