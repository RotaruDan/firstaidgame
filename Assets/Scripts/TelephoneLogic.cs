using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TelephoneLogic : MonoBehaviour
{

    public Sprite telefonoEspera;
    public Sprite hablandoTelefono;

    private Sprite sourceImage;

    // Use this for initialization
    void Start()
    {
        sourceImage = GetComponent<Image>().overrideSprite;
    }

    // Update is called once per frame
    void Update()
    {
        bool hablandoTlf = Flags.ValorDe("HablandoTelefono");
        if (hablandoTlf)
        {
            if (sourceImage != hablandoTelefono)
            {
                sourceImage = hablandoTelefono;
            }
        }
        else if (sourceImage != telefonoEspera)
        {
            sourceImage = telefonoEspera;
        }
    }

    public void OnClick()
    {

    }
}
