using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroLogic : MonoBehaviour
{

    public float timer = 1.500f;

    // Use this for initialization
    void Start()
    {

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
