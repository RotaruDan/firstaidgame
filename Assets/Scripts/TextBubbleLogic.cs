using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextBubbleLogic : MonoBehaviour
{

    public UnityEngine.Events.UnityAction bubbleEnded;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    

    public void Destroy()
    {
        Destroy(this.gameObject);
        if(bubbleEnded != null)
        {
            bubbleEnded();
        }
    }
}
