using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        CheckActive("TerminoINC", new string[] { "B Summary INC", "I Score INC" });
        CheckActive("TerminoAT", new string[] { "B Summary AT", "I Score AT" });
        CheckActive("TerminoDT", new string[] { "B Summary DT", "I Score DT" });


        CheckScore("NotaAT", "I Score AT");
        CheckScore("NotaDT", "I Score DT");
        CheckScore("NotaINC", "I Score INC");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CheckActive(string flagName, string[] gameObjectNames)
    {
        bool active = Flags.ValorDe(flagName);
        for (int i = 0; i < gameObjectNames.Length; ++i)
        {
            Transform transform = this.gameObject.transform.Find(gameObjectNames[i]);
            if (transform != null)
            {
                transform.gameObject.SetActive(active);
            }
        }
    }

    private void CheckScore(string flagName, string imageName)
    {
        Transform transform = this.gameObject.transform.Find(imageName);
        if(!transform.gameObject.activeSelf)
        {
            return;
        }
        int score = Flags.ValorDeInt(flagName);

        if (score < 1)
        {
            score = 1;
        }

        if (score > 10)
        {
            score = 10;
        }
        
        transform.GetComponent<Image>().overrideSprite = Resources.Load<Sprite>("ScoreImages/" + score);
    }
}
