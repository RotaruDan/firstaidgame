using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using UnityEngine.Events;

public static class Conversations
{

    [System.Serializable]
    public class Conversation
    {
        public Node[] nodes;

        public void exec()
        {
            nodes[0].nodeFinished = nodeFinished;
            nodes[0].exec();
        }

        private void nodeFinished(int nextNode)
        {
            if (nextNode != -1)
            {
                nodes[nextNode].nodeFinished = nodeFinished;
                nodes[nextNode].exec();
            }
        }
    }

    [System.Serializable]
    public class Node
    {
        public UnityAction<int> nodeFinished;
        public int nextIndex = -1;
        public virtual void exec()
        {
            nodeFinished(nextIndex);
        }
    }

    [System.Serializable]
    public class DialogNode : Node
    {
        public Speak[] speaks;
        public EndConvEffect[] effects;
        private int i;
        public override void exec()
        {
            i = 0;
            execSpeak(i);
        }

        private void execSpeak(int i)
        {
            if (i < speaks.Length)
            {
                speaks[i].exec(bubbleEnded);
            }
            else
            {
                if (effects == null)
                {
                    nodeFinished(nextIndex);
                }
                else
                {
                    for (int j = 0; j < effects.Length; ++j)
                    {
                        effects[j].exec();
                    }
                    nodeFinished(nextIndex);
                }
            }
        }

        private void bubbleEnded()
        {
            ++i;
            execSpeak(i);
        }
    }

    [System.Serializable]
    public class EndConvEffect
    {
        public Condition condition;
        public void exec()
        {
            if (condition != null && !condition.isTrue())
            {
                return;
            }
            execEffect();
        }

        public virtual void execEffect()
        {

        }
    }

    [System.Serializable]
    public class TriggerConvEffect : EndConvEffect
    {
        public string idTarget;

        public override void execEffect()
        {
            if (idTarget != null)
            {
                Conversations.PlayConversation(idTarget);
            }
        }
    }

    [System.Serializable]
    public class TriggerSceneEffect : EndConvEffect
    {
        public string idTarget;

        public override void execEffect()
        {
            if (idTarget != null)
            {
                Utils.LoadLevel(idTarget);
            }
        }
    }

    [System.Serializable]
    public class TriggerCutSceneEffect : EndConvEffect
    {
        public string idTarget;

        public override void execEffect()
        {
            if (idTarget != null)
            {
                MovieTexture movieTexture = null;
                if (idTarget == "VideoElectrodos")
                {
                    movieTexture = Resources.Load<MovieTexture>(idTarget);
                }
                Utils.PlayFullVideo(movieTexture);
            }
        }
    }

    [System.Serializable]
    public class ActivateFlagEffect : EndConvEffect
    {
        public string flag;

        public override void execEffect()
        {
            if (flag != null)
            {
                Flags.Agregar(flag, true);
            }
        }
    }

    [System.Serializable]
    public class DeactivateFlagEffect : EndConvEffect
    {
        public string flag;

        public override void execEffect()
        {
            if (flag != null)
            {
                Flags.Agregar(flag, false);
            }
        }
    }

    [System.Serializable]
    public class DecrementFlagEffect : EndConvEffect
    {
        public string var;
        public int val = 0;

        public override void execEffect()
        {
            if (var != null)
            {
                Utils.DecrementFlagValue(var, val);
            }
        }
    }


    [System.Serializable]
    public class IncrementFlagEffect : EndConvEffect
    {
        public string var;
        public int val = 0;

        public override void execEffect()
        {
            if (var != null)
            {
                Utils.IncrementFlagValue(var, val);
            }
        }
    }

    [System.Serializable]
    public class OptionNode : Node
    {
        public Option[] options;
        public bool isRandom = false;

        public override void exec()
        {
            Utils.ShowOptions(options, nodeFinished, isRandom);
        }
    }

    [System.Serializable]
    public class Speak
    {
        public string text;
        public Condition condition;

        // If true the phone is speaking, otherwise is the player
        public bool isPhone = false;

        public void exec(UnityAction bubbleEndedCall)
        {
            if (condition != null && !condition.isTrue())
            {
                bubbleEndedCall();
                return;
            }
            if (!isPhone)
            {
                Utils.ShowTextBubble(text, bubbleEndedCall);
            }
            else
            {
                Utils.ShowTextBubble(text, bubbleEndedCall, true);
            }
        }
    }

