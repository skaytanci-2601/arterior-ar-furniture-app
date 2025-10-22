using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arterior.UI
{
    /// <summary>
    /// Controls the bottom sheet UI for the product catalog
    /// </summary>
    public class BottomSheetController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private RectTransform bottomSheet;
        [SerializeField] private Button dragHandle;
        [SerializeField] private ScrollRect scrollRect;
        
        [Header("Animation Settings")]
        [SerializeField] private float animationDuration = 0.3f;
        [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [Header("Sheet States")]
        [SerializeField] private float collapsedHeight = 100f;
        [SerializeField] private float expandedHeight = 600f;
        [SerializeField] private float halfExpandedHeight = 300f;
        
        private bool isAnimating = false;
        private Coroutine currentAnimation;
        
        public enum SheetState
        {
            Collapsed,
            HalfExpanded,
            Expanded
        }
        
        private SheetState currentState = SheetState.HalfExpanded;
        
        private void Start()
        {
            if (dragHandle != null)
            {
                dragHandle.onClick.AddListener(ToggleSheet);
            }
            
            // Set initial state
            SetSheetState(SheetState.HalfExpanded, false);
        }
        
        /// <summary>
        /// Toggles between collapsed and expanded states
        /// </summary>
        public void ToggleSheet()
        {
            if (currentState == SheetState.Collapsed)
            {
                SetSheetState(SheetState.HalfExpanded, true);
            }
            else
            {
                SetSheetState(SheetState.Collapsed, true);
            }
        }
        
        /// <summary>
        /// Sets the sheet to expanded state
        /// </summary>
        public void ExpandSheet()
        {
            SetSheetState(SheetState.Expanded, true);
        }
        
        /// <summary>
        /// Sets the sheet to collapsed state
        /// </summary>
        public void CollapseSheet()
        {
            SetSheetState(SheetState.Collapsed, true);
        }
        
        /// <summary>
        /// Sets the sheet to half-expanded state
        /// </summary>
        public void HalfExpandSheet()
        {
            SetSheetState(SheetState.HalfExpanded, true);
        }
        
        /// <summary>
        /// Sets the sheet state with optional animation
        /// </summary>
        /// <param name="state">Target state</param>
        /// <param name="animate">Whether to animate the transition</param>
        private void SetSheetState(SheetState state, bool animate)
        {
            if (isAnimating && currentAnimation != null)
            {
                StopCoroutine(currentAnimation);
            }
            
            currentState = state;
            float targetHeight = GetHeightForState(state);
            
            if (animate)
            {
                currentAnimation = StartCoroutine(AnimateToHeight(targetHeight));
            }
            else
            {
                SetSheetHeight(targetHeight);
            }
        }
        
        /// <summary>
        /// Gets the height for a given sheet state
        /// </summary>
        /// <param name="state">Sheet state</param>
        /// <returns>Height value</returns>
        private float GetHeightForState(SheetState state)
        {
            switch (state)
            {
                case SheetState.Collapsed:
                    return collapsedHeight;
                case SheetState.HalfExpanded:
                    return halfExpandedHeight;
                case SheetState.Expanded:
                    return expandedHeight;
                default:
                    return halfExpandedHeight;
            }
        }
        
        /// <summary>
        /// Animates the sheet to a target height
        /// </summary>
        /// <param name="targetHeight">Target height</param>
        /// <returns>Coroutine</returns>
        private IEnumerator AnimateToHeight(float targetHeight)
        {
            isAnimating = true;
            float startHeight = bottomSheet.sizeDelta.y;
            float elapsedTime = 0f;
            
            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / animationDuration;
                float easedProgress = animationCurve.Evaluate(progress);
                
                float currentHeight = Mathf.Lerp(startHeight, targetHeight, easedProgress);
                SetSheetHeight(currentHeight);
                
                yield return null;
            }
            
            SetSheetHeight(targetHeight);
            isAnimating = false;
            currentAnimation = null;
        }
        
        /// <summary>
        /// Sets the sheet height directly
        /// </summary>
        /// <param name="height">Height value</param>
        private void SetSheetHeight(float height)
        {
            if (bottomSheet != null)
            {
                bottomSheet.sizeDelta = new Vector2(bottomSheet.sizeDelta.x, height);
            }
        }
        
        /// <summary>
        /// Handles touch input for drag gestures
        /// </summary>
        private void Update()
        {
            HandleTouchInput();
        }
        
        /// <summary>
        /// Handles touch input for sheet manipulation
        /// </summary>
        private void HandleTouchInput()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                
                if (touch.phase == TouchPhase.Moved)
                {
                    // Simple drag handling - could be enhanced with proper gesture recognition
                    float deltaY = touch.deltaPosition.y;
                    
                    if (Mathf.Abs(deltaY) > 10f) // Threshold to avoid accidental triggers
                    {
                        if (deltaY > 0 && currentState != SheetState.Expanded)
                        {
                            // Dragging up - expand
                            if (currentState == SheetState.Collapsed)
                                SetSheetState(SheetState.HalfExpanded, true);
                            else if (currentState == SheetState.HalfExpanded)
                                SetSheetState(SheetState.Expanded, true);
                        }
                        else if (deltaY < 0 && currentState != SheetState.Collapsed)
                        {
                            // Dragging down - collapse
                            if (currentState == SheetState.Expanded)
                                SetSheetState(SheetState.HalfExpanded, true);
                            else if (currentState == SheetState.HalfExpanded)
                                SetSheetState(SheetState.Collapsed, true);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Gets the current sheet state
        /// </summary>
        /// <returns>Current sheet state</returns>
        public SheetState GetCurrentState()
        {
            return currentState;
        }
        
        /// <summary>
        /// Checks if the sheet is currently animating
        /// </summary>
        /// <returns>True if animating</returns>
        public bool IsAnimating()
        {
            return isAnimating;
        }
    }
}
