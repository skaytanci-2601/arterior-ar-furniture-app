using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Arterior
{
    /// <summary>
    /// Handles saving and loading of room designs
    /// </summary>
    public class SaveLoadService : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Text statusText;
        
        [Header("Save Settings")]
        [SerializeField] private string saveFileName = "saved_room.json";
        [SerializeField] private int maxSaveSlots = 5;
        
        private ARPlacementController placementController;
        private string savePath;
        
        private void Start()
        {
            placementController = FindObjectOfType<ARPlacementController>();
            savePath = Path.Combine(Application.persistentDataPath, saveFileName);
            
            if (saveButton != null)
                saveButton.onClick.AddListener(SaveDesign);
                
            if (loadButton != null)
                loadButton.onClick.AddListener(LoadDesign);
                
            if (resetButton != null)
                resetButton.onClick.AddListener(ResetDesign);
        }
        
        /// <summary>
        /// Saves the current room design
        /// </summary>
        public void SaveDesign()
        {
            if (placementController == null)
            {
                UpdateStatus("Error: Placement controller not found");
                return;
            }
            
            List<GameObject> placedObjects = placementController.GetPlacedObjects();
            if (placedObjects.Count == 0)
            {
                UpdateStatus("No objects to save");
                return;
            }
            
            SavedRoomData roomData = new SavedRoomData
            {
                version = 1,
                items = new List<SavedItem>()
            };
            
            foreach (GameObject obj in placedObjects)
            {
                ARObjectManipulator manipulator = obj.GetComponent<ARObjectManipulator>();
                if (manipulator != null)
                {
                    roomData.items.Add(manipulator.GetSaveData());
                }
            }
            
            try
            {
                string json = JsonUtility.ToJson(roomData, true);
                File.WriteAllText(savePath, json);
                UpdateStatus($"Saved {roomData.items.Count} objects");
                Debug.Log($"Room saved to: {savePath}");
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Save failed: {e.Message}");
                Debug.LogError($"Error saving room: {e.Message}");
            }
        }
        
        /// <summary>
        /// Loads a saved room design
        /// </summary>
        public void LoadDesign()
        {
            if (!File.Exists(savePath))
            {
                UpdateStatus("No saved design found");
                return;
            }
            
            try
            {
                string json = File.ReadAllText(savePath);
                SavedRoomData roomData = JsonUtility.FromJson<SavedRoomData>(json);
                
                if (roomData == null || roomData.items == null)
                {
                    UpdateStatus("Invalid save file");
                    return;
                }
                
                if (placementController != null)
                {
                    placementController.LoadObjects(roomData.items);
                    UpdateStatus($"Loaded {roomData.items.Count} objects");
                }
                else
                {
                    UpdateStatus("Error: Placement controller not found");
                }
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Load failed: {e.Message}");
                Debug.LogError($"Error loading room: {e.Message}");
            }
        }
        
        /// <summary>
        /// Resets the current design
        /// </summary>
        public void ResetDesign()
        {
            if (placementController != null)
            {
                placementController.ResetScene();
                UpdateStatus("Design reset");
            }
        }
        
        /// <summary>
        /// Deletes all saved designs
        /// </summary>
        public void DeleteAllSavedDesigns()
        {
            try
            {
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                    UpdateStatus("All saved designs deleted");
                }
                else
                {
                    UpdateStatus("No saved designs to delete");
                }
            }
            catch (System.Exception e)
            {
                UpdateStatus($"Delete failed: {e.Message}");
                Debug.LogError($"Error deleting saved designs: {e.Message}");
            }
        }
        
        /// <summary>
        /// Checks if a saved design exists
        /// </summary>
        /// <returns>True if saved design exists</returns>
        public bool HasSavedDesign()
        {
            return File.Exists(savePath);
        }
        
        /// <summary>
        /// Gets information about the saved design
        /// </summary>
        /// <returns>Save file info or null if not found</returns>
        public FileInfo GetSaveFileInfo()
        {
            if (File.Exists(savePath))
            {
                return new FileInfo(savePath);
            }
            return null;
        }
        
        /// <summary>
        /// Updates the status text display
        /// </summary>
        /// <param name="message">Status message</param>
        private void UpdateStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
            Debug.Log($"SaveLoadService: {message}");
        }
    }
}
