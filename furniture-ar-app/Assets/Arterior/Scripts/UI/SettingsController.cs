using UnityEngine;
using UnityEngine.UI;

namespace Arterior.UI
{
    /// <summary>
    /// Controls the settings panel and user preferences
    /// </summary>
    public class SettingsController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Toggle metricUnitsToggle;
        [SerializeField] private Button privacyButton;
        [SerializeField] private Button deleteDesignsButton;
        [SerializeField] private Button resetOnboardingButton;
        [SerializeField] private Text versionText;
        
        [Header("Settings")]
        [SerializeField] private string appVersion = "1.0.0";
        [SerializeField] private string privacyUrl = "https://example.com/privacy";
        
        private SaveLoadService saveLoadService;
        private OnboardingController onboardingController;
        private ProductCardController[] productCards;
        
        private void Start()
        {
            saveLoadService = FindObjectOfType<SaveLoadService>();
            onboardingController = FindObjectOfType<OnboardingController>();
            
            SetupButtons();
            LoadSettings();
            UpdateVersionDisplay();
        }
        
        /// <summary>
        /// Sets up button event listeners
        /// </summary>
        private void SetupButtons()
        {
            if (settingsButton != null)
                settingsButton.onClick.AddListener(ToggleSettings);
                
            if (closeButton != null)
                closeButton.onClick.AddListener(CloseSettings);
                
            if (metricUnitsToggle != null)
                metricUnitsToggle.onValueChanged.AddListener(OnUnitPreferenceChanged);
                
            if (privacyButton != null)
                privacyButton.onClick.AddListener(OpenPrivacyPolicy);
                
            if (deleteDesignsButton != null)
                deleteDesignsButton.onClick.AddListener(DeleteAllDesigns);
                
            if (resetOnboardingButton != null)
                resetOnboardingButton.onClick.AddListener(ResetOnboarding);
        }
        
        /// <summary>
        /// Loads saved settings from PlayerPrefs
        /// </summary>
        private void LoadSettings()
        {
            // Load unit preference (default to metric)
            bool useMetricUnits = PlayerPrefs.GetInt("UseMetricUnits", 1) == 1;
            if (metricUnitsToggle != null)
            {
                metricUnitsToggle.isOn = useMetricUnits;
            }
            
            // Apply unit preference to all product cards
            ApplyUnitPreference(useMetricUnits);
        }
        
        /// <summary>
        /// Saves settings to PlayerPrefs
        /// </summary>
        private void SaveSettings()
        {
            if (metricUnitsToggle != null)
            {
                PlayerPrefs.SetInt("UseMetricUnits", metricUnitsToggle.isOn ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
        
        /// <summary>
        /// Toggles the settings panel visibility
        /// </summary>
        public void ToggleSettings()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(!settingsPanel.activeSelf);
            }
        }
        
        /// <summary>
        /// Closes the settings panel
        /// </summary>
        public void CloseSettings()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
        }
        
        /// <summary>
        /// Handles unit preference toggle change
        /// </summary>
        /// <param name="useMetric">Whether to use metric units</param>
        private void OnUnitPreferenceChanged(bool useMetric)
        {
            ApplyUnitPreference(useMetric);
            SaveSettings();
        }
        
        /// <summary>
        /// Applies unit preference to all relevant UI elements
        /// </summary>
        /// <param name="useMetric">Whether to use metric units</param>
        private void ApplyUnitPreference(bool useMetric)
        {
            // Update all product cards
            if (productCards == null)
            {
                productCards = FindObjectsOfType<ProductCardController>();
            }
            
            foreach (ProductCardController card in productCards)
            {
                if (card != null)
                {
                    card.SetUnitPreference(useMetric);
                }
            }
        }
        
        /// <summary>
        /// Opens the privacy policy URL
        /// </summary>
        private void OpenPrivacyPolicy()
        {
            try
            {
                Application.OpenURL(privacyUrl);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error opening privacy policy: {e.Message}");
            }
        }
        
        /// <summary>
        /// Deletes all saved designs
        /// </summary>
        private void DeleteAllDesigns()
        {
            if (saveLoadService != null)
            {
                saveLoadService.DeleteAllSavedDesigns();
            }
        }
        
        /// <summary>
        /// Resets the onboarding flow
        /// </summary>
        private void ResetOnboarding()
        {
            if (onboardingController != null)
            {
                onboardingController.ResetOnboarding();
            }
        }
        
        /// <summary>
        /// Updates the version display
        /// </summary>
        private void UpdateVersionDisplay()
        {
            if (versionText != null)
            {
                versionText.text = $"Version {appVersion}";
            }
        }
        
        /// <summary>
        /// Gets the current unit preference
        /// </summary>
        /// <returns>True if using metric units</returns>
        public bool IsUsingMetricUnits()
        {
            return metricUnitsToggle != null ? metricUnitsToggle.isOn : true;
        }
        
        /// <summary>
        /// Sets the unit preference programmatically
        /// </summary>
        /// <param name="useMetric">Whether to use metric units</param>
        public void SetUnitPreference(bool useMetric)
        {
            if (metricUnitsToggle != null)
            {
                metricUnitsToggle.isOn = useMetric;
                OnUnitPreferenceChanged(useMetric);
            }
        }
        
        /// <summary>
        /// Shows the settings panel
        /// </summary>
        public void ShowSettings()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(true);
            }
        }
        
        /// <summary>
        /// Hides the settings panel
        /// </summary>
        public void HideSettings()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
        }
    }
}
