using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITechBranchesTabController : MonoBehaviour
    {
        public Player player;
        public GameObject techBranchEntryContainer;
        public GameObject techBranchEntryPrefab;
        

        private List<GameObject> techBranchEntries = new List<GameObject>();
        
        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.MoneySpentEvent += LoadEntries;
                player.TechBranchPointIncreasedEvent += TechBranchPointAdded;
                player.TimePeriodChangedEvent += TimePeriodChanged;
            }
            LoadEntries();
        }

        private void OnDestroy()
        {
            if (player != null)
            {
                player.MoneySpentEvent -= LoadEntries;
                player.TechBranchPointIncreasedEvent -= TechBranchPointAdded;
                player.TimePeriodChangedEvent -= TimePeriodChanged;
            }
        }
        
        public void TimePeriodChanged(TimePeriod newTimePeriod)
        {
            LoadEntries();
        }

        private void TechBranchPointAdded(TechBranch obj)
        {
            LoadEntries();
        }

        private void OnEnable()
        {
            LoadEntries();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        [Button,DisableInEditorMode]
        public void LoadEntries()
        {
            if (techBranchEntries != null)
            {
                techBranchEntries.ForEach(entry => { Destroy(entry);});
                techBranchEntries.Clear();
            }else{techBranchEntries = new List<GameObject>();}
                
            if (player?.gameSetupData?.techBranches != null && techBranchEntryPrefab != null)
            {
                foreach (var techBranch in player.gameSetupData.techBranches)
                {
                    var newEntry = Instantiate(techBranchEntryPrefab, techBranchEntryContainer.transform);
                    techBranchEntries.Add(newEntry);
                    var branchIconEle = newEntry.transform.Find("TechBranchIcon")?.GetComponent<Image>();
                    var branchNameEle = newEntry.transform.Find("TechBranchNameCont/TechBranchName")?.GetComponent<Text>();
                    var branchCurrentLevelEle = newEntry.transform.Find("TechBranchCurrentLevelCont/TechBranchCurrentLevel")?.GetComponent<Text>();
                    var branchBtnInvest = newEntry.transform.Find("BtnInvest")?.GetComponent<Button>();
                    var branchBtnInvestTextEle = newEntry.transform.Find("BtnInvest/Text")?.GetComponent<Text>();

                    var currentLevel = 0;
                    player?.techBranchLevels?.TryGetValue(techBranch, out currentLevel);
                    var researchNextLevelCost = player.gameSetupData.GetTechBranchCost(techBranch, currentLevel+1);
                    
                    
                    if(branchIconEle != null && branchIconEle.sprite != null) branchIconEle.sprite = techBranch.branchIcon;
                    if(branchNameEle != null) branchNameEle.text = techBranch.branchName;
                    if(branchCurrentLevelEle != null) branchCurrentLevelEle.text = currentLevel.ToString();
                    if (branchBtnInvest != null)
                    {
                        branchBtnInvest.interactable = player.CanBuyTechBranchPoint(techBranch);
                        var techBranchParam = techBranch;
                        branchBtnInvest.onClick.AddListener(()=>{InvestInTechBranch(techBranchParam);});
                    }
                    if(branchBtnInvestTextEle != null) branchBtnInvestTextEle.text = "Invest\n"+player.GetCurrencyString(researchNextLevelCost);
                }
            }
            
        }

        public void InvestInTechBranch(TechBranch techBranch)
        {
            player?.BuyTechBranchPoint(techBranch);
        }
    }
}
