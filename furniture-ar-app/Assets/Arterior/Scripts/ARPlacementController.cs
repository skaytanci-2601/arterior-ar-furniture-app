using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

namespace Arterior
{
    /// <summary>
    /// Controls AR plane detection and object placement functionality
    /// </summary>
    public class ARPlacementController : MonoBehaviour
    {
        [Header("AR Components")]
        [SerializeField] private ARRaycastManager arRaycastManager;
        [SerializeField] private ARPlaneManager arPlaneManager;
        [SerializeField] private Camera arCamera;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject placedObjectPrefab;
        [SerializeField] private GameObject planeVisualizerPrefab;
        
        [Header("UI References")]
        [SerializeField] private Text statusText;
        [SerializeField] private Button resetButton;
        
        private CatalogController catalogController;
        private List<GameObject> placedObjects = new List<GameObject>();
        private GameObject currentSelectedObject;
        
        // Placement state
        private bool isPlacementMode = false;
        private CatalogItem selectedItem;
        
        private void Start()
        {
            catalogController = FindObjectOfType<CatalogController>();
            
            if (resetButton != null)
                resetButton.onClick.AddListener(ResetScene);
                
            // Initialize AR plane detection
            if (arPlaneManager != null)
            {
                arPlaneManager.planesChanged += OnPlanesChanged;
            }
            
            UpdateStatusText("Move your device to detect surfaces");
        }
        
        private void Update()
        {
            HandleTouchInput();
        }
        
        /// <summary>
        /// Handles touch input for object placement and selection
        /// </summary>
        private void HandleTouchInput()
        {
            if (Input.touchCount == 0) return;
            
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 screenPosition = touch.position;
                
                if (isPlacementMode && selectedItem != null)
                {
                    TryPlaceSelectedModel(screenPosition);
                }
                else
                {
                    TrySelectObject(screenPosition);
                }
            }
        }
        
        /// <summary>
        /// Attempts to place the selected model at the given screen position
        /// </summary>
        /// <param name="screenPosition">Screen position to place object</param>
        public void TryPlaceSelectedModel(Vector2 screenPosition)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            
            if (arRaycastManager.Raycast(screenPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                ARRaycastHit hit = hits[0];
                Vector3 placementPosition = hit.pose.position;
                Quaternion placementRotation = hit.pose.rotation;
                
                // Spawn the placed object
                GameObject newObject = Instantiate(placedObjectPrefab, placementPosition, placementRotation);
                
                // Set up the object with the selected model
                ARObjectManipulator manipulator = newObject.GetComponent<ARObjectManipulator>();
                if (manipulator != null)
                {
                    manipulator.Initialize(selectedItem, this);
                }
                
                placedObjects.Add(newObject);
                UpdateStatusText($"Placed {selectedItem.name}");
                
                // Exit placement mode
                SetPlacementMode(false, null);
            }
            else
            {
                UpdateStatusText("No surface detected. Try moving your device.");
            }
        }
        
        /// <summary>
        /// Attempts to select an object at the given screen position
        /// </summary>
        /// <param name="screenPosition">Screen position to check for objects</param>
        private void TrySelectObject(Vector2 screenPosition)
        {
            Ray ray = arCamera.ScreenPointToRay(screenPosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                ARObjectManipulator manipulator = hit.collider.GetComponent<ARObjectManipulator>();
                if (manipulator != null)
                {
                    SelectObject(manipulator.gameObject);
                }
            }
            else
            {
                DeselectCurrentObject();
            }
        }
        
        /// <summary>
        /// Selects the given object
        /// </summary>
        /// <param name="obj">Object to select</param>
        public void SelectObject(GameObject obj)
        {
            DeselectCurrentObject();
            currentSelectedObject = obj;
            
            ARObjectManipulator manipulator = obj.GetComponent<ARObjectManipulator>();
            if (manipulator != null)
            {
                manipulator.SetSelected(true);
            }
        }
        
        /// <summary>
        /// Deselects the currently selected object
        /// </summary>
        public void DeselectCurrentObject()
        {
            if (currentSelectedObject != null)
            {
                ARObjectManipulator manipulator = currentSelectedObject.GetComponent<ARObjectManipulator>();
                if (manipulator != null)
                {
                    manipulator.SetSelected(false);
                }
                currentSelectedObject = null;
            }
        }
        
        /// <summary>
        /// Sets placement mode and selected item
        /// </summary>
        /// <param name="enabled">Whether placement mode is enabled</param>
        /// <param name="item">Item to place (null to disable)</param>
        public void SetPlacementMode(bool enabled, CatalogItem item)
        {
            isPlacementMode = enabled;
            selectedItem = item;
            
            if (enabled)
            {
                UpdateStatusText($"Tap to place {item.name}");
            }
            else
            {
                UpdateStatusText("Tap an object to select it");
            }
        }
        
        /// <summary>
        /// Removes an object from the scene
        /// </summary>
        /// <param name="obj">Object to remove</param>
        public void RemoveObject(GameObject obj)
        {
            if (placedObjects.Contains(obj))
            {
                placedObjects.Remove(obj);
                Destroy(obj);
            }
        }
        
        /// <summary>
        /// Resets the entire scene
        /// </summary>
        public void ResetScene()
        {
            foreach (GameObject obj in placedObjects)
            {
                if (obj != null)
                    Destroy(obj);
            }
            placedObjects.Clear();
            DeselectCurrentObject();
            UpdateStatusText("Scene reset");
        }
        
        /// <summary>
        /// Gets all placed objects for save/load functionality
        /// </summary>
        /// <returns>List of placed objects</returns>
        public List<GameObject> GetPlacedObjects()
        {
            return new List<GameObject>(placedObjects);
        }
        
        /// <summary>
        /// Loads objects from saved data
        /// </summary>
        /// <param name="savedItems">List of saved item data</param>
        public void LoadObjects(List<SavedItem> savedItems)
        {
            ResetScene();
            
            foreach (SavedItem savedItem in savedItems)
            {
                CatalogItem catalogItem = catalogController.GetItemById(savedItem.modelId);
                if (catalogItem != null)
                {
                    GameObject newObject = Instantiate(placedObjectPrefab, 
                        savedItem.position, 
                        Quaternion.Euler(savedItem.rotationEuler));
                    
                    ARObjectManipulator manipulator = newObject.GetComponent<ARObjectManipulator>();
                    if (manipulator != null)
                    {
                        manipulator.Initialize(catalogItem, this);
                        manipulator.SetScale(savedItem.scale);
                    }
                    
                    placedObjects.Add(newObject);
                }
            }
            
            UpdateStatusText($"Loaded {savedItems.Count} objects");
        }
        
        /// <summary>
        /// Updates the status text display
        /// </summary>
        /// <param name="message">Status message to display</param>
        private void UpdateStatusText(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
        }
        
        /// <summary>
        /// Called when AR planes change
        /// </summary>
        /// <param name="args">Plane change event arguments</param>
        private void OnPlanesChanged(ARPlanesChangedEventArgs args)
        {
            if (args.added.Count > 0)
            {
                UpdateStatusText("Surface detected! Tap to place objects.");
            }
        }
        
        private void OnDestroy()
        {
            if (arPlaneManager != null)
            {
                arPlaneManager.planesChanged -= OnPlanesChanged;
            }
        }
    }
}
