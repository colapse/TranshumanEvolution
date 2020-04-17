using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIUpgradeTypeController : MonoBehaviour
    {
        public Image backgroundImage;

        public Image upgradeTypeIconImage;
    
        public Color32 positiveColor = new Color32(0,243,77,255);
        public Color32 negativeColor = new Color32(190,0,27, 255);

        public bool isPositive = true;

        public TechUpgradeType upgradeType;
    
        // Start is called before the first frame update
        void Start()
        {
            InitUpgradeType();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void InitUpgradeType()
        {
            if (upgradeType == null) return;

            backgroundImage.color = isPositive ? positiveColor : negativeColor;
            upgradeTypeIconImage.sprite = upgradeType.upgradeTypeIcon;
        }
    }
}
