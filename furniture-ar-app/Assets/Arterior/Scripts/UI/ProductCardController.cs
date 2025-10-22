using UnityEngine;
using UnityEngine.UI;
using System;

namespace Arterior.UI
{
    /// <summary>
    /// Controls individual product cards in the catalog
    /// </summary>
    public class ProductCardController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image thumbnailImage;
        [SerializeField] private Text nameText;
        [SerializeField] private Text priceText;
        [SerializeField] private Text dimensionsText;
        [SerializeField] private Button selectButton;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button detailsButton;
        
        [Header("Settings")]
        [SerializeField] private bool useMetricUnits = true;
        
        private CatalogItem catalogItem;
        private CatalogController catalogController;
        
        private void Start()
        {
            SetupButtons();
        }
        
        /// <summary>
        /// Initializes the product card with catalog item data
        /// </summary>
        /// <param name="item">Catalog item data</param>
        /// <param name="controller">Catalog controller reference</param>
        public void Initialize(CatalogItem item, CatalogController controller)
        {
            catalogItem = item;
            catalogController = controller;
            
            UpdateDisplay();
        }
        
        /// <summary>
        /// Sets up button event listeners
        /// </summary>
        private void SetupButtons()
        {
            if (selectButton != null)
                selectButton.onClick.AddListener(SelectProduct);
                
            if (buyButton != null)
                buyButton.onClick.AddListener(BuyProduct);
                
            if (detailsButton != null)
                detailsButton.onClick.AddListener(ShowProductDetails);
        }
        
        /// <summary>
        /// Updates the display with current catalog item data
        /// </summary>
        private void UpdateDisplay()
        {
            if (catalogItem == null) return;
            
            // Update name
            if (nameText != null)
                nameText.text = catalogItem.name;
            
            // Update price
            if (priceText != null)
                priceText.text = $"${catalogItem.price:F2}";
            
            // Update dimensions
            if (dimensionsText != null)
            {
                string dimensions = FormatDimensions(catalogItem.dimensionsCm);
                dimensionsText.text = dimensions;
            }
            
            // Update thumbnail (placeholder for now)
            if (thumbnailImage != null)
            {
                // Create a simple colored rectangle as placeholder
                Texture2D placeholderTexture = CreatePlaceholderTexture();
                Sprite placeholderSprite = Sprite.Create(placeholderTexture, 
                    new Rect(0, 0, placeholderTexture.width, placeholderTexture.height), 
                    new Vector2(0.5f, 0.5f));
                thumbnailImage.sprite = placeholderSprite;
            }
        }
        
        /// <summary>
        /// Formats dimensions based on unit preference
        /// </summary>
        /// <param name="dimensionsCm">Dimensions in centimeters</param>
        /// <returns>Formatted dimension string</returns>
        private string FormatDimensions(Vector3 dimensionsCm)
        {
            if (useMetricUnits)
            {
                return $"{dimensionsCm.x:F0} × {dimensionsCm.y:F0} × {dimensionsCm.z:F0} cm";
            }
            else
            {
                Vector3 dimensionsIn = dimensionsCm / 2.54f; // Convert cm to inches
                return $"{dimensionsIn.x:F0} × {dimensionsIn.y:F0} × {dimensionsIn.z:F0} in";
            }
        }
        
        /// <summary>
        /// Creates a placeholder texture for the product thumbnail
        /// </summary>
        /// <returns>Placeholder texture</returns>
        private Texture2D CreatePlaceholderTexture()
        {
            Texture2D texture = new Texture2D(100, 100);
            Color color = Color.HSVToRGB(UnityEngine.Random.Range(0f, 1f), 0.3f, 0.8f);
            
            Color[] pixels = new Color[100 * 100];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }
            
            texture.SetPixels(pixels);
            texture.Apply();
            
            return texture;
        }
        
        /// <summary>
        /// Selects this product for placement
        /// </summary>
        private void SelectProduct()
        {
            if (catalogController != null && catalogItem != null)
            {
                catalogController.SelectProduct(catalogItem);
            }
        }
        
        /// <summary>
        /// Opens the product's buy URL
        /// </summary>
        private void BuyProduct()
        {
            if (catalogItem != null && !string.IsNullOrEmpty(catalogItem.buyUrl))
            {
                try
                {
                    Application.OpenURL(catalogItem.buyUrl);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error opening buy URL: {e.Message}");
                }
            }
        }
        
        /// <summary>
        /// Shows detailed product information
        /// </summary>
        private void ShowProductDetails()
        {
            // This would typically open a detailed product modal
            // For now, we'll just log the details
            if (catalogItem != null)
            {
                Debug.Log($"Product Details:\n" +
                         $"Name: {catalogItem.name}\n" +
                         $"Price: ${catalogItem.price:F2}\n" +
                         $"Dimensions: {FormatDimensions(catalogItem.dimensionsCm)}\n" +
                         $"Model: {catalogItem.modelPath}\n" +
                         $"Buy URL: {catalogItem.buyUrl}");
            }
        }
        
        /// <summary>
        /// Sets the unit preference for dimension display
        /// </summary>
        /// <param name="useMetric">True for metric units, false for imperial</param>
        public void SetUnitPreference(bool useMetric)
        {
            useMetricUnits = useMetric;
            UpdateDisplay();
        }
        
        /// <summary>
        /// Gets the catalog item associated with this card
        /// </summary>
        /// <returns>Catalog item data</returns>
        public CatalogItem GetCatalogItem()
        {
            return catalogItem;
        }
        
        /// <summary>
        /// Highlights the card to indicate selection
        /// </summary>
        /// <param name="highlighted">Whether to highlight the card</param>
        public void SetHighlighted(bool highlighted)
        {
            // Simple highlight implementation
            Image cardImage = GetComponent<Image>();
            if (cardImage != null)
            {
                cardImage.color = highlighted ? Color.yellow : Color.white;
            }
        }
    }
}
