using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LicenseLogic : MonoBehaviour
{

    public Sprite secondLicenseSprite;
    public Sprite firstLicenseSprite;

    private GameObject backButton;
    private Image background;

    // Use this for initialization
    void Start()
    {
        Debug.Log("1");
        backButton = transform.Find("B Back").gameObject;
        backButton.SetActive(false);

        background = transform.Find("Background").gameObject.GetComponent<Image>();
        background.overrideSprite = firstLicenseSprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextButtonOnMouseDown()
    {
        Debug.Log("1");
        if (!backButton.activeSelf)
        {
            Debug.Log("2");
            backButton.SetActive(true);
            background.overrideSprite = secondLicenseSprite;
        } else
        {
            Debug.Log("3");
            SceneManager.LoadScene("Inicio");
        }
    }

    public void BackButtonOnMouseDown()
    {
        Debug.Log("4");
        backButton.SetActive(false);
        background.overrideSprite = firstLicenseSprite;
    }
}
