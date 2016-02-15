using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Utils : MonoBehaviour
{

    private static PT_XMLReader XML_READER;

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
        LoadLevel(scene);
    }

    public static void LoadLevel(string scene)
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

    public void ShowPlayerBubble(TextAsset text)
    {
        ShowPlayerBubble(text.text);
    }

    public void ShowPlayerBubble(string text)
    {
        ShowTextBubble(text);
    }

    /**
      Comma Separated Values - in order to be passed from the UI Event Trigger System.
    */
    public void DecrementFlag(string flagAndValueCSV)
    {
        string[] values = flagAndValueCSV.Split(',');
        if (values.Length == 2)
        {
            DecrementFlagValue(values[0], IntParseFast(values[1]));
        }
    }

    public void playVideo(MovieTexture texture)
    {
        PlayFullVideo(texture);
    }

    /**
      Comma Separated Values - in order to be passed from the UI Event Trigger System.
    */
    public void IncrementFlag(string flagAndValueCSV)
    {
        string[] values = flagAndValueCSV.Split(',');
        if (values.Length == 2)
        {
            IncrementFlagValue(values[0], IntParseFast(values[1]));
        }
    }

    public static void IncrementFlagValue(string flag, int value)
    {
        int valor = Flags.ValorDeInt(flag);
        Flags.Agregar(flag, valor + value);
    }

    public static void DecrementFlagValue(string flag, int value)
    {
        int valor = Flags.ValorDeInt(flag);
        Flags.Agregar(flag, valor - value);
    }

    /**
      Prefab Staitc Utilities
    */

    public static GameObject LoadPrefab(Transform transform, string prefabName)
    {
        GameObject prefab = Instantiate((GameObject)Resources.Load("Prefabs/" + prefabName,
            typeof(GameObject)));

        Vector3 localPos = prefab.transform.localPosition;
        localPos = new Vector3(localPos.x, localPos.y, localPos.z);

        prefab.transform.SetParent(transform);

        prefab.transform.localPosition = localPos;
        prefab.transform.localScale = new Vector3(1, 1, 1);
        return prefab;
    }

    public static GameObject LoadPhonePrefab(Transform transform)
    {
        return LoadPrefab(transform, "B Telephone");
    }

    public static GameObject LoadActionsPrefab(Transform transform)
    {
        return LoadPrefab(transform, "Actions");
    }

    public static void ShowTextBubble(string text)
    {
        ShowTextBubble(text, null);
    }

    public static void ShowTextBubble(string text, UnityAction bubbleEnded)
    {
        ShowTextBubble(text, bubbleEnded, false);
    }

    public static void ShowTextBubble(string text, UnityAction bubbleEnded, bool isPhone)
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject textBubble = LoadPrefab(canvas.transform, "Text Bubble");
        textBubble.GetComponentInChildren<Text>().text = text;
        if (isPhone)
        {
            textBubble.transform.Find("Text Bubble panel").transform.localPosition =
                new Vector3(240, -40, 0f);
        }
        if (bubbleEnded != null)
        {
            textBubble.GetComponent<TextBubbleLogic>().bubbleEnded = bubbleEnded;
        }
    }

    public static void ShowOptions(Conversations.Option[] options,
        UnityAction<int> optionPressed, bool isRandom)
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject textBubble = LoadPrefab(canvas.transform, "Text Options");
        OptionsLogic optionsLogic = textBubble.GetComponent<OptionsLogic>();
        optionsLogic.options = options;
        optionsLogic.isRandom = isRandom;
        if (optionPressed != null)
        {
            optionsLogic.optionPressed = optionPressed;
        }
    }

    public static void PlayFullVideo(MovieTexture texture)
    {
        PlayFullVideo(texture, null);
    }

    public static void PlayFullVideo(MovieTexture texture, UnityAction call)
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject textBubble = LoadPrefab(canvas.transform, "V VideoTexture");
        PlayVideo playVideo = textBubble.GetComponentInChildren<PlayVideo>();
        playVideo.videoEnded = call;
        playVideo.playVideo(texture);
    }


    public static void reshuffle(Transform transform)
    {

        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        int i = 0;
        int length = transform.childCount;
        foreach (Transform child in transform)
        {
            int r = UnityEngine.Random.Range(i, length);

            Transform indexChild = transform.GetChild(i);
            Vector3 indexChildPosition = indexChild.position;
            Vector3 indexChildLocalPosition = indexChild.localPosition;
            Vector3 indexChildScale = indexChild.localScale;

            Transform randomChild = transform.GetChild(r);

            indexChild.position = randomChild.position;
            indexChild.localPosition = randomChild.localPosition;
            indexChild.localScale = randomChild.localScale;

            randomChild.position = indexChildPosition;
            randomChild.localPosition = indexChildLocalPosition;
            randomChild.localScale = indexChildScale;

            ++i;
        }
    }

    /**
        OTHER UTILITIES
    */

    public static int IntParseFast(string value)
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
