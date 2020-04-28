using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIUpgradeTypeController : MonoBehaviour
    {
        private Canvas canvas;
        public GameObject toolTipPrefab;
        public Image backgroundImage;

        public Image upgradeTypeIconImage;
    
        public Color32 positiveColor = new Color32(0,243,77,255);
        public Color32 negativeColor = new Color32(190,0,27, 255);

        public bool isPositive = true;

        public TechUpgradeType upgradeType;
        
        private ToolTip toolTip;
        private RectTransform rectTransform;
    
        // Start is called before the first frame update
        void Start()
        {
            canvas = FindObjectOfType<Canvas>();
            rectTransform = GetComponent<RectTransform>();
            InitUpgradeType();
        }

        private void OnEnable()
        {
            canvas = FindObjectOfType<Canvas>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void InitUpgradeType()
        {
            if (upgradeType == null) return;

            canvas = FindObjectOfType<Canvas>();
            rectTransform = GetComponent<RectTransform>();
            
            backgroundImage.color = isPositive ? positiveColor : negativeColor;
            upgradeTypeIconImage.sprite = upgradeType.upgradeTypeIcon;
            
            // ToolTip
            if (toolTipPrefab != null && toolTip == null)
            {
                var go = Instantiate(toolTipPrefab, canvas.transform);
                toolTip = go.GetComponent<ToolTip>();
                if (toolTip != null)
                {
                    toolTip.SetTitle(upgradeType.upgradeTypeName);
                    toolTip.SetDescription(upgradeType.upgradeTypeDescription);
                    var toolTipPos = rectTransform.position;
                    toolTipPos.x += rectTransform.rect.width/2 + 1;
                    toolTipPos.y += rectTransform.rect.height/2 + 1;
                    toolTip.rectTransform.position = toolTipPos;
                    toolTip.HideToolTip();
                }
            }
        }

        public void OnMouseEnter()
        {
            toolTip?.ShowToolTip();
        }

        public void OnMouseExit()
        {
            toolTip?.HideToolTip();
        }
    }
}
