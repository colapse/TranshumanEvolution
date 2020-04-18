using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPartsTabController : MonoBehaviour
    {
        public GameObject upgCategoryButtonPrefab;
        public GameObject partButtonPrefab;
    
        public GameObject upgCategoryButtonsContainer;

        public GameObject categoryPartsButtonContainer;

        public Image partIconImage;
        public Text partInfoTitleText;
        public Text partInfoDescriptionText;

        public Color32 btnPartColorNormal = Color.gray;

        public Color32 btnPartColorActive = Color.cyan;

        private Player _player;

        private bool playerInitialized = false;
        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Init()
        {
            if (!playerInitialized)
            {
                _player = FindObjectOfType<Player>();

                playerInitialized = true;
            }
        
            LoadUpgradeCategoryButtons();
        }

        public void LoadUpgradeCategoryButtons()
        {
            if (upgCategoryButtonPrefab == null || upgCategoryButtonsContainer == null ||_player == null) return;

            if (upgCategoryButtonsContainer.transform.childCount > 0)
            {
                for (int i = 0; i < upgCategoryButtonsContainer.transform.childCount; i++)
                {
                    Destroy(upgCategoryButtonsContainer.transform.GetChild(i).gameObject);
                }
            }

            if (_player.gameSetupData?.techUpgradeCategories != null)
            {
                foreach (var upgCat in _player.gameSetupData.techUpgradeCategories)
                {
                    var go = Instantiate(upgCategoryButtonPrefab, upgCategoryButtonsContainer.transform);
                    var btn = go?.GetComponent<Button>();

                    if (btn != null)
                    {
                        var upgCatRefCopy = upgCat;
                        btn.onClick.AddListener(() =>
                        {
                            LoadPartsOfCategory(upgCatRefCopy);
                        });
                    }

                    var btnName = go?.GetComponentInChildren<Text>();
                    if (btnName != null)
                    {
                        btnName.text = upgCat.categoryName;
                    }
                }
            }
        }
        
        private Dictionary<UpgradePart, Button> upgradePartButtons = new Dictionary<UpgradePart, Button>();

        public void LoadPartsOfCategory(TechUpgradeCategory techUpgradeCategory)
        {
            if (_player == null || techUpgradeCategory == null) return;

            // Load Parts
            if (categoryPartsButtonContainer != null && partButtonPrefab != null)
            {
                // Delete old parts buttons
                if (categoryPartsButtonContainer.transform.childCount > 0)
                {
                    for (int i = 0; i < categoryPartsButtonContainer.transform.childCount; i++)
                    {
                        Destroy(categoryPartsButtonContainer.transform.GetChild(i).gameObject);
                    }
                    upgradePartButtons.Clear();
                }

                // Load acquired parts of current upgradecategory
                foreach (var part in _player.upgradeParts)
                {
                    if(part.upgradeCategory != techUpgradeCategory) continue;
                    
                    var go = Instantiate(partButtonPrefab, categoryPartsButtonContainer.transform);
                    var btn = go.GetComponent<Button>();
                    upgradePartButtons.Add(part, btn);
                    var img = go.GetComponentInChildren<Image>();
                    var txt = go.GetComponentInChildren<Text>();

                    if (txt != null) txt.text = part.upgradePartName;
                    if (img != null && part.upgradePartIcon) img.sprite = part.upgradePartIcon;
                    if(btn != null) btn.onClick.AddListener(()=>{LoadPartDetails(part);UpdatePartButtonsColor(part);});
                }
            
            }
        }

        public void LoadPartDetails(UpgradePart upgradePart)
        {
            if (_player == null || upgradePart == null) return;

            
            
            if (partIconImage != null && upgradePart.upgradePartIcon != null)
                partIconImage.sprite = upgradePart.upgradePartIcon;
            if (partInfoTitleText != null) partInfoTitleText.text = upgradePart.upgradePartName;

            if (partInfoDescriptionText != null) partInfoDescriptionText.text = upgradePart.upgradePartDescription;
            
            // TODO load additional info (Including upgraded stats, cost, humanity index, ...)
        }

        public void UpdatePartButtonsColor(UpgradePart activePart)
        {
            if (upgradePartButtons != null && upgradePartButtons.Count > 0)
            {
                foreach (var partBtn in upgradePartButtons)
                {
                    if (partBtn.Value != null)
                    {
                        var colors = partBtn.Value.colors;
                        if (partBtn.Key == activePart) colors.normalColor = btnPartColorActive;
                        else colors.normalColor = btnPartColorNormal;
                        partBtn.Value.colors = colors;
                    }
                }
            }
        }
    }
}
