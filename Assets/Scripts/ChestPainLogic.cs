using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChestPainLogic : MonoBehaviour
{
    public Sprite dolorToracico, sentado, inconsciente;
    public TextAsset unconsciousPatient;

    private Image backgroundImage;



    // Use this for initialization
    void Start()
    {
        Utils.LoadPhonePrefab(this.gameObject.transform);

        backgroundImage = this.gameObject.transform.Find("Background").GetComponent<Image>();
        backgroundImage.sprite = dolorToracico;
        backgroundImage.overrideSprite = dolorToracico;

        if (Flags.ValorDe("Sentado"))
        {
            Conversations.PlayConversation("esperarAmbulanciaSentado");
            Invoke("inconsciencia", 3f);
        }
        else
        {
            Invoke("startInitialConversation", 0.5f);
        }
    }

    public void startInitialConversation()
    {
        Conversations.PlayConversation("inicioConvoDt");
    }

    public void inconsciencia()
    {
        Flags.Agregar("INC", true);
        Flags.Agregar("Estimulado", true);
        Utils.ShowTextBubble(unconsciousPatient.text, alertMessageEnded);
    }

    public void alertMessageEnded()
    {
        Conversations.PlayConversation("ConversationDTInconsciente");
    }

    // Update is called once per frame
    void Update()
    {
        bool sentado = Flags.ValorDe("Sentado");
        if (!sentado)
        {
            if(backgroundImage.overrideSprite != dolorToracico)
            {
                backgroundImage.overrideSprite = this.dolorToracico;
            }
        } else
        {
            bool inc = Flags.ValorDe("INC");
            
            if(!inc)
            {
                if (backgroundImage.overrideSprite != this.sentado)
                {
                    backgroundImage.overrideSprite = this.sentado;
                }
            } else
            {
                if (backgroundImage.overrideSprite != inconsciente)
                {
                    backgroundImage.overrideSprite = this.inconsciente;
                }
            }

        }

    }
}
