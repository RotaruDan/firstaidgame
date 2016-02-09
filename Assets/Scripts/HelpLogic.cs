using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HelpLogic : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToMainMenu()
    {
        Image image = GetComponent<Image>();
        
        if (image != null)
        {
            Sprite sprite = image.overrideSprite;
            if (sprite != null && sprite.name == "Ayuda2")
            {
                SceneManager.LoadScene("Inicio"); // TODO change to Main Menu
            }
        }
    }
}
