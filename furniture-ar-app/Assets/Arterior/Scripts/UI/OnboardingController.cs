using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arterior.UI
{
    /// <summary>
    /// Controls the onboarding flow for new users
    /// </summary>
    public class OnboardingController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject onboardingPanel;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button skipButton;
        [SerializeField] private Button getStartedButton;
        [SerializeField] private Text titleText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Image illustrationImage;
        [SerializeField] private GameObject[] stepIndicators;
        
        [Header("Onboarding Content")]
        [SerializeField] private OnboardingStep[] steps;
        
        private int currentStep = 0;
        private bool hasCompletedOnboarding = false;
        
        [System.Serializable]
        public class OnboardingStep
        {
            public string title;
            public string description;
            public Sprite illustration;
        }
        
        private void Start()
        {
            // Check if user has completed onboarding
            hasCompletedOnboarding = PlayerPrefs.GetInt("OnboardingCompleted", 0) == 1;
            
            if (!hasCompletedOnboarding)
            {
                ShowOnboarding();
            }
            else
            {
                HideOnboarding();
            }
            
            SetupButtons();
        }
        
        /// <summary>
        /// Sets up button event listeners
        /// </summary>
        private void SetupButtons()
        {
            if (nextButton != null)
                nextButton.onClick.AddListener(NextStep);
                
            if (skipButton != null)
                skipButton.onClick.AddListener(SkipOnboarding);
                
            if (getStartedButton != null)
                getStartedButton.onClick.AddListener(CompleteOnboarding);
        }
        
        /// <summary>
        /// Shows the onboarding panel
        /// </summary>
        public void ShowOnboarding()
        {
            if (onboardingPanel != null)
            {
                onboardingPanel.SetActive(true);
                currentStep = 0;
                UpdateStepDisplay();
            }
        }
        
        /// <summary>
        /// Hides the onboarding panel
        /// </summary>
        public void HideOnboarding()
        {
            if (onboardingPanel != null)
            {
                onboardingPanel.SetActive(false);
            }
        }
        
        /// <summary>
        /// Moves to the next onboarding step
        /// </summary>
        public void NextStep()
        {
            if (currentStep < steps.Length - 1)
            {
                currentStep++;
                UpdateStepDisplay();
            }
            else
            {
                CompleteOnboarding();
            }
        }
        
        /// <summary>
        /// Moves to the previous onboarding step
        /// </summary>
        public void PreviousStep()
        {
            if (currentStep > 0)
            {
                currentStep--;
                UpdateStepDisplay();
            }
        }
        
        /// <summary>
        /// Skips the onboarding process
        /// </summary>
        public void SkipOnboarding()
        {
            CompleteOnboarding();
        }
        
        /// <summary>
        /// Completes the onboarding process
        /// </summary>
        public void CompleteOnboarding()
        {
            hasCompletedOnboarding = true;
            PlayerPrefs.SetInt("OnboardingCompleted", 1);
            PlayerPrefs.Save();
            
            HideOnboarding();
            
            // Request camera permission
            RequestCameraPermission();
        }
        
        /// <summary>
        /// Updates the display for the current step
        /// </summary>
        private void UpdateStepDisplay()
        {
            if (currentStep >= 0 && currentStep < steps.Length)
            {
                OnboardingStep step = steps[currentStep];
                
                if (titleText != null)
                    titleText.text = step.title;
                    
                if (descriptionText != null)
                    descriptionText.text = step.description;
                    
                if (illustrationImage != null && step.illustration != null)
                    illustrationImage.sprite = step.illustration;
                
                // Update step indicators
                UpdateStepIndicators();
                
                // Update button visibility
                UpdateButtonVisibility();
            }
        }
        
        /// <summary>
        /// Updates the step indicator dots
        /// </summary>
        private void UpdateStepIndicators()
        {
            if (stepIndicators != null)
            {
                for (int i = 0; i < stepIndicators.Length; i++)
                {
                    if (stepIndicators[i] != null)
                    {
                        stepIndicators[i].SetActive(i == currentStep);
                    }
                }
            }
        }
        
        /// <summary>
        /// Updates button visibility based on current step
        /// </summary>
        private void UpdateButtonVisibility()
        {
            bool isLastStep = currentStep == steps.Length - 1;
            
            if (nextButton != null)
                nextButton.gameObject.SetActive(!isLastStep);
                
            if (getStartedButton != null)
                getStartedButton.gameObject.SetActive(isLastStep);
        }
        
        /// <summary>
        /// Requests camera permission from the user
        /// </summary>
        private void RequestCameraPermission()
        {
            // This would typically use Unity's permission system
            // For now, we'll just log the request
            Debug.Log("Requesting camera permission for AR functionality");
            
            // In a real implementation, you would use:
            // UnityEngine.Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }
        
        /// <summary>
        /// Resets onboarding completion status
        /// </summary>
        public void ResetOnboarding()
        {
            hasCompletedOnboarding = false;
            PlayerPrefs.SetInt("OnboardingCompleted", 0);
            PlayerPrefs.Save();
            ShowOnboarding();
        }
        
        /// <summary>
        /// Checks if onboarding has been completed
        /// </summary>
        /// <returns>True if onboarding is completed</returns>
        public bool IsOnboardingCompleted()
        {
            return hasCompletedOnboarding;
        }
        
        /// <summary>
        /// Gets the current step index
        /// </summary>
        /// <returns>Current step index</returns>
        public int GetCurrentStep()
        {
            return currentStep;
        }
    }
}
