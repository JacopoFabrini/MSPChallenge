
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HSVPicker
{
    [System.Serializable]
    public class ColorPickerSetup
    {
        [System.Serializable]
        public class UiElements
        {
            public RectTransform[] Elements;


            public void Toggle(bool active)
            {
                for (int cnt = 0; cnt < Elements.Length; cnt++)
                {
                    Elements[cnt].gameObject.SetActive(active);
                }
            }

        }

        public bool ShowRgb = true;
        public bool ShowHsv;
        public bool ShowAlpha = true;
        public bool ShowColorBox = true;       

        public UiElements RgbSliders;
        public UiElements HsvSliders;
        public UiElements AlphaSliders;

        public UiElements ColorBox;

        public string PresetColorsId = "default";
        public Color[] DefaultPresetColors;
    }
}
