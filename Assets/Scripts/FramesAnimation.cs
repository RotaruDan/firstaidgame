using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FramesAnimation : MonoBehaviour
{
    public Sprite sprite1, sprite2;
    public float spriteTime = 0.800f;

    private float time;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        time = spriteTime;
        image.sprite = sprite1;
        image.overrideSprite = sprite1;
    }

    void Update()
    {
        time -= Time.deltaTime;
        
        if (time < 0)
        {
            time = spriteTime;
            image.overrideSprite = image.overrideSprite == sprite1 ? sprite2 : sprite1;
        }

    }
}
