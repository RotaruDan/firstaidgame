using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StimulateLogic : MonoBehaviour {

    public MovieTexture videoTexture;
    private GameObject group;

	// Use this for initialization
	void Start () {
        group = this.gameObject.transform.FindChild("Cough Group").gameObject;

        group.transform.FindChild("B Cough 1").
            GetComponentInChildren<Button>().onClick.AddListener(() => playVideo());

        Utils.reshuffle(group.transform);
	}

    void playVideo()
    {
        Utils.PlayFullVideo(videoTexture, () => videoEnded());
    }

    void videoEnded()
    {
        Flags.Agregar("Estimulado", true);
        Utils.LoadLevel("Inconsciente");
        if (!Flags.ValorDe("DT")) {
            Utils.IncrementFlagValue("NotaINC", 2);
        }
    }


    // Update is called once per frame
    void Update () {
	
	}
}
