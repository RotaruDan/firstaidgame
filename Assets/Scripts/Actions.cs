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
    private GameObject actionsControl;

    // Use this for initialization
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();

    }

    void ShowActionsControl()
    {
        if (actionsControl != null)
        {
            Destroy(actionsControl);
        }

        actionsControl = Utils.LoadActionsPrefab(canvas.transform);

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

            if (condition != null)
            {
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

            XmlNodeList effectsList = node.SelectNodes("*");

            for (int j = 0; j < effectsList.Count; j += 2)
            {
                XmlNode effectNode = effectsList[j];

                string effectNodeName = effectNode.Name;

                XmlNode conditionNode = effectsList[j + 1];
                Conversations.Condition conditionModel = null;
                XmlNodeList conditions = conditionNode.SelectNodes("*");
                if (conditions != null && conditions.Count > 0)
                {
                    conditionModel = new Conversations.Condition();

                    XmlNodeList activesList = conditionNode.SelectNodes("active");
                    conditionModel.actives = new string[activesList.Count];
                    for (int y = 0; y < activesList.Count; y++)
                    {
                        conditionModel.actives[y] = activesList[y].Attributes["flag"].Value;
                    }

                    XmlNodeList inactivesList = conditionNode.SelectNodes("inactive");
                    conditionModel.inactives = new string[inactivesList.Count];
                    for (int y = 0; y < inactivesList.Count; y++)
                    {
                        conditionModel.inactives[y] = inactivesList[y].Attributes["flag"].Value;
                    }
                }


                if (effectNodeName == "activate")
                {
                    if (conditionModel == null || conditionModel.isTrue())
                    {
                        customButton.onClick.AddListener(
                            () => Flags.Agregar(effectNode.Attributes["flag"].Value, true));
                    }
                }
                else if (effectNodeName == "deactivate")
                {
                    if (conditionModel == null || conditionModel.isTrue())
                    {
                        customButton.onClick.AddListener(
                            () => Flags.Agregar(effectNode.Attributes["flag"].Value, false));
                    }
                }
                else if (effectNodeName == "increment")
                {
                    if (conditionModel == null || conditionModel.isTrue())
                    {
                        customButton.onClick.AddListener(
                            () => Utils.IncrementFlagValue(effectNode.Attributes["value"].Value,
                            Utils.IntParseFast(effectNode.Attributes["var"].Value)));
                    }
                }
                else if (effectNodeName == "decrement")
                {
                    if (conditionModel == null || conditionModel.isTrue())
                    {
                        customButton.onClick.AddListener(
                            () => Utils.DecrementFlagValue(effectNode.Attributes["value"].Value,
                            Utils.IntParseFast(effectNode.Attributes["var"].Value)));
                    }
                }
                else if (effectNodeName == "trigger-conversation")
                {
                    if (conditionModel == null || conditionModel.isTrue())
                    {
                        customButton.onClick.AddListener(
                            () => Conversations.PlayConversation(effectNode.Attributes["idTarget"].Value));
                    }
                }
                else if (effectNodeName == "trigger-scene")
                {
                    if (conditionModel == null || conditionModel.isTrue())
                    {
                        customButton.onClick.AddListener(
                            () => Utils.LoadLevel(effectNode.Attributes["idTarget"].Value));
                    }
                }
                else if (effectNodeName == "speak-player")
                {
                    if (conditionModel == null || conditionModel.isTrue())
                    {
                        customButton.onClick.AddListener(
                            () => Utils.ShowTextBubble(effectNode.InnerText.Replace("#O ", "")));
                    }
                }
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
