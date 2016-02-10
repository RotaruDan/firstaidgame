using UnityEngine;
using System.Collections;

public class ChokeLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject phone = (GameObject)Resources.Load("Prefabs/B Telephone", typeof(GameObject));
        Description phoneDescription = phone.GetComponent<Description>();
        phoneDescription.xmlDescription = (TextAsset)Resources.Load("descriptions/desc_telefono", typeof(TextAsset));
        phone.transform.SetParent(this.gameObject.transform);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
