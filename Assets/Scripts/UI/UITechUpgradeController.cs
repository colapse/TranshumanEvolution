using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITechUpgradeController : MonoBehaviour
    {
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
        // Start is called before the first frame update
        void Start()
        {
            
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
        private void TimePeriodChanged(TimePeriod obj) => Init();
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
                if (upgradeTypeBar.transform.childCount > 0)
                {
                    for (int i = 0; i < upgradeTypeBar.transform.childCount; i++)
                    {
                        Destroy(upgradeTypeBar.transform.GetChild(i).gameObject);
                    }
                }
                
                foreach (var upgradeTypeKVP in techUpgrade.upgradeTypes)
                {
                    GameObject go = Instantiate(upgradeTypePrefab, upgradeTypeBar.transform);
                    var utController = go.GetComponent<UIUpgradeTypeController>();
                    utController.isPositive = upgradeTypeKVP.Value;
                    utController.upgradeType = upgradeTypeKVP.Key;
                    utController.InitUpgradeType();
                }
            }

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
        }

        private void BuyTechUpgradeClicked()
        {
            if (_player != null && techUpgrade != null && techUpgrade.CheckRequirements(_player))
            {
                _player.BuyTechUpgrade(techUpgrade);
            }
        }
    }
}
