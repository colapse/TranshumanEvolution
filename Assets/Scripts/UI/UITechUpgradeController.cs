using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITechUpgradeController : MonoBehaviour
    {
        private Canvas canvas;
        public GameObject toolTipPrefab;
        
        public GameObject upgradeTypePrefab;

        public GameObject upgradeTypeBar;
        public Text upgradeCostText;
        public Image upgradeIconImage;
        public Image borderImage;
        public Button buyButton;
        
        public Color32 upgradePossessedColor = new Color32(0,200,240,255);
        public Color32 upgradeAvailableColor = new Color32(0,243,77,255);
        public Color32 upgradeNotAvailableColor = new Color32(190,0,27, 255);

        public TechUpgrade techUpgrade;
        private bool eventRegistered = false;

        private Player _player;

        private ToolTip toolTip;
        private RectTransform rectTransform;
        
        // Start is called before the first frame update
        void Start()
        {
            canvas = FindObjectOfType<Canvas>();
            rectTransform = GetComponent<RectTransform>();
            Init();
        }

        private void OnDestroy()
        {
            if (_player != null && eventRegistered)
            {
                _player.MoneySpentEvent -= Init;
                _player.NewTechUpgradeResearchedEvent -= NewUpgradeResearched;
                _player.TimePeriodChangedEvent -= TimePeriodChanged;
                _player.TechBranchPointIncreasedEvent -= TechBrachPointIncreased;
            }
            
        }

        private void TechBrachPointIncreased(TechBranch obj) => Init();
        private void TimePeriodChanged(TimePeriod oldTimePeriod, TimePeriod newTimePeriod) => Init();
        private void NewUpgradeResearched(TechUpgrade obj) => Init();

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Init()
        {
            _player = FindObjectOfType<Player>();
            if (_player != null && !eventRegistered)
            {
                _player.MoneySpentEvent += Init;
                _player.NewTechUpgradeResearchedEvent += NewUpgradeResearched;
                _player.TimePeriodChangedEvent += TimePeriodChanged;
                _player.TechBranchPointIncreasedEvent += TechBrachPointIncreased;
                eventRegistered = true;
            }
            if (techUpgrade == null || _player == null) return;
            
            if(buyButton != null)
                buyButton.onClick.AddListener(BuyTechUpgradeClicked);
            
            if (upgradeIconImage != null)
                upgradeIconImage.sprite = techUpgrade.upgradeIcon;

            int upgradeCost = _player.gameSetupData?.techUpgradesCost?.ContainsKey(techUpgrade) ?? false
                ? _player.gameSetupData.techUpgradesCost[techUpgrade]
                : 0;
            if (upgradeCostText != null && !_player.techUpgrades.Contains(techUpgrade))
            {
                upgradeCostText.text = _player.GetCurrencyString(upgradeCost);
            }
                
            
            // Upgrade Types
            if (techUpgrade.upgradeTypes != null && upgradeTypePrefab != null && upgradeTypeBar != null)
            {
                // Destroy existing children
                if (upgradeTypeBar.transform.childCount > 0)
                {
                    for (int i = 0; i < upgradeTypeBar.transform.childCount; i++)
                    {
                        Destroy(upgradeTypeBar.transform.GetChild(i).gameObject);
                    }
                }
                
                // Instantiate UpgradeTypes indicators
                foreach (var upgradeTypeKVP in techUpgrade.upgradeTypes)
                {
                    GameObject go = Instantiate(upgradeTypePrefab, upgradeTypeBar.transform);
                    var utController = go.GetComponent<UIUpgradeTypeController>();
                    utController.isPositive = upgradeTypeKVP.Value;
                    utController.upgradeType = upgradeTypeKVP.Key;
                    utController.InitUpgradeType();
                }
            }

            // Set Border Color
            if (borderImage != null)
            {
                if (_player.techUpgrades.Contains(techUpgrade))
                {
                    borderImage.color = upgradePossessedColor;
                }else
                {
                    bool upgradeAvailable = techUpgrade.CheckRequirements(_player);
                    
                    borderImage.color = upgradeAvailable ? upgradeAvailableColor : upgradeNotAvailableColor;
                }
            }
            
            // ToolTip
            if (toolTipPrefab != null && toolTip == null)
            {
                var go = Instantiate(toolTipPrefab, canvas.transform);
                toolTip = go.GetComponent<ToolTip>();
                if (toolTip != null)
                {
                    toolTip.SetTitle(techUpgrade.upgradeName);
                    toolTip.SetDescription(techUpgrade.upgradeDescription);
                    var toolTipPos = rectTransform.position;
                    toolTipPos.x += rectTransform.rect.width/2 + 1;
                    toolTipPos.y += rectTransform.rect.height/2 + 1;
                    toolTip.rectTransform.position = toolTipPos;
                    toolTip.HideToolTip();
                }
            }
        }

        private void BuyTechUpgradeClicked()
        {
            if (_player != null && techUpgrade != null && techUpgrade.CheckRequirements(_player))
            {
                _player.BuyTechUpgrade(techUpgrade);
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
