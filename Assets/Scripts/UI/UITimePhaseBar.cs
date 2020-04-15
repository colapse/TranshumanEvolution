using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITimePhaseBar : MonoBehaviour
    {
        public GameObject timePhaseBlockPrefab;
        public GameObject timePhaseEntryContainer;
        
        public Color entryBackgroundColorPastPhase = new Color(0.4f,1f,1f, 1f);
        public Color entryBackgroundColorCurrentPhase = new Color(0f,0.75f,0.8f, 1f);
        public Color entryBackgroundColorFuturePhase = new Color(.8f,.8f,.8f, 1f);
        
        public Player player;
        
        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Player>();

            if (player != null)
            {
                player.TimePeriodChangedEvent += TimePeriodChanged;
                CreateTimePeriodBlocks();
            }
        }

        private void OnDestroy()
        {
            if (player != null)
            {
                player.TimePeriodChangedEvent -= TimePeriodChanged;
            }
        }

        private void TimePeriodChanged(TimePeriod timePeriod)
        {
            CreateTimePeriodBlocks();
        }

        private void CreateTimePeriodBlocks()
        {
            if (timePhaseBlockPrefab != null && timePhaseEntryContainer != null && player != null)
            {
                var currentTimePhase = player.currentTimePeriod?.GetCurrentTimePeriodLevel() ?? 1;
                var startYear = player.gameSetupData?.timePhaseStartYear ?? 2020;
                var periodIncrease = player.gameSetupData?.timePhaseDuration ?? 10;
                
                if (timePhaseEntryContainer.transform.childCount > 0)
                {
                    for (int i = 0; i < timePhaseEntryContainer.transform.childCount; i++)
                    {
                        Destroy(timePhaseEntryContainer.transform.GetChild(i).gameObject);
                    }
                }
                
                for (int i = 0; i < (player.gameSetupData?.maxTimePhases??0); i++)
                {
                    var go = Instantiate(timePhaseBlockPrefab, timePhaseEntryContainer.transform);
                    var text = go?.GetComponentInChildren<Text>();
                    var backgroundImage = go?.GetComponent<Image>();

                    if (text != null)
                    {
                        text.text = (startYear + i * periodIncrease).ToString();
                        if(i+1 == currentTimePhase)
                            text.fontStyle = FontStyle.Bold;
                        else
                            text.fontStyle = FontStyle.Normal;
                    }

                    if (backgroundImage != null)
                    {
                        backgroundImage.color = i+1 < currentTimePhase ? entryBackgroundColorPastPhase :
                            i+1 == currentTimePhase ? entryBackgroundColorCurrentPhase : entryBackgroundColorFuturePhase;
                    }
                }
                
                
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