    [System.Serializable]
    public class Option
    {
        public string text;
        public Condition condition;
        public int nextIndex;
    }

    [System.Serializable]
    public class Condition
    {
        public string[] actives;
        public string[] inactives;

        public bool isTrue()
        {
            if (actives != null)
            {
                for (int i = 0; i < actives.Length; ++i)
                {
                    if (!Flags.ValorDe(actives[i]))
                    {
                        return false;
                    }
                }
            }

            if (inactives != null)
            {
                for (int i = 0; i < inactives.Length; ++i)
                {
                    if (Flags.ValorDe(inactives[i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    private static XmlDocument xmld;

    private static Dictionary<string, string> conversations = new Dictionary<string, string>();

    private static void Init()
    {
        TextAsset convXml = Resources.Load<TextAsset>("conversations.xml");

        xmld = new XmlDocument();
        xmld.LoadXml(convXml.text);

    }

    public static void PlayConversation(string id)
    {
        if (xmld == null)
        {
            Init();
        }
        string query = string.Format("//*[@id='{0}']", id);
        XmlElement conv = (XmlElement)xmld.SelectSingleNode(query);

        XmlNodeList nodes = conv.SelectNodes("*");

        Conversation convModel = new Conversation();
        convModel.nodes = new Node[nodes.Count];

        for (int i = 0; i < nodes.Count; ++i)
        {
            XmlNode node = nodes[i];
            string name = node.Name;
            Node nodeModel = null;
            if (name == "dialogue-node")
            {
                DialogNode dialogNodeModel = new DialogNode();

                XmlNodeList dialogSpeaks = node.SelectNodes("*");

                dialogNodeModel.speaks = new Speak[node.SelectNodes("speak-player").Count
                     + node.SelectNodes("speak-char").Count];
                int z = 0;

                for (int j = 0; j < dialogSpeaks.Count; ++j)
                {
                    XmlNode speakNode = dialogSpeaks[j];

                    string speakName = speakNode.Name;

                    if (speakName == "speak-player")
                    {
                        Speak speakModel = new Speak();
                        speakModel.text = speakNode.InnerText;
                        dialogNodeModel.speaks[z] = speakModel;
                        ++z;
                    }
                    else if (speakName == "speak-char")
                    {
                        Speak speakModel = new Speak();
                        speakModel.text = speakNode.InnerText;
                        speakModel.isPhone = true;
                        dialogNodeModel.speaks[z] = speakModel;
                        ++z;
                    }
                    else if (speakName == "condition")
                    {
                        Condition conditionModel = new Condition();

                        XmlNodeList activesList = speakNode.SelectNodes("active");
                        conditionModel.actives = new string[activesList.Count];
                        for (int y = 0; y < activesList.Count; y++)
                        {
                            conditionModel.actives[y] = activesList[y].Attributes["flag"].Value;
                        }

                        XmlNodeList inactivesList = speakNode.SelectNodes("inactive");
                        conditionModel.inactives = new string[inactivesList.Count];
                        for (int y = 0; y < inactivesList.Count; y++)
                        {
                            conditionModel.inactives[y] = inactivesList[y].Attributes["flag"].Value;
                        }

                        dialogNodeModel.speaks[z - 1].condition = conditionModel;
                    }
                    else if (speakName == "child")
                    {
                        dialogNodeModel.nextIndex = Utils.IntParseFast(speakNode.Attributes["nodeindex"].Value);
                    }
                    else if (speakName == "end-conversation")
                    {
                        XmlNode effect = speakNode.SelectSingleNode("effect");
                        if (effect != null)
                        {
                            EndConvEffect endConvEffModel = new EndConvEffect();
                            XmlNodeList effectsList = effect.SelectNodes("*");

                            dialogNodeModel.effects = new EndConvEffect[
                                effectsList.Count - effect.SelectNodes("condition").Count];
                            int m = 0;
                            for (int x = 0; x < effectsList.Count; ++x)
                            {
                                XmlNode effectNode = effectsList[x];

                                string effectNodeName = effectNode.Name;
                                if (effectNodeName == "activate")
                                {
                                    ActivateFlagEffect activateFlageffect = new ActivateFlagEffect();
                                    activateFlageffect.flag = effectNode.Attributes["flag"].Value;
                                    dialogNodeModel.effects[m] = activateFlageffect;
                                    ++m;
                                }
                                else if (effectNodeName == "deactivate")
                                {
                                    DeactivateFlagEffect deactivateFlageffect = new DeactivateFlagEffect();
                                    deactivateFlageffect.flag = effectNode.Attributes["flag"].Value;
                                    dialogNodeModel.effects[m] = deactivateFlageffect;
                                    ++m;
                                }
                                else if (effectNodeName == "increment")
                                {
                                    IncrementFlagEffect eff = new IncrementFlagEffect();
                                    eff.val = Utils.IntParseFast(
                                        effectNode.Attributes["value"].Value);
                                    eff.var = effectNode.Attributes["var"].Value;
                                    dialogNodeModel.effects[m] = eff;
                                    ++m;
                                }
                                else if (effectNodeName == "decrement")
                                {
                                    DecrementFlagEffect eff = new DecrementFlagEffect();
                                    eff.val = Utils.IntParseFast(
                                        effectNode.Attributes["value"].Value);
                                    eff.var = effectNode.Attributes["var"].Value;
                                    dialogNodeModel.effects[m] = eff;
                                    ++m;

                                }
                                else if (effectNodeName == "trigger-conversation")
                                {
                                    TriggerConvEffect eff = new TriggerConvEffect();
                                    eff.idTarget = effectNode.Attributes["idTarget"].Value;
                                    dialogNodeModel.effects[m] = eff;
                                    ++m;
                                }
                                else if (effectNodeName == "trigger-scene")
                                {
                                    TriggerSceneEffect eff = new TriggerSceneEffect();
                                    eff.idTarget = effectNode.Attributes["idTarget"].Value;
                                    dialogNodeModel.effects[m] = eff;
                                    ++m;
                                }
                                else if (effectNodeName == "trigger-cutscene")
                                {
                                    TriggerCutSceneEffect eff = new TriggerCutSceneEffect();
                                    eff.idTarget = effectNode.Attributes["idTarget"].Value;
                                    dialogNodeModel.effects[m] = eff;
                                    ++m;

                                }
                                else if (effectNodeName == "condition")
                                {
                                    Condition conditionModel = new Condition();

                                    XmlNodeList activesList = effectNode.SelectNodes("active");
                                    conditionModel.actives = new string[activesList.Count];
                                    for (int y = 0; y < activesList.Count; y++)
                                    {
                                        conditionModel.actives[y] = activesList[y].Attributes["flag"].Value;
                                    }

                                    XmlNodeList inactivesList = effectNode.SelectNodes("inactive");
                                    conditionModel.inactives = new string[inactivesList.Count];
                                    for (int y = 0; y < inactivesList.Count; y++)
                                    {
                                        conditionModel.inactives[y] = inactivesList[y].Attributes["flag"].Value;
                                    }

                                    dialogNodeModel.effects[m - 1].condition = conditionModel;
                                }
                            }

                        }
                    }
                    else if (speakName == "effect")
                    {
                        XmlNodeList effectsList = speakNode.SelectNodes("*");

                        dialogNodeModel.effects = new EndConvEffect[
                            effectsList.Count - speakNode.SelectNodes("condition").Count];
                        int m = 0;
                        for (int x = 0; x < effectsList.Count; ++x)
                        {
                            XmlNode effectNode = effectsList[x];

                            string effectNodeName = effectNode.Name;
                            if (effectNodeName == "activate")
                            {
                                ActivateFlagEffect activateFlageffect = new ActivateFlagEffect();
                                activateFlageffect.flag = effectNode.Attributes["flag"].Value;
                                dialogNodeModel.effects[m] = activateFlageffect;
                                ++m;
                            }
                            else if (effectNodeName == "deactivate")
                            {
                                DeactivateFlagEffect deactivateFlageffect = new DeactivateFlagEffect();
                                deactivateFlageffect.flag = effectNode.Attributes["flag"].Value;
                                dialogNodeModel.effects[m] = deactivateFlageffect;
                                ++m;
                            }
                            else if (effectNodeName == "increment")
                            {
                                IncrementFlagEffect eff = new IncrementFlagEffect();
                                eff.val = Utils.IntParseFast(
                                    effectNode.Attributes["value"].Value);
                                eff.var = effectNode.Attributes["var"].Value;
                                dialogNodeModel.effects[m] = eff;
                                ++m;
                            }
                            else if (effectNodeName == "decrement")
                            {
                                DecrementFlagEffect eff = new DecrementFlagEffect();
                                eff.val = Utils.IntParseFast(
                                    effectNode.Attributes["value"].Value);
                                eff.var = effectNode.Attributes["var"].Value;
                                dialogNodeModel.effects[m] = eff;
                                ++m;

                            }
                            else if (effectNodeName == "trigger-conversation")
                            {
                                TriggerConvEffect eff = new TriggerConvEffect();
                                eff.idTarget = effectNode.Attributes["idTarget"].Value;
                                dialogNodeModel.effects[m] = eff;
                                ++m;
                            }
                            else if (effectNodeName == "trigger-scene")
                            {
                                TriggerSceneEffect eff = new TriggerSceneEffect();
                                eff.idTarget = effectNode.Attributes["idTarget"].Value;
                                dialogNodeModel.effects[m] = eff;
                                ++m;
                            }
                            else if (effectNodeName == "trigger-cutscene")
                            {
                                TriggerCutSceneEffect eff = new TriggerCutSceneEffect();
                                eff.idTarget = effectNode.Attributes["idTarget"].Value;
                                dialogNodeModel.effects[m] = eff;
                                ++m;

                            }
                            else if (effectNodeName == "condition")
                            {
                                Condition conditionModel = new Condition();

                                XmlNodeList activesList = effectNode.SelectNodes("active");
                                conditionModel.actives = new string[activesList.Count];
                                for (int y = 0; y < activesList.Count; y++)
                                {
                                    conditionModel.actives[y] = activesList[y].Attributes["flag"].Value;
                                }

                                XmlNodeList inactivesList = effectNode.SelectNodes("inactive");
                                conditionModel.inactives = new string[inactivesList.Count];
                                for (int y = 0; y < inactivesList.Count; y++)
                                {
                                    conditionModel.inactives[y] = inactivesList[y].Attributes["flag"].Value;
                                }

                                dialogNodeModel.effects[m - 1].condition = conditionModel;
                            }
                        }

                    }
                }

                nodeModel = dialogNodeModel;
            }
            else if (name == "option-node")
            {
                OptionNode optionNodeModel = new OptionNode();
                XmlAttribute randomAttr = node.Attributes["random"];
                if (randomAttr != null && randomAttr.Value == "yes")
                {
                    optionNodeModel.isRandom = true;
                }
                XmlNodeList optionSpeaks = node.SelectNodes("*");

                optionNodeModel.options = new Option[node.SelectNodes("speak-player").Count];
                int z = 0;

                for (int j = 0; j < optionSpeaks.Count; ++j)
                {
                    XmlNode optionNode = optionSpeaks[j];
                    string optionName = optionNode.Name;
                    if (optionName == "speak-player")
                    {
                        Option speakModel = new Option();
                        speakModel.text = optionNode.InnerText;
                        optionNodeModel.options[z] = speakModel;
                        ++z;
                    }
                    else if (optionName == "condition")
                    {
                        Condition conditionModel = new Condition();

                        XmlNodeList activesList = optionNode.SelectNodes("active");
                        conditionModel.actives = new string[activesList.Count];
                        for (int y = 0; y < activesList.Count; y++)
                        {
                            conditionModel.actives[y] = activesList[y].Attributes["flag"].Value;
                        }

                        XmlNodeList inactivesList = optionNode.SelectNodes("inactive");
                        conditionModel.inactives = new string[inactivesList.Count];
                        for (int y = 0; y < inactivesList.Count; y++)
                        {
                            conditionModel.inactives[y] = inactivesList[y].Attributes["flag"].Value;
                        }

                        optionNodeModel.options[z - 1].condition = conditionModel;
                    }
                    else if (optionName == "child")
                    {
                        optionNodeModel.options[z - 1].nextIndex = Utils.IntParseFast(optionNode.Attributes["nodeindex"].Value);
                    }
                }

                nodeModel = optionNodeModel;
            }

            convModel.nodes[i] = nodeModel;
        }
        convModel.exec();
    }

    public static string ValorDeInt(string flag)
    {
        string temp = "";
        if (conversations.TryGetValue(flag, out temp))
        {
            return temp;
        }
        else {
            return "";
        }
    }

    public static void Agregar(string flag, int valor)
    {
        //flags[flag] = valor;
    }
}
