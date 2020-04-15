using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIBudgetBarController : MonoBehaviour
    {

        public RectTransform progressBarContainerRect;
        public RectTransform progressBarRect;
        public Text progressBarText;

        private Player _player;
        
        // Start is called before the first frame update
        void Start()
        {
            _player = FindObjectOfType<Player>();
            if (_player != null)
            {
                _player.MoneySpentEvent += UpdateProgressBar;
                _player.TimePeriodChangedEvent += TimePeriodChanged;
            }
                
            UpdateProgressBar();
        }

        private void OnDestroy()
        {
            if (_player != null)
            {
                _player.MoneySpentEvent -= UpdateProgressBar;
                _player.TimePeriodChangedEvent -= TimePeriodChanged;
            }
        }

        public void TimePeriodChanged(TimePeriod newTimePeriod)
        {
            UpdateProgressBar();
        }

        public void UpdateProgressBar()
        {
            if (progressBarContainerRect != null && progressBarRect != null && _player != null)
            {
                var initialTotalBudget = _player.currentTimePeriod?.totalStartBudget ?? 0;
                float moneySpentBudgetRatio =  (float)_player.AvailableBudget / initialTotalBudget;
                var newProgressBarWidth = Mathf.Clamp(progressBarContainerRect.sizeDelta.x * moneySpentBudgetRatio,0,progressBarContainerRect.sizeDelta.x);
                
                progressBarRect.sizeDelta = new Vector2(newProgressBarWidth, progressBarRect.sizeDelta.y);

                if (progressBarText != null)
                {
                    progressBarText.text = "Budget "+_player.GetCurrencyString(_player.AvailableBudget) +"/"+_player.GetCurrencyString(initialTotalBudget);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
