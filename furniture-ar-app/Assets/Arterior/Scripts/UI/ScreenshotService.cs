using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

namespace Arterior.UI
{
    /// <summary>
    /// Handles screenshot capture and sharing functionality
    /// </summary>
    public class ScreenshotService : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button screenshotButton;
        [SerializeField] private GameObject toastPrefab;
        [SerializeField] private Transform toastParent;
        
        [Header("Screenshot Settings")]
        [SerializeField] private string screenshotFolder = "Screenshots";
        [SerializeField] private string screenshotPrefix = "Arterior_";
        [SerializeField] private int screenshotQuality = 100;
        
        private Camera arCamera;
        private string screenshotPath;
        
        private void Start()
        {
            arCamera = Camera.main;
            screenshotPath = Path.Combine(Application.persistentDataPath, screenshotFolder);
            
            // Create screenshot directory if it doesn't exist
            if (!Directory.Exists(screenshotPath))
            {
                Directory.CreateDirectory(screenshotPath);
            }
            
            if (screenshotButton != null)
                screenshotButton.onClick.AddListener(CaptureScreenshot);
        }
        
        /// <summary>
        /// Captures a screenshot of the current AR view
        /// </summary>
        public void CaptureScreenshot()
        {
            StartCoroutine(CaptureScreenshotCoroutine());
        }
        
        /// <summary>
        /// Coroutine to capture screenshot
        /// </summary>
        /// <returns>Coroutine</returns>
        private IEnumerator CaptureScreenshotCoroutine()
        {
            // Wait for end of frame to ensure all rendering is complete
            yield return new WaitForEndOfFrame();
            
            // Create a texture to hold the screenshot
            Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            
            // Read the screen pixels
            screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshotTexture.Apply();
            
            // Convert to PNG
            byte[] pngData = screenshotTexture.EncodeToPNG();
            
            // Generate filename with timestamp
            string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filename = $"{screenshotPrefix}{timestamp}.png";
            string fullPath = Path.Combine(screenshotPath, filename);
            
            try
            {
                // Save the screenshot
                File.WriteAllBytes(fullPath, pngData);
                
                // Show success toast
                ShowToast($"Screenshot saved: {filename}");
                
                Debug.Log($"Screenshot saved to: {fullPath}");
                
                // Optional: Save to device gallery (platform-specific)
                SaveToGallery(fullPath);
            }
            catch (System.Exception e)
            {
                ShowToast($"Screenshot failed: {e.Message}");
                Debug.LogError($"Error saving screenshot: {e.Message}");
            }
            finally
            {
                // Clean up
                Destroy(screenshotTexture);
            }
        }
        
        /// <summary>
        /// Saves screenshot to device gallery (platform-specific implementation)
        /// </summary>
        /// <param name="filePath">Path to the screenshot file</param>
        private void SaveToGallery(string filePath)
        {
            // This is a placeholder for platform-specific gallery saving
            // On iOS, you would use Native Gallery plugin or similar
            // On Android, you would use MediaStore API
            
            #if UNITY_IOS
                // iOS-specific gallery saving code would go here
                Debug.Log("iOS: Saving to gallery not implemented");
            #elif UNITY_ANDROID
                // Android-specific gallery saving code would go here
                Debug.Log("Android: Saving to gallery not implemented");
            #else
                Debug.Log("Gallery saving not supported on this platform");
            #endif
        }
        
        /// <summary>
        /// Shows a toast message to the user
        /// </summary>
        /// <param name="message">Message to display</param>
        private void ShowToast(string message)
        {
            if (toastPrefab != null && toastParent != null)
            {
                GameObject toast = Instantiate(toastPrefab, toastParent);
                Text toastText = toast.GetComponentInChildren<Text>();
                
                if (toastText != null)
                {
                    toastText.text = message;
                }
                
                // Auto-destroy toast after 3 seconds
                Destroy(toast, 3f);
            }
            else
            {
                Debug.Log($"Toast: {message}");
            }
        }
        
        /// <summary>
        /// Gets the path to the screenshot directory
        /// </summary>
        /// <returns>Screenshot directory path</returns>
        public string GetScreenshotPath()
        {
            return screenshotPath;
        }
        
        /// <summary>
        /// Gets all screenshot files in the directory
        /// </summary>
        /// <returns>Array of screenshot file paths</returns>
        public string[] GetScreenshotFiles()
        {
            if (Directory.Exists(screenshotPath))
            {
                return Directory.GetFiles(screenshotPath, "*.png");
            }
            return new string[0];
        }
        
        /// <summary>
        /// Deletes all screenshots
        /// </summary>
        public void DeleteAllScreenshots()
        {
            try
            {
                string[] files = GetScreenshotFiles();
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                ShowToast($"Deleted {files.Length} screenshots");
            }
            catch (System.Exception e)
            {
                ShowToast($"Delete failed: {e.Message}");
                Debug.LogError($"Error deleting screenshots: {e.Message}");
            }
        }
        
        /// <summary>
        /// Shares a screenshot using the device's sharing capabilities
        /// </summary>
        /// <param name="filePath">Path to the screenshot file</param>
        public void ShareScreenshot(string filePath)
        {
            // This is a placeholder for platform-specific sharing
            // You would typically use a plugin like Native Share or similar
            
            #if UNITY_IOS || UNITY_ANDROID
                // Platform-specific sharing code would go here
                Debug.Log($"Sharing screenshot: {filePath}");
            #else
                Debug.Log("Sharing not supported on this platform");
            #endif
        }
    }
}
