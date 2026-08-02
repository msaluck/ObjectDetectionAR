using UnityEngine;
using TMPro;
using ObjectDetectionAR.Core;

namespace ObjectDetectionAR.UI
{
    public class ModelSelector : MonoBehaviour
    {
        [SerializeField]
        private TMP_Dropdown dropdown;

        [SerializeField] private ModelRegistry registry;

        [SerializeField] private DetectionRunner detectionRunner;

        public string CurrentModel { get; private set; }

        private void Start()
        {
            dropdown.ClearOptions();

            dropdown.AddOptions(registry.RegisteredModels);

            dropdown.value = 0;

            dropdown.RefreshShownValue();
            
            CurrentModel = dropdown.options[0].text;

            detectionRunner.SetModel(CurrentModel);

            dropdown.onValueChanged.AddListener(OnChanged);
        }

        private void OnChanged(int index)
        {
            CurrentModel = dropdown.options[index].text;
            detectionRunner.SetModel(CurrentModel);
        }
    }
}