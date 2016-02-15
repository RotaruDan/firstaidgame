using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsLogic : MonoBehaviour
{
    public bool isRandom;
    public Conversations.Option[] options;
    public UnityEngine.Events.UnityAction<int> optionPressed;

    // Use this for initialization
    void Start()
    {

        if (isRandom)
        {
            reshuffle();
        }
        GameObject textChild = this.gameObject.transform.Find("Button").gameObject;
        textChild.GetComponentInChildren<Button>().onClick.AddListener(
            () => optionPressed(options[0].nextIndex));
        textChild.GetComponentInChildren<Button>().onClick.AddListener(
            () => Destroy());
        Text childTextComp = textChild.GetComponentInChildren<Text>();
        childTextComp.text = "1 - " + options[0].text;
        float prefHeight = childTextComp.preferredHeight;
        Vector3 position = textChild.transform.localPosition;
        position.y = prefHeight * (options.Length - 1) + 0.5f * prefHeight - 300;
        textChild.transform.localPosition = new Vector3(position.x, position.y, position.z);

        for (int i = 1; i < options.Length; ++i)
        {
            GameObject newText = Instantiate(textChild);

            Vector3 localPos = newText.transform.localPosition;
            localPos = new Vector3(localPos.x, localPos.y, localPos.z);
            newText.transform.SetParent(this.gameObject.transform);
            newText.transform.localPosition = localPos;
            newText.transform.localScale = new Vector3(1, 1, 1);

            int nextIndex = options[i].nextIndex;

            newText.GetComponentInChildren<Button>().onClick.AddListener(
                () => optionPressed(nextIndex));
            newText.GetComponentInChildren<Button>().onClick.AddListener(
                () => Destroy());
            newText.GetComponentInChildren<Text>().text = (i + 1) + " - " + options[i].text;
            position.y = position.y - prefHeight;
            newText.transform.localPosition = position;
        }
    }


    public void reshuffle()
    {

        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        int length = options.Length;
        for (int i = 0; i < length; ++i)
        {
            int r = UnityEngine.Random.Range(i, length);

            Conversations.Option indexOption = options[i];
            options[i] = options[r];
            options[r] = indexOption;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
