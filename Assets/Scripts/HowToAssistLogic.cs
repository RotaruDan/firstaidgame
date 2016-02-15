using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HowToAssistLogic : MonoBehaviour
{

    private GameObject group;

    // Use this for initialization
    void Start()
    {
        group = this.gameObject.transform.FindChild("Group").gameObject;

        Utils.reshuffle(group.transform);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
