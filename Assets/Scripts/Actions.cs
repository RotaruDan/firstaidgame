using System;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Actions : MonoBehaviour, IPointerClickHandler
{
    public TextAsset xmlActions;

    private Canvas canvas;
    private Camera cam;
    private GameObject actionsPrefab, actionsControl;

    // Use this for initialization
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        actionsPrefab = Utils.LoadActionsPrefab(canvas.transform);

    }

    void ShowActionsControl()
    {
        if (actionsControl != null)
        {
            Destroy(actionsControl);
        }

        actionsControl = Instantiate(actionsPrefab);
        actionsControl.transform.SetParent(canvas.transform);

        ReadActions(xmlActions.text);

        Graphic[] graphics = actionsControl.GetComponentsInChildren<Graphic>();

        for (int i = 0; i < graphics.Length; ++i)
        {
            graphics[i].CrossFadeAlpha(0f, 0f, false);
            graphics[i].CrossFadeAlpha(1f, 1.5f, false);
        }

        Vector3 pos = TransformUtils.CalculatePositionFromMouseToRectTransform(canvas, cam);

        actionsControl.transform.localScale = new Vector3(1, 1, 1);
        actionsControl.transform.position = pos;

    }

    void ReadActions(string text)
    {

        XmlDocument xmld = new XmlDocument();
        xmld.LoadXml(text);

        XmlNodeList actions = xmld.SelectNodes("actions/*");

        for (int i = 0; i < actions.Count; ++i)
        {
            XmlNode actionNode = actions[i];

            string nodename = actionNode.Name;


            XmlNode condition = actionNode.SelectSingleNode("condition");

            XmlNodeList activeFlags = condition.SelectNodes("active");
            bool continueLoop = false;
            for (int j = 0; j < activeFlags.Count; ++j)
            {
                if (!Flags.ValorDe(activeFlags[j].Attributes["flag"].Value))
                {
                    continueLoop = true;
                    break;
                }
            }
            if (continueLoop)
            {
                continue;
            }

            XmlNodeList inactiveFlags = condition.SelectNodes("inactive");
            for (int j = 0; j < inactiveFlags.Count; ++j)
            {
                if (Flags.ValorDe(inactiveFlags[j].Attributes["flag"].Value))
                {
                    continueLoop = true;
                    break;
                }
            }
            if (continueLoop)
            {
                continue;
            }

            GameObject actionButton = Utils.LoadPrefab(actionsControl.transform, "Action Button");
            Description desc = actionButton.AddComponent<Description>();

            Button customButton = actionButton.GetComponent<Button>();

            customButton.transition = Selectable.Transition.SpriteSwap;

            string buttonNormalUri = "";
            string buttonOverUri = "";
            if (nodename == "custom")
            {
                XmlNodeList assets = actionNode.SelectNodes("resources/asset");
                buttonNormalUri = assets[2].Attributes["uri"].Value.Replace(".png", "");
                buttonOverUri = assets[0].Attributes["uri"].Value.Replace(".png", "");
                desc.SetDescription(actionNode.Attributes["name"].Value);
            }
            else if (nodename == "use")
            {
                buttonNormalUri = "gui/buttons/Use-normal";
                buttonOverUri = "gui/buttons/Use-pressed";
                desc.SetDescription("Use");

            }
            else if (nodename == "talk-to")
            {
                buttonNormalUri = "gui/buttons/Talk-normal";
                buttonOverUri = "gui/buttons/Talk-pressed";
                desc.SetDescription("Talk");

            }
            else if (nodename == "talk-to")
            {
                buttonNormalUri = "gui/buttons/Talk-normal";
                buttonOverUri = "gui/buttons/Talk-pressed";

            }

            // buttonNormal
            Image image = actionButton.GetComponent<Image>();
            Sprite normalSprite = Resources.Load<Sprite>(buttonNormalUri);
            image.sprite = normalSprite;
            image.overrideSprite = normalSprite; ;
            image.type = Image.Type.Simple;
            image.preserveAspect = true;
            image.SetNativeSize();

            // buttonOver
            SpriteState state = customButton.spriteState;
            Sprite buttonOverSprite = Resources.Load<Sprite>(buttonOverUri);
            state.highlightedSprite = buttonOverSprite;
            state.disabledSprite = state.highlightedSprite;
            state.pressedSprite = state.highlightedSprite;
            customButton.spriteState = state;

            XmlNode node = actionNode.SelectSingleNode("effect");


            XmlNode triggerScene = node.SelectSingleNode("trigger-scene");
            if (triggerScene != null)
            {
                customButton.onClick.AddListener(
                    () => Utils.LoadLevel(triggerScene.Attributes["idTarget"].Value));
            }

            XmlNode speakPlayer = node.SelectSingleNode("speak-player");
            if (speakPlayer != null)
            {
                customButton.onClick.AddListener(
                    () => Utils.ShowTextBubble(speakPlayer.Value));
            }

            XmlNode triggerConv = node.SelectSingleNode("trigger-conversation");
            if (triggerConv != null)
            {
                // TODO trigger conversation fron ID
                customButton.onClick.AddListener(
                    () => Conversations.PlayConversation(triggerConv.Attributes["idTarget"].Value));
            }
            customButton.onClick.AddListener(
                    () => Destroy());
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Destroy()
    {
        Destroy(actionsControl);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShowActionsControl();
    }
}
