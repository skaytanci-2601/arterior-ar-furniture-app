using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Arterior
{
    /// <summary>
    /// Manages the product catalog and UI interactions
    /// </summary>
    public class CatalogController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject bottomSheet;
        [SerializeField] private Transform catalogContent;
        [SerializeField] private GameObject productCardPrefab;
        [SerializeField] private Button catalogToggleButton;
        
        [Header("Catalog Data")]
        [SerializeField] private string catalogJsonPath = "Arterior/Data/catalog.json";
        
        private CatalogData catalogData;
        private ARPlacementController placementController;
        private Dictionary<string, CatalogItem> itemLookup = new Dictionary<string, CatalogItem>();
        
        private void Start()
        {
            placementController = FindObjectOfType<ARPlacementController>();
            
            if (catalogToggleButton != null)
                catalogToggleButton.onClick.AddListener(ToggleBottomSheet);
                
            LoadCatalog();
            PopulateCatalogUI();
        }
        
        /// <summary>
        /// Loads the catalog data from JSON
        /// </summary>
        private void LoadCatalog()
        {
            try
            {
                TextAsset catalogAsset = Resources.Load<TextAsset>(catalogJsonPath.Replace(".json", ""));
                if (catalogAsset != null)
                {
                    catalogData = JsonUtility.FromJson<CatalogData>(catalogAsset.text);
                    BuildItemLookup();
                    Debug.Log($"Loaded catalog with {catalogData.categories.Count} categories");
                }
                else
                {
                    Debug.LogError($"Could not load catalog from {catalogJsonPath}");
                    CreateDefaultCatalog();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading catalog: {e.Message}");
                CreateDefaultCatalog();
            }
        }
        
        /// <summary>
        /// Creates a default catalog if loading fails
        /// </summary>
        private void CreateDefaultCatalog()
        {
            catalogData = new CatalogData
            {
                categories = new List<CatalogCategory>
                {
                    new CatalogCategory
                    {
                        id = "seating",
                        name = "Seating",
                        items = new List<CatalogItem>
                        {
                            new CatalogItem
                            {
                                id = "chair_001",
                                name = "Minimal Chair",
                                modelPath = "Arterior/Models/chair.glb",
                                thumbnailPath = "Arterior/Thumbnails/chair.png",
                                dimensionsCm = new Vector3(55, 85, 55),
                                price = 79.99f,
                                buyUrl = "https://example.com/chair-001"
                            },
                            new CatalogItem
                            {
                                id = "sofa_001",
                                name = "Modern Sofa",
                                modelPath = "Arterior/Models/sofa.glb",
                                thumbnailPath = "Arterior/Thumbnails/sofa.png",
                                dimensionsCm = new Vector3(200, 80, 90),
                                price = 599.99f,
                                buyUrl = "https://example.com/sofa-001"
                            }
                        }
                    },
                    new CatalogCategory
                    {
                        id = "tables",
                        name = "Tables",
                        items = new List<CatalogItem>
                        {
                            new CatalogItem
                            {
                                id = "table_001",
                                name = "Coffee Table",
                                modelPath = "Arterior/Models/table.glb",
                                thumbnailPath = "Arterior/Thumbnails/table.png",
                                dimensionsCm = new Vector3(120, 45, 60),
                                price = 199.99f,
                                buyUrl = "https://example.com/table-001"
                            }
                        }
                    },
                    new CatalogCategory
                    {
                        id = "lighting",
                        name = "Lighting",
                        items = new List<CatalogItem>
                        {
                            new CatalogItem
                            {
                                id = "lamp_001",
                                name = "Floor Lamp",
                                modelPath = "Arterior/Models/lamp.glb",
                                thumbnailPath = "Arterior/Thumbnails/lamp.png",
                                dimensionsCm = new Vector3(30, 160, 30),
                                price = 129.99f,
                                buyUrl = "https://example.com/lamp-001"
                            }
                        }
                    },
                    new CatalogCategory
                    {
                        id = "rugs",
                        name = "Rugs",
                        items = new List<CatalogItem>
                        {
                            new CatalogItem
                            {
                                id = "rug_001",
                                name = "Area Rug",
                                modelPath = "Arterior/Models/rug.glb",
                                thumbnailPath = "Arterior/Thumbnails/rug.png",
                                dimensionsCm = new Vector3(200, 1, 300),
                                price = 89.99f,
                                buyUrl = "https://example.com/rug-001"
                            }
                        }
                    }
                }
            };
            
            BuildItemLookup();
        }
        
        /// <summary>
        /// Builds a lookup dictionary for quick item access
        /// </summary>
        private void BuildItemLookup()
        {
            itemLookup.Clear();
            foreach (var category in catalogData.categories)
            {
                foreach (var item in category.items)
                {
                    itemLookup[item.id] = item;
                }
            }
        }
        
        /// <summary>
        /// Populates the catalog UI with product cards
        /// </summary>
        private void PopulateCatalogUI()
        {
            if (catalogContent == null || productCardPrefab == null) return;
            
            // Clear existing content
            foreach (Transform child in catalogContent)
            {
                Destroy(child.gameObject);
            }
            
            // Create product cards for each category
            foreach (var category in catalogData.categories)
            {
                // Create category header
                GameObject categoryHeader = CreateCategoryHeader(category.name);
                categoryHeader.transform.SetParent(catalogContent, false);
                
                // Create product cards for this category
                foreach (var item in category.items)
                {
                    GameObject productCard = CreateProductCard(item);
                    productCard.transform.SetParent(catalogContent, false);
                }
            }
        }
        
        /// <summary>
        /// Creates a category header UI element
        /// </summary>
        /// <param name="categoryName">Name of the category</param>
        /// <returns>Category header GameObject</returns>
        private GameObject CreateCategoryHeader(string categoryName)
        {
            GameObject header = new GameObject($"Category_{categoryName}");
            header.transform.SetParent(catalogContent, false);
            
            // Add Text component for category name
            Text text = header.AddComponent<Text>();
            text.text = categoryName;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 18;
            text.fontStyle = FontStyle.Bold;
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleLeft;
            
            // Add RectTransform
            RectTransform rectTransform = header.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 30);
            
            return header;
        }
        
        /// <summary>
        /// Creates a product card UI element
        /// </summary>
        /// <param name="item">Catalog item data</param>
        /// <returns>Product card GameObject</returns>
        private GameObject CreateProductCard(CatalogItem item)
        {
            GameObject card = Instantiate(productCardPrefab);
            
            // Set up the product card components
            ProductCardController cardController = card.GetComponent<ProductCardController>();
            if (cardController != null)
            {
                cardController.Initialize(item, this);
            }
            
            return card;
        }
        
        /// <summary>
        /// Toggles the bottom sheet visibility
        /// </summary>
        public void ToggleBottomSheet()
        {
            if (bottomSheet != null)
            {
                bottomSheet.SetActive(!bottomSheet.activeSelf);
            }
        }
        
        /// <summary>
        /// Selects a product for placement
        /// </summary>
        /// <param name="item">Catalog item to select</param>
        public void SelectProduct(CatalogItem item)
        {
            if (placementController != null)
            {
                placementController.SetPlacementMode(true, item);
            }
            
            // Close bottom sheet after selection
            if (bottomSheet != null)
            {
                bottomSheet.SetActive(false);
            }
        }
        
        /// <summary>
        /// Gets a catalog item by ID
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <returns>Catalog item or null if not found</returns>
        public CatalogItem GetItemById(string id)
        {
            return itemLookup.ContainsKey(id) ? itemLookup[id] : null;
        }
        
        /// <summary>
        /// Gets all catalog items
        /// </summary>
        /// <returns>List of all catalog items</returns>
        public List<CatalogItem> GetAllItems()
        {
            List<CatalogItem> allItems = new List<CatalogItem>();
            foreach (var category in catalogData.categories)
            {
                allItems.AddRange(category.items);
            }
            return allItems;
        }
    }
    
    /// <summary>
    /// Data structure for catalog information
    /// </summary>
    [System.Serializable]
    public class CatalogData
    {
        public List<CatalogCategory> categories;
    }
    
    /// <summary>
    /// Data structure for catalog categories
    /// </summary>
    [System.Serializable]
    public class CatalogCategory
    {
        public string id;
        public string name;
        public List<CatalogItem> items;
    }
    
    /// <summary>
    /// Data structure for catalog items
    /// </summary>
    [System.Serializable]
    public class CatalogItem
    {
        public string id;
        public string name;
        public string modelPath;
        public string thumbnailPath;
        public Vector3 dimensionsCm;
        public float price;
        public string buyUrl;
    }
    
    /// <summary>
    /// Data structure for saved items
    /// </summary>
    [System.Serializable]
    public class SavedItem
    {
        public string modelId;
        public Vector3 position;
        public Vector3 rotationEuler;
        public float scale;
    }
    
    /// <summary>
    /// Data structure for saved room data
    /// </summary>
    [System.Serializable]
    public class SavedRoomData
    {
        public int version = 1;
        public List<SavedItem> items;
    }
}
