using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChokeLogic : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Utils.LoadPhonePrefab(this.gameObject.transform);
        if (Flags.ValorDe("InicioTos"))
        {
            Conversations.PlayConversation("TosNoHaIniciado");
        }
        else
        {
            Invoke("startInitialConversation", 2f);
        }
    }

    public void startInitialConversation()
    {
        // TODO copy to inicioConvoInc and inicioConvoDt
        Conversations.PlayConversation("inicioConvoTos");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
