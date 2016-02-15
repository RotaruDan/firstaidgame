using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeadPositionLogic : MonoBehaviour
{
    public TextAsset correctHeadPosition;
    public MovieTexture mirarOirSentirVideo;
    // Use this for initialization
    void Start()
    {
        Transform opt3 = this.gameObject.transform.Find("Option 3"); ;
        bool isHeadOrientationScene = opt3 == null;
        this.gameObject.transform.Find("Option 2").GetComponent<Button>().onClick
            .AddListener(() =>
            {
                
                
                if (!Flags.ValorDe("DT"))
                {
                    Utils.IncrementFlagValue("NotaINC", 3);
                }
                else
                {
                    Utils.IncrementFlagValue("NotaDT", 2);
                }
                if(isHeadOrientationScene)
                {
                    Flags.Agregar("ComproboRespiracion", true);
                    // Flags.Agregar("Respira", Random.value > 0.5f);
                    Flags.Agregar("Respira", true);
                    Utils.ShowTextBubble(correctHeadPosition.text, () =>
                    {
                        Utils.PlayFullVideo(mirarOirSentirVideo, () =>
                        {
                            Utils.LoadLevel("Inconsciente");
                        });
                    });
                } else
                {
                    string[] texts = correctHeadPosition.text.Split('|');
                    Utils.ShowTextBubble(texts[0], () =>
                    {
                        Utils.ShowTextBubble(texts[1], () =>
                        {
                            Utils.LoadLevel("DondeMirar");
                        });
                    });
                }
            });

        this.gameObject.transform.Find("Option 1").GetComponent<Button>().onClick
           .AddListener(() => wrongOption());
        if (!isHeadOrientationScene)
        {
            opt3.GetComponent<Button>().onClick
               .AddListener(() => wrongOption());
        }
    }

    void wrongOption()
    {
        if (!Flags.ValorDe("DT"))
        {
            Utils.DecrementFlagValue("NotaINC", 2);
        }
        else
        {
            Utils.DecrementFlagValue("NotaDT", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
