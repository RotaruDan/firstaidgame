using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnconsciousLogic : MonoBehaviour
{
    public Sprite sinDesfibrilador, conDesfibrilador, posicionLateral;
    public TextAsset unconsciousPatient, patientIsBreathing,
        changePatientPositionAndWaitTheAmbulance, callAmbulance;

    private Image backgroundImage;



    // Use this for initialization
    void Start()
    {
        Utils.LoadPhonePrefab(this.gameObject.transform);

        backgroundImage = this.gameObject.transform.Find("Background").GetComponent<Image>();
        backgroundImage.sprite = sinDesfibrilador;
        backgroundImage.overrideSprite = sinDesfibrilador;

        // Lógica Inconscienia
        bool comproboRespiracion = Flags.ValorDe("ComproboRespiracion");
        bool posDeSeguridad = Flags.ValorDe("PosicionDeSeguridad");
        if (Flags.ValorDe("Estimulado") &&
            !comproboRespiracion &&
            !posDeSeguridad)
        {
            Conversations.PlayConversation("estimuloNoFunciona");
        }
        else
        {
            if (!comproboRespiracion)
            {
                if (!posDeSeguridad)
                {
                    Invoke("startInitialConversation", 0.5f);
                }
                else
                {
                    bool llamo112 = Flags.ValorDe("Llamo112");
                    if (!llamo112)
                    {
                        Utils.ShowTextBubble(callAmbulance.text, () =>
                        {
                            Invoke("llegaAmbulancia", 2f);
                        });
                    }
                    else
                    {
                        Invoke("llegaAmbulancia", 2f);
                    }
                }
            }
            else
            {
                if (!posDeSeguridad)
                {
                    bool respira = Flags.ValorDe("Respira");
                    if (respira)
                    {
                        Utils.ShowTextBubble(patientIsBreathing.text, () =>
                        {
                            bool ayuda = Flags.ValorDe("Ayuda");
                            if (ayuda)
                            {
                                Utils.ShowTextBubble(
                                    changePatientPositionAndWaitTheAmbulance.text, () =>
                                    {
                                        // Invoke("llegaAmbulancia", 2f);
                                    });
                            }
                            else
                            {
                                // Invoke("llegaAmbulancia", 2f);
                            }
                        });
                    }
                }
                else
                {
                    bool llamo112 = Flags.ValorDe("Llamo112");
                    if (!llamo112)
                    {
                        Utils.ShowTextBubble(callAmbulance.text, () =>
                        {
                            Invoke("llegaAmbulancia", 2f);
                        });
                    }
                    else
                    {
                        Invoke("llegaAmbulancia", 2f);
                    }
                }
            }
        }
    }

    public void llegaAmbulancia()
    {
        Utils.LoadLevel("Final");
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
