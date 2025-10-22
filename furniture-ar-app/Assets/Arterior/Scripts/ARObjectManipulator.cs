using UnityEngine;
using UnityEngine.UI;

namespace Arterior
{
    /// <summary>
    /// Handles manipulation (move, scale, rotate) of placed AR objects
    /// </summary>
    public class ARObjectManipulator : MonoBehaviour
    {
        [Header("Manipulation Settings")]
        [SerializeField] private float minScale = 0.5f;
        [SerializeField] private float maxScale = 2.0f;
        [SerializeField] private float rotationSnapAngle = 5f;
        [SerializeField] private bool snapRotation = false;
        
        [Header("Visual Feedback")]
        [SerializeField] private GameObject selectionOutline;
        [SerializeField] private GameObject manipulationHandles;
        [SerializeField] private Button deleteButton;
        
        private CatalogItem catalogItem;
        private ARPlacementController placementController;
        private bool isSelected = false;
        private bool isManipulating = false;
        
        // Touch handling
        private Vector2 lastTouchPosition;
        private float lastTouchDistance;
        private Vector3 lastTouchAngle;
        private int touchCount = 0;
        
        // Manipulation state
        private Vector3 initialPosition;
        private Vector3 initialScale;
        private Vector3 initialRotation;
        
        private void Start()
        {
            if (deleteButton != null)
                deleteButton.onClick.AddListener(DeleteObject);
                
            SetSelected(false);
        }
        
        private void Update()
        {
            HandleTouchInput();
        }
        
        /// <summary>
        /// Initializes the object with catalog item data
        /// </summary>
        /// <param name="item">Catalog item data</param>
        /// <param name="controller">AR placement controller reference</param>
        public void Initialize(CatalogItem item, ARPlacementController controller)
        {
            catalogItem = item;
            placementController = controller;
            
            // Load the 3D model (placeholder for now)
            LoadModel();
        }
        
        /// <summary>
        /// Loads the 3D model for this object
        /// </summary>
        private void LoadModel()
        {
            // TODO: Implement actual model loading using Addressables or Resources
            // For now, create a simple placeholder cube
            GameObject model = GameObject.CreatePrimitive(PrimitiveType.Cube);
            model.transform.SetParent(transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
            
            // Add a simple material
            Renderer renderer = model.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Standard"));
                mat.color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
                renderer.material = mat;
            }
        }
        
        /// <summary>
        /// Handles touch input for object manipulation
        /// </summary>
        private void HandleTouchInput()
        {
            if (!isSelected) return;
            
            int currentTouchCount = Input.touchCount;
            
            if (currentTouchCount == 0)
            {
                if (isManipulating)
                {
                    EndManipulation();
                }
                return;
            }
            
            if (currentTouchCount == 1)
            {
                HandleSingleTouch();
            }
            else if (currentTouchCount == 2)
            {
                HandleMultiTouch();
            }
        }
        
        /// <summary>
        /// Handles single touch input for movement
        /// </summary>
        private void HandleSingleTouch()
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                StartManipulation();
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved && isManipulating)
            {
                MoveObject(touch.position);
            }
        }
        
        /// <summary>
        /// Handles multi-touch input for scaling and rotation
        /// </summary>
        private void HandleMultiTouch()
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            
            Vector2 currentTouchPosition1 = touch1.position;
            Vector2 currentTouchPosition2 = touch2.position;
            
            float currentDistance = Vector2.Distance(currentTouchPosition1, currentTouchPosition2);
            Vector2 currentCenter = (currentTouchPosition1 + currentTouchPosition2) / 2f;
            
            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                StartManipulation();
                lastTouchDistance = currentDistance;
                lastTouchPosition = currentCenter;
            }
            else if ((touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved) && isManipulating)
            {
                // Scale based on distance change
                if (lastTouchDistance > 0)
                {
                    float scaleFactor = currentDistance / lastTouchDistance;
                    ScaleObject(scaleFactor);
                }
                
                // Rotate based on angle change
                Vector2 lastVector = lastTouchPosition - currentCenter;
                Vector2 currentVector = currentCenter - currentCenter;
                
                if (lastVector.magnitude > 10f)
                {
                    float angle = Vector2.SignedAngle(lastVector, currentVector);
                    RotateObject(angle);
                }
                
                lastTouchDistance = currentDistance;
                lastTouchPosition = currentCenter;
            }
        }
        
        /// <summary>
        /// Starts manipulation mode
        /// </summary>
        private void StartManipulation()
        {
            if (!isManipulating)
            {
                isManipulating = true;
                initialPosition = transform.position;
                initialScale = transform.localScale;
                initialRotation = transform.eulerAngles;
            }
        }
        
        /// <summary>
        /// Ends manipulation mode
        /// </summary>
        private void EndManipulation()
        {
            isManipulating = false;
        }
        
        /// <summary>
        /// Moves the object based on touch input
        /// </summary>
        /// <param name="touchPosition">Current touch position</param>
        private void MoveObject(Vector2 touchPosition)
        {
            // Project touch position to world position on the ground plane
            Camera cam = Camera.main;
            if (cam == null) return;
            
            Ray ray = cam.ScreenPointToRay(touchPosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            
            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 worldPosition = ray.GetPoint(distance);
                transform.position = worldPosition;
            }
        }
        
        /// <summary>
        /// Scales the object based on pinch gesture
        /// </summary>
        /// <param name="scaleFactor">Scale factor to apply</param>
        private void ScaleObject(float scaleFactor)
        {
            Vector3 newScale = transform.localScale * scaleFactor;
            newScale = Vector3.Max(Vector3.one * minScale, Vector3.Min(Vector3.one * maxScale, newScale));
            transform.localScale = newScale;
        }
        
        /// <summary>
        /// Rotates the object based on twist gesture
        /// </summary>
        /// <param name="angle">Rotation angle in degrees</param>
        private void RotateObject(float angle)
        {
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.y += angle;
            
            if (snapRotation)
            {
                currentRotation.y = Mathf.Round(currentRotation.y / rotationSnapAngle) * rotationSnapAngle;
            }
            
            transform.eulerAngles = currentRotation;
        }
        
        /// <summary>
        /// Sets the selection state of the object
        /// </summary>
        /// <param name="selected">Whether the object is selected</param>
        public void SetSelected(bool selected)
        {
            isSelected = selected;
            
            if (selectionOutline != null)
                selectionOutline.SetActive(selected);
                
            if (manipulationHandles != null)
                manipulationHandles.SetActive(selected);
        }
        
        /// <summary>
        /// Sets the scale of the object
        /// </summary>
        /// <param name="scale">Scale value</param>
        public void SetScale(float scale)
        {
            scale = Mathf.Clamp(scale, minScale, maxScale);
            transform.localScale = Vector3.one * scale;
        }
        
        /// <summary>
        /// Deletes this object from the scene
        /// </summary>
        private void DeleteObject()
        {
            if (placementController != null)
            {
                placementController.RemoveObject(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Gets the catalog item associated with this object
        /// </summary>
        /// <returns>Catalog item data</returns>
        public CatalogItem GetCatalogItem()
        {
            return catalogItem;
        }
        
        /// <summary>
        /// Gets the current transform data for saving
        /// </summary>
        /// <returns>Saved item data</returns>
        public SavedItem GetSaveData()
        {
            return new SavedItem
            {
                modelId = catalogItem?.id ?? "",
                position = transform.position,
                rotationEuler = transform.eulerAngles,
                scale = transform.localScale.x
            };
        }
    }
}
