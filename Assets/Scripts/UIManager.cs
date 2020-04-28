using System;
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
    private UIHelpTabController helpTabController;
    private UIReportsTabController reportsTabController;
    private UIPartsTabController partsTabController;
    
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
        _player.TimePeriodChangedEvent += TimePeriodChanged;
        
        InitTopBarContainer();
        InitTabs();
    }

    private void OnDestroy()
    {
        if(_player != null)
            _player.TimePeriodChangedEvent -= TimePeriodChanged;
    }

    private void TimePeriodChanged(TimePeriod arg1, TimePeriod arg2)
    {
        ShowReportTab();
        
        // Check current Time Period / if last & disable btn
        if (arg2.GetCurrentTimePeriodLevel() >= _player.gameSetupData.maxTimePhases)
        {
            var btnNextPeriod = GameObject.FindWithTag("NextTimePeriodButton")?.GetComponent<Button>();
            if (btnNextPeriod != null)
                btnNextPeriod.enabled = false;
        }
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

        if (helpTabPrefab != null)
        {
            var tabGo = Instantiate(helpTabPrefab,tabsContainer.transform);
            helpTabController = tabGo?.GetComponent<UIHelpTabController>();
            tabGo?.SetActive(false);
            buttonHelpTab?.onClick.AddListener(ShowHelpTab);
        }

        if (reportTabPrefab != null)
        {
            var tabGo = Instantiate(reportTabPrefab,tabsContainer.transform);
            reportsTabController = tabGo?.GetComponent<UIReportsTabController>();
            tabGo?.SetActive(false);
            buttonReportTab?.onClick.AddListener(ShowReportTab);
        }

        if (partsTabPrefab != null)
        {
            var tabGo = Instantiate(partsTabPrefab,tabsContainer.transform);
            partsTabController = tabGo?.GetComponent<UIPartsTabController>();
            tabGo?.SetActive(false);
            buttonPartsTab?.onClick.AddListener(ShowPartsTab);
        }
        
        ShowHelpTab();
    }

    public void ShowBranchesTab()
    {
        HideAllTabs();
        branchesTabController?.gameObject.SetActive(true);
    }

    public void ShowPartsTab()
    {
        HideAllTabs();
        partsTabController?.gameObject.SetActive(true);
    }

    public void ShowUpgradesTab()
    {
        HideAllTabs();
        upgradesTabController?.gameObject.SetActive(true);
        
    }

    public void ShowReportTab()
    {
        HideAllTabs();
        reportsTabController?.gameObject.SetActive(true);
    }

    public void ShowHelpTab()
    {
        HideAllTabs();
        helpTabController?.gameObject.SetActive(true);
        
    }

    public void HideAllTabs()
    {
        if (tabsContainer != null && tabsContainer.transform.childCount > 0)
        {
            for (int i = 0; i < tabsContainer.transform.childCount; i++)
            {
                tabsContainer.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
