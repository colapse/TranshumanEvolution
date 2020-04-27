using System;
using System.Collections;
using System.Collections.Generic;
using Reporting;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UIReportsTabController : MonoBehaviour
{
    public GameObject timePeriodButtonPrefab;

    public GameObject timePeriodButtonsContainer;
    public GameObject reportContentContainer;

    public GameObject overAllThIndexContainer;
    public UIIndexBar overAllTHIndexBar;
    public GameObject lowerClassThIndexContainer;
    public UIIndexBar lowerClassTHIndexBar;
    public GameObject middleClassThIndexContainer;
    public UIIndexBar middleClassTHIndexBar;
    public GameObject upperClassThIndexContainer;
    public UIIndexBar upperClassTHIndexBar;
    
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

        if (overAllTHIndexBar != null)
        {
            overAllTHIndexBar.CurrentIndexPos = timePeriod.report?.thIndexTotalPopulation ?? 5;
            overAllThIndexContainer?.SetActive(true);
        }
        if (lowerClassTHIndexBar != null && timePeriod.report != null)
        {
            timePeriod.report.thIndexPerWL.TryGetValue(GameSetupData.WealthLevels.LowerClass,
                out var thIndexLowerClass);
            lowerClassTHIndexBar.CurrentIndexPos = thIndexLowerClass;
            lowerClassThIndexContainer?.SetActive(true);
        }
        if (middleClassTHIndexBar != null && timePeriod.report != null)
        {
            timePeriod.report.thIndexPerWL.TryGetValue(GameSetupData.WealthLevels.MiddleClass,
                out var thIndexMiddleClass);
            middleClassTHIndexBar.CurrentIndexPos = thIndexMiddleClass;
            middleClassThIndexContainer?.SetActive(true);
        }
        if (upperClassTHIndexBar != null && timePeriod.report != null)
        {
            timePeriod.report.thIndexPerWL.TryGetValue(GameSetupData.WealthLevels.UpperClass,
                out var thIndexUpperClass);
            upperClassTHIndexBar.CurrentIndexPos = thIndexUpperClass;
            upperClassThIndexContainer?.SetActive(true);
        }
    }

    private void TimePeriodChanged(TimePeriod oldTimePeriod, TimePeriod newTimePeriod)
    {
        CreateTimePeriodButtons();
    }
}
