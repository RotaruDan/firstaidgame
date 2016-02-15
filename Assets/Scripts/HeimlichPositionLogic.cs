﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeimlichPositionoLogic : MonoBehaviour
{

    public MovieTexture videoTexture;
    private GameObject group;

    // Use this for initialization
    void Start()
    {
        group = this.gameObject.transform.FindChild("Heimlich Group").gameObject;

        Utils.reshuffle(group.transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
