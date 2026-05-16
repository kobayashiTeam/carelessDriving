using UnityEngine;
using UnityEngine.UI;

public class FocusSc : MonoBehaviour
{
    public Sprite focus1;
    public Sprite focus2;
    public Sprite[] focusSprites;
    public Image focusImage;

    public float interval;
    private int index;
    private float timer;
    void Start()
    {
        focusSprites = new Sprite[2] { focus1,focus2};
        focusImage.sprite = focusSprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            index = (index + 1) % focusSprites.Length;
            focusImage.sprite = focusSprites[index];
        }

    }
}
