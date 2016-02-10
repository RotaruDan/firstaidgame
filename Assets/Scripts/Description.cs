using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Description : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipPrefab;

    private Canvas canvas;
    private Camera cam;
    private RectTransform rectTransform;
    private GameObject tooltipObject;
    private Text text;

    private Vector3 position, scale;

    // Use this for initialization
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        position = new Vector3();
        scale = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if (tooltipObject != null)
        {
            Vector2 pos = TransformUtils.CalculatePositionFromMouseToRectTransform(canvas, cam);
            scale.Set(1, 1, 1);
            tooltipObject.transform.localScale = scale;

            position.Set(pos.x, pos.y + 0.05f, 1);
            tooltipObject.transform.position = position;
            // Debug.Log(pos.x + " " + pos.y);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.tooltipPrefab != null)
        {
            tooltipObject = Instantiate(tooltipPrefab);
            tooltipObject.transform.SetParent(this.gameObject.transform);
            rectTransform = tooltipObject.GetComponent<RectTransform>();
            text = tooltipObject.GetComponent<Text>();
            text.text = "Ta-dahdfhgdfhghjfghjfghjfghjgfhjfdfh dfhgdfh gdfh gdfh gdfhg!";
            Debug.Log("OnPointerEnter");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(tooltipObject);
        tooltipObject = null;
        Debug.Log("OnPointerExit");
    }

}
