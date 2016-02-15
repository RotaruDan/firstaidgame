using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartCoughLogic : MonoBehaviour {

    public MovieTexture videoTexture;
    private GameObject group;

	// Use this for initialization
	void Start () {
        group = this.gameObject.transform.FindChild("Cough Group").gameObject;

        group.transform.FindChild("B Cough 1").
            GetComponentInChildren<Button>().onClick.AddListener(() => playCoughVideo());

        Utils.reshuffle(group.transform);
	}

    void playCoughVideo()
    {
        Utils.PlayFullVideo(videoTexture, () => videoEnded());
    }

    void videoEnded()
    {
        Flags.Agregar("InicioTos", true);
        Utils.LoadLevel("Atragantamiento");
    }


    // Update is called once per frame
    void Update () {
	
	}
}
