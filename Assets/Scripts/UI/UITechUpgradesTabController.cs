using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class UITechUpgradesTabController : MonoBehaviour
{
    public GameObject upgradeCategoryButtonPrefab;

    public Color32 categoryButtonDefaultColor = Color.gray;
    public Color32 categoryButtonActiveColor = Color.green;
    
    public GameObject upgradeCategoryButtonsContainer;
    public GameObject upgradeCategorySkillTreeContainer;

    private Dictionary<TechUpgradeCategory, GameObject> categorySubTabs = new Dictionary<TechUpgradeCategory, GameObject>();
    private Dictionary<TechUpgradeCategory, Button> categoryButtons = new Dictionary<TechUpgradeCategory, Button>();

    private Player _player;
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
        _player = FindObjectOfType<Player>();

        InitUpgradeCategorySkillTrees();
        InitUpgradeCategoryButtons();
    }

    private void InitUpgradeCategorySkillTrees()
    {
        if (_player == null || _player.gameSetupData?.techUpgradeCategories == null || upgradeCategorySkillTreeContainer == null) return;

        if (categorySubTabs != null && categorySubTabs.Count > 0)
        {
            foreach (var catSubTabsKVP in categorySubTabs)
            {
                Destroy(catSubTabsKVP.Value);
            }
            categorySubTabs.Clear();
        }
        
        if (_player.gameSetupData.techUpgradeCategories.Count > 0)
        {
            foreach (var upgCategory in _player.gameSetupData.techUpgradeCategories)
            {
                if(upgCategory.techUpgradeSkillTreePrefab == null) continue;

                var go = Instantiate(upgCategory.techUpgradeSkillTreePrefab,
                    upgradeCategorySkillTreeContainer.transform);
                categorySubTabs.Add(upgCategory, go);
            }
        }
    }

    private void InitUpgradeCategoryButtons()
    {
        if (upgradeCategoryButtonsContainer == null || upgradeCategoryButtonPrefab == null || _player == null) return;

        if (categoryButtons != null && categoryButtons.Count > 0)
        {
            foreach (var btnKVP in categoryButtons)
            {
                Destroy(btnKVP.Value);
            }
            categoryButtons.Clear();
        }/*if (upgradeCategoryButtonsContainer.transform.childCount > 0)
        {
            for (int i = 0; i < upgradeCategoryButtonsContainer.transform.childCount; i++)
            {
                Destroy(upgradeCategoryButtonsContainer.transform.GetChild(i));
            }
        }*/

        if (_player.gameSetupData?.techUpgradeCategories != null && _player.gameSetupData?.techUpgradeCategories.Count > 0)
        {
            foreach (var upgCategory in _player.gameSetupData?.techUpgradeCategories)
            {
                var go = Instantiate(upgradeCategoryButtonPrefab, upgradeCategoryButtonsContainer.transform);
                var btn = go?.GetComponent<Button>();
                categoryButtons.Add(upgCategory, btn);
                if (btn != null)
                {
                    var btnText = btn.GetComponentInChildren<Text>();
                    if (btnText != null)
                        btnText.text = upgCategory.categoryName;
                    
                    var upgCategoryRefCopy = upgCategory;
                    btn.onClick.AddListener(() =>
                    {
                        if (categorySubTabs != null && categorySubTabs.Count > 0)
                        {
                            categorySubTabs.Values.ForEach((tabObj) => tabObj.SetActive(false));
                            
                            categorySubTabs.TryGetValue(upgCategoryRefCopy, out var activeTab);
                            activeTab?.SetActive(true);
                        }

                        if (categoryButtons != null && categoryButtons.Count > 0)
                        {
                            categoryButtons.Values.ForEach(button =>
                            {
                                var colors = button.colors;
                                colors.normalColor = categoryButtonDefaultColor;
                                button.colors = colors;
                            });

                            categoryButtons.TryGetValue(upgCategoryRefCopy, out var activeButton);
                            if (activeButton != null)
                            {
                                var activeBtnColors = activeButton.colors;
                                activeBtnColors.normalColor = categoryButtonActiveColor;
                                activeButton.colors = activeBtnColors;
                            }
                            
                        }
                    });
                }
            }
        }
    }
}
