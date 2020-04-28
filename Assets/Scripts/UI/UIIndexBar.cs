using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace UI
{
    public class UIIndexBar : MonoBehaviour
    {
        public RectTransform indexBarContainer;
        public RectTransform indexBarKnob;

        private Resolution res;

        [Range(0,10),ShowInInspector]
        private int currentIndexPos = 5;
        public int CurrentIndexPos
        {
            get => currentIndexPos;
            set
            {
                currentIndexPos = Mathf.Clamp(value,0,10);
                UpdateIndexPosition();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            res = Screen.currentResolution;
            UpdateIndexPosition();
        }

        // Update is called once per frame
        void Update()
        {
            if (res.width != Screen.currentResolution.width) {
                res = Screen.currentResolution;
                UpdateIndexPosition();
            }
        }

        private void UpdateIndexPosition()
        {
            var indexBarPartSize = indexBarContainer.sizeDelta.x / 10;
            var indexBarPartHalfSize = 0;//indexBarPartSize / 2;
            var newKnobPos = indexBarKnob.anchoredPosition;
            
            newKnobPos.x = indexBarPartSize * CurrentIndexPos - indexBarPartHalfSize;
            indexBarKnob.anchoredPosition = newKnobPos;
        }
    }
}
