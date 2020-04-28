using System;
using System.Collections;
using System.Collections.Generic;
using Reporting;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UIReportsTabController : MonoBehaviour
{
    public GameObject timePeriodButtonPrefab;

    public GameObject timePeriodButtonsContainer;
    public GameObject reportContentContainer;

    // Transhuman Indexes
    [FoldoutGroup("Transhuman Index Setup")]
    public GameObject thIndexTitleContainer;
    [FoldoutGroup("Transhuman Index Setup")]
    public GameObject overAllThIndexContainer;
    [FoldoutGroup("Transhuman Index Setup")]
    public UIIndexBar overAllTHIndexBar;
    [FoldoutGroup("Transhuman Index Setup")]
    public GameObject lowerClassThIndexContainer;
    [FoldoutGroup("Transhuman Index Setup")]
    public UIIndexBar lowerClassTHIndexBar;
    [FoldoutGroup("Transhuman Index Setup")]
    public GameObject middleClassThIndexContainer;
    [FoldoutGroup("Transhuman Index Setup")]
    public UIIndexBar middleClassTHIndexBar;
    [FoldoutGroup("Transhuman Index Setup")]
    public GameObject upperClassThIndexContainer;
    [FoldoutGroup("Transhuman Index Setup")]
    public UIIndexBar upperClassTHIndexBar;
    
    // Aesthetic Transhuman Indexes
    [FoldoutGroup("Aesthetic Transhuman Index Setup")]
    public GameObject aesThIndexTitleContainer;
    [FoldoutGroup("Aesthetic Transhuman Index Setup")]
    public GameObject overAllAesThIndexContainer;
    [FoldoutGroup("Aesthetic Transhuman Index Setup")]
    public UIIndexBar overAllAesTHIndexBar;
    [FoldoutGroup("Aesthetic Transhuman Index Setup")]
    public GameObject lowerClassAesThIndexContainer;
    [FoldoutGroup("Aesthetic Transhuman Index Setup")]
    public UIIndexBar lowerClassAesTHIndexBar;
    [FoldoutGroup("Aesthetic Transhuman Index Setup")]
    public GameObject middleClassAesThIndexContainer;
    [FoldoutGroup("Aesthetic Transhuman Index Setup")]
    public UIIndexBar middleClassAesTHIndexBar;
    [FoldoutGroup("Aesthetic Transhuman Index Setup")]
    public GameObject upperClassAesThIndexContainer;
    [FoldoutGroup("Aesthetic Transhuman Index Setup")]
    public UIIndexBar upperClassAesTHIndexBar;
    
    public Text reportTitle;
    public Text reportText;

    public Color timePeriodButtonColorNormal = Color.gray;
    public Color timePeriodButtonColorActive = Color.cyan;
    
    private Player _player;

    private bool initialized;
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
        if (!initialized)
        {
            _player = FindObjectOfType<Player>();
            _player.TimePeriodChangedEvent += TimePeriodChanged;
            initialized = true;
        }

        if (_player == null) return;
        
        CreateTimePeriodButtons();
    }

    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.TimePeriodChangedEvent -= TimePeriodChanged;
        }
    }

    public void CreateTimePeriodButtons()
    {
        if (timePeriodButtonsContainer == null || timePeriodButtonPrefab == null || _player == null) return;
        
        // Delete existing buttons
        if (timePeriodButtonsContainer.transform.childCount > 0)
        {
            for (int i = 0; i < timePeriodButtonsContainer.transform.childCount; i++)
            {
                Destroy(timePeriodButtonsContainer.transform.GetChild(i).gameObject);
            }
        }

        // Create buttons
        if (_player.timePeriodsData != null && _player.timePeriodsData.Count > 0)
        {
            Button latestTimePeriodButton = null;
            foreach (var timePeriod in _player.timePeriodsData)
            {
                var go = Instantiate(timePeriodButtonPrefab, timePeriodButtonsContainer.transform);
                var btn = go.GetComponent<Button>();
                var txt = go.GetComponentInChildren<Text>();

                if (btn != null)
                {
                    if (timePeriod == _player.currentTimePeriod?.previousPeriod)
                    {
                        latestTimePeriodButton = btn;
                    }
                    var timePeriodCopy = timePeriod;
                    btn.onClick.AddListener(() =>
                    {
                        LoadReport(timePeriodCopy);
                        UpdateTimePeriodButtonColors(btn);
                    });
                }

                if (txt != null)
                {
                    txt.text = timePeriod.GetYear(_player.gameSetupData).ToString();
                    if (timePeriod == _player.currentTimePeriod?.previousPeriod)
                    {
                        //txt.text += "\n(Newest Report)";
                        txt.fontStyle = FontStyle.Bold;
                    }
                }
            }
            LoadReport(_player.currentTimePeriod?.previousPeriod);
            UpdateTimePeriodButtonColors(latestTimePeriodButton);
        }
        
    }

    private void UpdateTimePeriodButtonColors(Button activeButton)
    {
        if (timePeriodButtonsContainer != null && timePeriodButtonsContainer.transform.childCount > 0)
        {
            for (int i = 0; i < timePeriodButtonsContainer.transform.childCount; i++)
            {
                var go = timePeriodButtonsContainer.transform.GetChild(i);
                var btn = go.GetComponent<Button>();

                var colors = btn.colors;
                colors.normalColor = (activeButton == btn) ? timePeriodButtonColorActive : timePeriodButtonColorNormal;
                btn.colors = colors;
            }
        }
    }

    private void LoadReport(TimePeriod timePeriod)
    {
        if (timePeriod == null || reportContentContainer == null) return;

        if (reportTitle != null) reportTitle.text = "Report of " +timePeriod.GetYear(_player.gameSetupData);
        if (reportText != null)
        {
            reportText.text = "\n\nAccessibility Review\n=====================\n";
            reportText.text += timePeriod.report?.GetReportText(Report.ReportType.AccessiblityPerWL).GetReportText(timePeriod.report)??"";
            reportText.text += "\n\nTechnology Review\n=====================\n";
            reportText.text += timePeriod.report?.GetReportText(Report.ReportType.TechBranchResearch).GetReportText(timePeriod.report)??"";
        }

        // Transhuman Indexes
        thIndexTitleContainer?.SetActive(true);
        if (overAllTHIndexBar != null)
        {
            overAllThIndexContainer?.SetActive(true);
            overAllTHIndexBar.CurrentIndexPos = timePeriod.report?.thIndexTotalPopulation ?? 0;
        }
        if (lowerClassTHIndexBar != null && timePeriod.report != null)
        {
            lowerClassThIndexContainer?.SetActive(true);
            timePeriod.report.thIndexPerWL.TryGetValue(GameSetupData.WealthLevels.LowerClass,
                out var thIndexLowerClass);
            lowerClassTHIndexBar.CurrentIndexPos = thIndexLowerClass;
        }
        if (middleClassTHIndexBar != null && timePeriod.report != null)
        {
            middleClassThIndexContainer?.SetActive(true);
            timePeriod.report.thIndexPerWL.TryGetValue(GameSetupData.WealthLevels.MiddleClass,
                out var thIndexMiddleClass);
            middleClassTHIndexBar.CurrentIndexPos = thIndexMiddleClass;
        }
        if (upperClassTHIndexBar != null && timePeriod.report != null)
        {
            upperClassThIndexContainer?.SetActive(true);
            timePeriod.report.thIndexPerWL.TryGetValue(GameSetupData.WealthLevels.UpperClass,
                out var thIndexUpperClass);
            upperClassTHIndexBar.CurrentIndexPos = thIndexUpperClass;
        }
        
        // Aesthetic Transhuman Indexes
        aesThIndexTitleContainer?.SetActive(true);
        if (overAllAesTHIndexBar != null)
        {
            overAllAesThIndexContainer?.SetActive(true);
            overAllAesTHIndexBar.CurrentIndexPos = timePeriod.report?.aesThIndexTotalPopulation ?? 0;
        }
        if (lowerClassAesTHIndexBar != null && timePeriod.report != null)
        {
            lowerClassAesThIndexContainer?.SetActive(true);
            timePeriod.report.aestheticThIndexPerWL.TryGetValue(GameSetupData.WealthLevels.LowerClass,
                out var aesThIndexLowerClass);
            lowerClassAesTHIndexBar.CurrentIndexPos = aesThIndexLowerClass;
        }
        if (middleClassAesTHIndexBar != null && timePeriod.report != null)
        {
            middleClassAesThIndexContainer?.SetActive(true);
            timePeriod.report.aestheticThIndexPerWL.TryGetValue(GameSetupData.WealthLevels.MiddleClass,
                out var aesThIndexMiddleClass);
            middleClassAesTHIndexBar.CurrentIndexPos = aesThIndexMiddleClass;
        }
        if (upperClassAesTHIndexBar != null && timePeriod.report != null)
        {
            upperClassAesThIndexContainer?.SetActive(true);
            timePeriod.report.aestheticThIndexPerWL.TryGetValue(GameSetupData.WealthLevels.UpperClass,
                out var aesThIndexUpperClass);
            upperClassAesTHIndexBar.CurrentIndexPos = aesThIndexUpperClass;
        }
    }

    private void TimePeriodChanged(TimePeriod oldTimePeriod, TimePeriod newTimePeriod)
    {
        StartCoroutine(TimePeriodChangedCR(oldTimePeriod, newTimePeriod));
    }

    private IEnumerator TimePeriodChangedCR(TimePeriod oldTimePeriod, TimePeriod newTimePeriod)
    {
        yield return new WaitForEndOfFrame();
        CreateTimePeriodButtons();
        yield return new WaitForEndOfFrame();
        LoadReport(oldTimePeriod);
        yield return 0;
    }
}
