using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Description : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D actionCursor;
    public GameObject tooltipPrefab;
    public TextAsset xmlDescription;

    private Canvas canvas;
    private Camera cam;
    private GameObject tooltipObject;
    private Text text;
    private string description;

    private Vector3 position, scale;

    // Use this for initialization
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        position = new Vector3();
        scale = new Vector3();

        string textDescription = xmlDescription.text;
        ReadDescription(textDescription);
    }

    void ReadDescription(string text)
    {
        PT_XMLReader xmlr = Utils.GetXMLReader();
        xmlr.Parse(text);

        description = xmlr.xml["description"][0]["name"][0].text;
    }

    // Update is called once per frame
    void Update()
    {
        if (tooltipObject != null)
        {
            scale.Set(1, 1, 1);
            tooltipObject.transform.localScale = scale;

            Vector2 pos = TransformUtils.CalculatePositionFromMouseToRectTransform(canvas, cam);
            position.Set(pos.x, pos.y + 0.05f, 1);
            tooltipObject.transform.position = position;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.tooltipPrefab != null)
        {
            tooltipObject = Instantiate(tooltipPrefab);
            tooltipObject.transform.SetParent(this.gameObject.transform);
            text = tooltipObject.GetComponent<Text>();
            text.text = description;
        }
        if(this.actionCursor != null)
        {
            Cursor.SetCursor(actionCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(tooltipObject);
        tooltipObject = null;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

}
