using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnconsciousLogic : MonoBehaviour
{
    public Sprite sinDesfibrilador, conDesfibrilador, posicionLateral;
    public TextAsset unconsciousPatient;

    private Image backgroundImage;



    // Use this for initialization
    void Start()
    {
        Utils.LoadPhonePrefab(this.gameObject.transform);

        backgroundImage = this.gameObject.transform.Find("Background").GetComponent<Image>();
        backgroundImage.sprite = sinDesfibrilador;
        backgroundImage.overrideSprite = sinDesfibrilador;

        if (Flags.ValorDe("Estimulado"))
        {
            Conversations.PlayConversation("estimuloNoFunciona");
        }
        else
        {
            Invoke("startInitialConversation", 0.5f);
        }
    }

    public void startInitialConversation()
    {
        Conversations.PlayConversation("inicioConvoInc");
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

        bool desfiColocado = Flags.ValorDe("DesfibriladorColocado");
        bool posicionDeSeguridad = Flags.ValorDe("PosicionDeSeguridad");

        if (!desfiColocado && !posicionDeSeguridad)
        {
            if (backgroundImage.overrideSprite != sinDesfibrilador)
            {
                backgroundImage.overrideSprite = sinDesfibrilador;
            }
        }
        else if (desfiColocado)
        {
            if (backgroundImage.overrideSprite != conDesfibrilador)
            {
                backgroundImage.overrideSprite = conDesfibrilador;
            }
        }
        else
        {
            if (backgroundImage.overrideSprite != posicionLateral)
            {
                backgroundImage.overrideSprite = posicionLateral;
            }
        }

    }
}
