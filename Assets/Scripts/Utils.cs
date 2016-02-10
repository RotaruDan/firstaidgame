using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils : MonoBehaviour
{
    public static PT_XMLReader XML_READER;

    // Use this for initialization
    void Start()
    {
        if(XML_READER == null)
        {
            XML_READER = new PT_XMLReader();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /**
        SCENE
    */

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }


    /**
        FLAGS
    */

    public void ReiniciarFlags()
    {
        Flags.Reininciar();
    }

    public void IniciarAleatoriedadFlags()
    {
        Flags.IniciarAleatoriedad();
    }

    public void ActivateFlag(string flag)
    {
        Flags.Agregar(flag, true);
    }

    public void DeactivateFlag(string flag)
    {
        Flags.Agregar(flag, false);
    }

    /**
      Comma Separated Values - in order to be passed from the UI Event Trigger System.
    */
    public void SetFlag(string flagAndValueCSV)
    {
        string[] values = flagAndValueCSV.Split(',');
        if (values.Length == 2)
        {
            Flags.Agregar(values[0], IntParseFast(values[1]));
        }
    }

    /**
        OTHER PRIVATE UTILITIES
    */

    private int IntParseFast(string value)
    {
        int result = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char letter = value[i];
            result = 10 * result + (letter - 48);
        }
        return result;
    }

}
