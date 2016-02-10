using UnityEngine;
using System.Collections;

public class MenuLogic : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        CheckActive("TerminoINC", new string[] { "B Summary INC", "I Score INC" });
        CheckActive("TerminoAT", new string[] { "B Summary AT", "I Score AT" });
        CheckActive("TerminoDT", new string[] { "B Summary DT", "I Score DT" });
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
}
