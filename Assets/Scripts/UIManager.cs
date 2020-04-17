using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [FoldoutGroup("Tabs Setup")] public GameObject branchesTabPrefab;
    [FoldoutGroup("Tabs Setup")] public GameObject upgradesTabPrefab;
    [FoldoutGroup("Tabs Setup")] public GameObject reportTabPrefab;
    [FoldoutGroup("Tabs Setup")] public GameObject partsTabPrefab;
    [FoldoutGroup("Tabs Setup")] public GameObject helpTabPrefab;
    
    [FoldoutGroup("Tabs Setup")] public GameObject tabsContainer;

    [FoldoutGroup("Tabs Setup")] public Button buttonPartsTab;
    [FoldoutGroup("Tabs Setup")] public Button buttonBranchesTab;
    [FoldoutGroup("Tabs Setup")] public Button buttonUpgradesTab;
    [FoldoutGroup("Tabs Setup")] public Button buttonReportTab;
    [FoldoutGroup("Tabs Setup")] public Button buttonHelpTab;
    
    private UITechBranchesTabController branchesTabController;
    private UITechUpgradesTabController upgradesTabController;
    
    [FoldoutGroup("Top Bar Setup")] public GameObject timePhaseBarPrefab;
    [FoldoutGroup("Top Bar Setup")] public GameObject budgetProgressBarPrefab;
    [FoldoutGroup("Top Bar Setup")] public GameObject nextTimePeriodButtonPrefab;
    [FoldoutGroup("Top Bar Setup")] public GameObject topBarSubContainerPrefab;
    
    [FoldoutGroup("Top Bar Setup")] public GameObject topBarContainer;
    [FoldoutGroup("Runtime Variables")] public GameObject topBarSubContainer;

    

    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitUI()
    {
        _player = FindObjectOfType<Player>();
        InitTopBarContainer();
        InitTabs();
    }

    private void InitTopBarContainer()
    {
        if (topBarContainer != null)
        {
            if (timePhaseBarPrefab != null)
            {
                Instantiate(timePhaseBarPrefab, topBarContainer.transform);
            }
            
            if (topBarSubContainerPrefab != null)
            {
                topBarSubContainer = Instantiate(topBarSubContainerPrefab, topBarContainer.transform);
            }
        
            if (topBarSubContainer != null)
            {
            
                if (budgetProgressBarPrefab != null)
                {
                    var go = Instantiate(budgetProgressBarPrefab, topBarSubContainer.transform);
                    go?.GetComponent<UIBudgetBarController>()?.UpdateProgressBar();
                }

                if (nextTimePeriodButtonPrefab != null)
                {
                    var go = Instantiate(nextTimePeriodButtonPrefab, topBarSubContainer.transform);
                    var button = go?.GetComponent<Button>();
                    button?.onClick.AddListener(BtnClickNextTimePeriod);
                }
            }
        }
    }

    private void BtnClickNextTimePeriod()
    {
        _player.AdvanceToNextTimePeriod();
    }

    private void InitTabs()
    {
        // Init Branches Tab
        if (branchesTabPrefab != null)
        {
            var brachnesTabGo = Instantiate(branchesTabPrefab,tabsContainer.transform);
            branchesTabController = brachnesTabGo?.GetComponent<UITechBranchesTabController>();
            brachnesTabGo?.SetActive(false);
            buttonBranchesTab?.onClick.AddListener(ShowBranchesTab);
            
        }

        if (upgradesTabPrefab != null)
        {
            var tabGo = Instantiate(upgradesTabPrefab,tabsContainer.transform);
            upgradesTabController = tabGo?.GetComponent<UITechUpgradesTabController>();
            tabGo?.SetActive(false);
            buttonUpgradesTab?.onClick.AddListener(ShowUpgradesTab);
        }
        
        // TODO Other
        
        ShowHelpTab();
    }

    public void ShowBranchesTab()
    {
        branchesTabController?.gameObject.SetActive(true);
        
        // TODO Hide otherpanels
        upgradesTabController?.gameObject.SetActive(false);
    }

    public void ShowPartsTab()
    {
        // TODO Hide otherpanels
    }

    public void ShowUpgradesTab()
    {
        upgradesTabController?.gameObject.SetActive(true);
        
        // TODO Hide otherpanels
        branchesTabController?.gameObject.SetActive(false);
    }

    public void ShowReportTab()
    {
        // TODO Hide otherpanels
    }

    public void ShowHelpTab()
    {
        // TODO Hide otherpanels
    }
}
