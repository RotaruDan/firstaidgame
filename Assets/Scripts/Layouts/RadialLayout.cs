
namespace UnityEngine.UI.Extensions
{
    public class RadialLayout : LayoutGroup
    {
        public float radio;

        [Range(0f, 360f)]
        public float minAngle, maxAngle, startAngle;

        protected override void OnEnable()
        {
            base.OnEnable();
            ComputeDpsitions();
        }

        public override void SetLayoutHorizontal()
        {

        }

        public override void SetLayoutVertical()
        {

        }

        public override void CalculateLayoutInputVertical()
        {
            ComputeDpsitions();
        }

        public override void CalculateLayoutInputHorizontal()
        {
            ComputeDpsitions();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            ComputeDpsitions();
        }
#endif

        void ComputeDpsitions()
        {
            m_Tracker.Clear();
            if (transform.childCount == 0)
            {
                return;
            }

            float fOffsetAngle = (maxAngle - minAngle) / (transform.childCount - 1);

            float fAngle = startAngle;
            for (int i = 0; i < transform.childCount; ++i)
            {
                RectTransform child = (RectTransform)transform.GetChild(i);
                if (child != null)
                {
                    //Adding the elements to the tracker stops the user from modifiying their positions via the editor.
                    m_Tracker.Add(this, child, DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition |
                    DrivenTransformProperties.Pivot);
                    Vector3 vPos = new Vector3(Mathf.Cos(fAngle * Mathf.Deg2Rad), Mathf.Sin(fAngle * Mathf.Deg2Rad), 0);
                    child.localPosition = vPos * radio;

                    //Force objects to be center aligned
                    child.anchorMin = child.anchorMax = child.pivot = new Vector2(0.5f, 0.5f);
                    fAngle += fOffsetAngle;
                }
            }

        }
    }
}
