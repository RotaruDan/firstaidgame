using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils : MonoBehaviour
{
    public static PT_XMLReader XML_READER;

    // Use this for initialization
    void Start()
    {
        GetXMLReader();
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

    internal static PT_XMLReader GetXMLReader()
    {
        if (XML_READER == null)
        {
            XML_READER = new PT_XMLReader();
        }
        return XML_READER;
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
      Prefab Staitc Utilities
    */

    public static GameObject LoadPhonePrefab(Transform transform)
    {
        GameObject phone = Instantiate((GameObject)Resources.Load("Prefabs/B Telephone", typeof(GameObject)));

        Vector3 localPos = phone.transform.localPosition;
        localPos = new Vector3(localPos.x, localPos.y, localPos.z);

        phone.transform.SetParent(transform);
        phone.transform.localPosition = localPos;
        phone.transform.localScale = new Vector3(1, 1, 1);
        return phone;
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
