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
                GetComponent<Image>().overrideSprite = sourceImage;
            }
        }
        else if (sourceImage != telefonoEspera)
        {
            sourceImage = telefonoEspera;
            GetComponent<Image>().overrideSprite = sourceImage;
        }

        bool telEscondido = Flags.ValorDe("TelEscondido");
        Vector3 scale = this.gameObject.transform.localScale;
        if(telEscondido)
        {
            if(scale.x == 1f)
            {
                scale.x = 0f;
                this.gameObject.transform.localScale = scale;
            }
        } else if(scale.x == 0f)
        {
            scale.x = 1f;
            this.gameObject.transform.localScale = scale;
        }
    }

    public void OnClick()
    {

    }
}
