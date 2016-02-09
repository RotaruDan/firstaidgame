using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroLogic : MonoBehaviour {

    public float timer = 1.500f;
    //public Texture2d fadeOutTexture;
        public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private int fadeDir = -1;
    private float alpha = 1.0f;
    

	// Use this for initialization
	void Start () {

	}


    void OnGui()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;

        alpha = Mathf.Clamp01(alpha);


    }
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime; // I need timer which from a particular time goes to zero

        if (timer <= 0)
        {
            SceneManager.LoadScene("Inicio");
            timer = 10000f;
        }
    }
}
