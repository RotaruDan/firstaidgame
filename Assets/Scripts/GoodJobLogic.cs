using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoodJobLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.gameObject.transform.Find("Background").GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                if(Flags.ValorDe("DT"))
                {
                    Utils.LoadLevel("ResumenDT");
                } else if(Flags.ValorDe("INC"))
                {
                    Utils.LoadLevel("ResumenINC");
                } else
                {
                    Utils.LoadLevel("ResumenAT");
                }
            });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
