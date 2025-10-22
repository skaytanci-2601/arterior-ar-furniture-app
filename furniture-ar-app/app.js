// AR Furniture Visualizer App
class FurnitureARApp {
    constructor() {
        this.scene = null;
        this.camera = null;
        this.renderer = null;
        this.controls = null;
        this.arButton = null;
        this.vrButton = null;
        this.isARMode = false;
        this.isVRMode = false;
        this.selectedFurniture = null;
        this.placedFurniture = [];
        this.furnitureLibrary = [];
        this.currentCategory = 'all';
        this.raycaster = new THREE.Raycaster();
        this.mouse = new THREE.Vector2();
        this.intersected = null;
        this.isDragging = false;
        this.dragOffset = new THREE.Vector3();
        
        this.init();
    }

    init() {
        this.setupScene();
        this.setupEventListeners();
        this.loadFurnitureLibrary();
        this.setupAR();
        this.animate();
    }

    setupScene() {
        // Create scene
        this.scene = new THREE.Scene();
        this.scene.background = new THREE.Color(0xf0f0f0);

        // Create camera
        this.camera = new THREE.PerspectiveCamera(
            75,
            window.innerWidth / window.innerHeight,
            0.1,
            1000
        );
        this.camera.position.set(5, 5, 5);

        // Create renderer
        const container = document.getElementById('viewport');
        this.renderer = new THREE.WebGLRenderer({ 
            antialias: true,
            alpha: true 
        });
        this.renderer.setSize(container.clientWidth, container.clientHeight);
        this.renderer.shadowMap.enabled = true;
        this.renderer.shadowMap.type = THREE.PCFSoftShadowMap;
        this.renderer.xr.enabled = true;
        container.appendChild(this.renderer.domElement);

        // Add controls for desktop mode
        this.controls = new THREE.OrbitControls(this.camera, this.renderer.domElement);
        this.controls.enableDamping = true;
        this.controls.dampingFactor = 0.05;
        this.controls.maxPolarAngle = Math.PI / 2;

        // Add lighting
        this.setupLighting();

        // Add floor grid
        this.addFloorGrid();

        // Handle window resize
        window.addEventListener('resize', () => this.onWindowResize());
    }

    setupLighting() {
        // Ambient light
        const ambientLight = new THREE.AmbientLight(0x404040, 0.6);
        this.scene.add(ambientLight);

        // Directional light (sun)
        const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
        directionalLight.position.set(10, 10, 5);
        directionalLight.castShadow = true;
        directionalLight.shadow.mapSize.width = 2048;
        directionalLight.shadow.mapSize.height = 2048;
        directionalLight.shadow.camera.near = 0.5;
        directionalLight.shadow.camera.far = 50;
        directionalLight.shadow.camera.left = -10;
        directionalLight.shadow.camera.right = 10;
        directionalLight.shadow.camera.top = 10;
        directionalLight.shadow.camera.bottom = -10;
        this.scene.add(directionalLight);

        // Point light for additional illumination
        const pointLight = new THREE.PointLight(0xffffff, 0.5, 100);
        pointLight.position.set(-10, 10, -10);
        this.scene.add(pointLight);
    }

    addFloorGrid() {
        const gridSize = 20;
        const gridDivisions = 20;
        const gridColor = 0x888888;
        
        const gridHelper = new THREE.GridHelper(gridSize, gridDivisions, gridColor, gridColor);
        gridHelper.position.y = 0;
        this.scene.add(gridHelper);

        // Add floor plane
        const floorGeometry = new THREE.PlaneGeometry(gridSize, gridSize);
        const floorMaterial = new THREE.MeshLambertMaterial({ 
            color: 0xffffff,
            transparent: true,
            opacity: 0.1
        });
        const floor = new THREE.Mesh(floorGeometry, floorMaterial);
        floor.rotation.x = -Math.PI / 2;
        floor.receiveShadow = true;
        this.scene.add(floor);
    }

    setupAR() {
        // Create AR button
        this.arButton = ARButton.createButton(this.renderer, {
            requiredFeatures: ['hit-test'],
            optionalFeatures: ['dom-overlay'],
            domOverlay: { root: document.body }
        });
        this.arButton.style.position = 'absolute';
        this.arButton.style.bottom = '20px';
        this.arButton.style.left = '50%';
        this.arButton.style.transform = 'translateX(-50%)';
        document.body.appendChild(this.arButton);

        // Handle AR session start
        this.renderer.xr.addEventListener('sessionstart', () => {
            this.isARMode = true;
            document.body.classList.add('ar-mode');
            this.setupARScene();
        });

        // Handle AR session end
        this.renderer.xr.addEventListener('sessionend', () => {
            this.isARMode = false;
            document.body.classList.remove('ar-mode');
            this.cleanupARScene();
        });
    }

    setupARScene() {
        // Add reticle for AR placement
        this.reticle = new THREE.Mesh(
            new THREE.RingGeometry(0.15, 0.2, 32).rotateX(-Math.PI / 2),
            new THREE.MeshBasicMaterial({ color: 0xffffff })
        );
        this.reticle.matrixAutoUpdate = false;
        this.reticle.visible = false;
        this.scene.add(this.reticle);

        // Add controller for AR interactions
        this.controller = this.renderer.xr.getController(0);
        this.controller.addEventListener('selectstart', () => this.onARSelectStart());
        this.controller.addEventListener('selectend', () => this.onARSelectEnd());
        this.scene.add(this.controller);

        // Add controller model
        const controllerModelFactory = new THREE.XRControllerModelFactory();
        const controllerGrip = this.renderer.xr.getControllerGrip(0);
        controllerGrip.add(controllerModelFactory.createControllerModel(controllerGrip));
        this.scene.add(controllerGrip);
    }

    cleanupARScene() {
        if (this.reticle) {
            this.scene.remove(this.reticle);
            this.reticle = null;
        }
        if (this.controller) {
            this.scene.remove(this.controller);
            this.controller = null;
        }
    }

    loadFurnitureLibrary() {
        // Sample furniture data - in a real app, this would come from an API
        this.furnitureLibrary = [
            {
                id: 'chair-1',
                name: 'Modern Chair',
                category: 'seating',
                price: '$299',
                icon: '🪑',
                geometry: () => this.createChairGeometry(),
                material: () => new THREE.MeshLambertMaterial({ color: 0x8B4513 })
            },
            {
                id: 'sofa-1',
                name: 'Luxury Sofa',
                category: 'seating',
                price: '$1,299',
                icon: '🛋️',
                geometry: () => this.createSofaGeometry(),
                material: () => new THREE.MeshLambertMaterial({ color: 0x4169E1 })
            },
            {
                id: 'table-1',
                name: 'Coffee Table',
                category: 'tables',
                price: '$599',
                icon: '🪑',
                geometry: () => this.createTableGeometry(),
                material: () => new THREE.MeshLambertMaterial({ color: 0x8B4513 })
            },
            {
                id: 'desk-1',
                name: 'Office Desk',
                category: 'tables',
                price: '$799',
                icon: '🪑',
                geometry: () => this.createDeskGeometry(),
                material: () => new THREE.MeshLambertMaterial({ color: 0x2F4F4F })
            },
            {
                id: 'bookshelf-1',
                name: 'Tall Bookshelf',
                category: 'storage',
                price: '$899',
                icon: '📚',
                geometry: () => this.createBookshelfGeometry(),
                material: () => new THREE.MeshLambertMaterial({ color: 0x8B4513 })
            },
            {
                id: 'lamp-1',
                name: 'Floor Lamp',
                category: 'decor',
                price: '$199',
                icon: '💡',
                geometry: () => this.createLampGeometry(),
                material: () => new THREE.MeshLambertMaterial({ color: 0xFFD700 })
            },
            {
                id: 'plant-1',
                name: 'Decorative Plant',
                category: 'decor',
                price: '$89',
                icon: '🪴',
                geometry: () => this.createPlantGeometry(),
                material: () => new THREE.MeshLambertMaterial({ color: 0x228B22 })
            }
        ];

        this.populateFurnitureGrid();
    }

    // Geometry creation methods
    createChairGeometry() {
        const group = new THREE.Group();
        
        // Seat
        const seatGeometry = new THREE.BoxGeometry(1, 0.1, 1);
        const seat = new THREE.Mesh(seatGeometry);
        seat.position.y = 0.5;
        group.add(seat);
        
        // Back
        const backGeometry = new THREE.BoxGeometry(1, 1, 0.1);
        const back = new THREE.Mesh(backGeometry);
        back.position.set(0, 1, -0.45);
        group.add(back);
        
        // Legs
        const legGeometry = new THREE.BoxGeometry(0.1, 0.5, 0.1);
        const positions = [
            [-0.4, 0.25, -0.4],
            [0.4, 0.25, -0.4],
            [-0.4, 0.25, 0.4],
            [0.4, 0.25, 0.4]
        ];
        
        positions.forEach(pos => {
            const leg = new THREE.Mesh(legGeometry);
            leg.position.set(...pos);
            group.add(leg);
        });
        
        return group;
    }

    createSofaGeometry() {
        const group = new THREE.Group();
        
        // Main body
        const bodyGeometry = new THREE.BoxGeometry(3, 1, 1.5);
        const body = new THREE.Mesh(bodyGeometry);
        body.position.y = 0.5;
        group.add(body);
        
        // Back
        const backGeometry = new THREE.BoxGeometry(3, 1.5, 0.2);
        const back = new THREE.Mesh(backGeometry);
        back.position.set(0, 1.25, -0.65);
        group.add(back);
        
        // Armrests
        const armGeometry = new THREE.BoxGeometry(0.3, 1, 1.5);
        const leftArm = new THREE.Mesh(armGeometry);
        leftArm.position.set(-1.35, 0.5, 0);
        group.add(leftArm);
        
        const rightArm = new THREE.Mesh(armGeometry);
        rightArm.position.set(1.35, 0.5, 0);
        group.add(rightArm);
        
        return group;
    }

    createTableGeometry() {
        const group = new THREE.Group();
        
        // Tabletop
        const topGeometry = new THREE.BoxGeometry(2, 0.1, 1);
        const top = new THREE.Mesh(topGeometry);
        top.position.y = 0.4;
        group.add(top);
        
        // Legs
        const legGeometry = new THREE.BoxGeometry(0.1, 0.4, 0.1);
        const positions = [
            [-0.9, 0.2, -0.4],
            [0.9, 0.2, -0.4],
            [-0.9, 0.2, 0.4],
            [0.9, 0.2, 0.4]
        ];
        
        positions.forEach(pos => {
            const leg = new THREE.Mesh(legGeometry);
            leg.position.set(...pos);
            group.add(leg);
        });
        
        return group;
    }

    createDeskGeometry() {
        const group = new THREE.Group();
        
        // Desktop
        const topGeometry = new THREE.BoxGeometry(2, 0.1, 1);
        const top = new THREE.Mesh(topGeometry);
        top.position.y = 0.75;
        group.add(top);
        
        // Legs
        const legGeometry = new THREE.BoxGeometry(0.1, 0.75, 0.1);
        const positions = [
            [-0.9, 0.375, -0.4],
            [0.9, 0.375, -0.4],
            [-0.9, 0.375, 0.4],
            [0.9, 0.375, 0.4]
        ];
        
        positions.forEach(pos => {
            const leg = new THREE.Mesh(legGeometry);
            leg.position.set(...pos);
            group.add(leg);
        });
        
        return group;
    }

    createBookshelfGeometry() {
        const group = new THREE.Group();
        
        // Main structure
        const structureGeometry = new THREE.BoxGeometry(1, 2, 0.1);
        const structure = new THREE.Mesh(structureGeometry);
        structure.position.set(0, 1, 0);
        group.add(structure);
        
        // Shelves
        for (let i = 0; i < 4; i++) {
            const shelfGeometry = new THREE.BoxGeometry(1, 0.05, 0.3);
            const shelf = new THREE.Mesh(shelfGeometry);
            shelf.position.set(0, 0.4 + i * 0.4, 0.1);
            group.add(shelf);
        }
        
        return group;
    }

    createLampGeometry() {
        const group = new THREE.Group();
        
        // Base
        const baseGeometry = new THREE.CylinderGeometry(0.2, 0.2, 0.1, 8);
        const base = new THREE.Mesh(baseGeometry);
        base.position.y = 0.05;
        group.add(base);
        
        // Pole
        const poleGeometry = new THREE.CylinderGeometry(0.05, 0.05, 1.5, 8);
        const pole = new THREE.Mesh(poleGeometry);
        pole.position.y = 0.8;
        group.add(pole);
        
        // Shade
        const shadeGeometry = new THREE.ConeGeometry(0.3, 0.4, 8);
        const shade = new THREE.Mesh(shadeGeometry);
        shade.position.y = 1.6;
        group.add(shade);
        
        return group;
    }

    createPlantGeometry() {
        const group = new THREE.Group();
        
        // Pot
        const potGeometry = new THREE.CylinderGeometry(0.3, 0.4, 0.4, 8);
        const pot = new THREE.Mesh(potGeometry);
        pot.position.y = 0.2;
        group.add(pot);
        
        // Plant
        const plantGeometry = new THREE.SphereGeometry(0.3, 8, 6);
        const plant = new THREE.Mesh(plantGeometry);
        plant.position.y = 0.6;
        plant.scale.y = 1.5;
        group.add(plant);
        
        return group;
    }

    populateFurnitureGrid() {
        const grid = document.getElementById('furniture-grid');
        grid.innerHTML = '';

        const filteredFurniture = this.currentCategory === 'all' 
            ? this.furnitureLibrary 
            : this.furnitureLibrary.filter(item => item.category === this.currentCategory);

        filteredFurniture.forEach(item => {
            const furnitureElement = document.createElement('div');
            furnitureElement.className = 'furniture-item';
            furnitureElement.dataset.furnitureId = item.id;
            
            furnitureElement.innerHTML = `
                <div class="furniture-preview">${item.icon}</div>
                <div class="furniture-name">${item.name}</div>
                <div class="furniture-price">${item.price}</div>
            `;
            
            furnitureElement.addEventListener('click', () => this.selectFurniture(item));
            grid.appendChild(furnitureElement);
        });
    }

    selectFurniture(furniture) {
        this.selectedFurniture = furniture;
        
        // Update UI
        document.querySelectorAll('.furniture-item').forEach(item => {
            item.classList.remove('selected');
        });
        document.querySelector(`[data-furniture-id="${furniture.id}"]`).classList.add('selected');
        
        // Update properties panel
        this.updatePropertiesPanel(furniture);
        
        // Change cursor to indicate placement mode
        document.body.style.cursor = 'crosshair';
    }

    updatePropertiesPanel(furniture) {
        const propertiesContent = document.getElementById('properties-content');
        propertiesContent.innerHTML = `
            <div class="property-group">
                <label class="property-label">Name</label>
                <input type="text" class="property-input" value="${furniture.name}" readonly>
            </div>
            <div class="property-group">
                <label class="property-label">Category</label>
                <input type="text" class="property-input" value="${furniture.category}" readonly>
            </div>
            <div class="property-group">
                <label class="property-label">Price</label>
                <input type="text" class="property-input" value="${furniture.price}" readonly>
            </div>
            <div class="property-group">
                <label class="property-label">Color</label>
                <input type="color" class="color-picker" value="#8B4513">
            </div>
            <div class="property-group">
                <label class="property-label">Scale</label>
                <input type="range" class="property-slider" min="0.5" max="2" step="0.1" value="1">
            </div>
            <div class="property-group">
                <label class="property-label">Rotation</label>
                <input type="range" class="property-slider" min="0" max="360" step="5" value="0">
            </div>
        `;
    }

    setupEventListeners() {
        // Mode toggle
        document.getElementById('desktop-mode').addEventListener('click', () => {
            this.setMode('desktop');
        });
        
        document.getElementById('ar-mode').addEventListener('click', () => {
            this.setMode('ar');
        });

        // Category tabs
        document.querySelectorAll('.tab-btn').forEach(btn => {
            btn.addEventListener('click', () => {
                document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
                btn.classList.add('active');
                this.currentCategory = btn.dataset.category;
                this.populateFurnitureGrid();
            });
        });

        // Viewport controls
        document.getElementById('reset-view').addEventListener('click', () => {
            this.resetView();
        });
        
        document.getElementById('toggle-grid').addEventListener('click', () => {
            this.toggleGrid();
        });
        
        document.getElementById('save-scene').addEventListener('click', () => {
            this.saveScene();
        });
        
        document.getElementById('load-scene').addEventListener('click', () => {
            this.loadScene();
        });

        // Mouse events for desktop mode
        this.renderer.domElement.addEventListener('click', (event) => {
            if (!this.isARMode && this.selectedFurniture) {
                this.placeFurniture(event);
            }
        });

        this.renderer.domElement.addEventListener('mousemove', (event) => {
            if (!this.isARMode) {
                this.handleMouseMove(event);
            }
        });

        // AR instructions
        document.getElementById('start-ar').addEventListener('click', () => {
            this.startAR();
        });
        
        document.getElementById('close-instructions').addEventListener('click', () => {
            this.closeARInstructions();
        });
    }

    setMode(mode) {
        document.querySelectorAll('.mode-btn').forEach(btn => btn.classList.remove('active'));
        document.getElementById(`${mode}-mode`).classList.add('active');
        
        if (mode === 'ar') {
            this.showARInstructions();
        }
    }

    showARInstructions() {
        document.getElementById('ar-instructions').style.display = 'block';
    }

    closeARInstructions() {
        document.getElementById('ar-instructions').style.display = 'none';
    }

    startAR() {
        this.closeARInstructions();
        if (this.arButton) {
            this.arButton.click();
        }
    }

    placeFurniture(event) {
        if (!this.selectedFurniture) return;

        // Calculate mouse position in normalized device coordinates
        const rect = this.renderer.domElement.getBoundingClientRect();
        this.mouse.x = ((event.clientX - rect.left) / rect.width) * 2 - 1;
        this.mouse.y = -((event.clientY - rect.top) / rect.height) * 2 + 1;

        // Create raycaster
        this.raycaster.setFromCamera(this.mouse, this.camera);

        // Intersect with floor plane
        const floorPlane = new THREE.Plane(new THREE.Vector3(0, 1, 0), 0);
        const intersectionPoint = new THREE.Vector3();
        this.raycaster.ray.intersectPlane(floorPlane, intersectionPoint);

        if (intersectionPoint) {
            this.createFurnitureObject(intersectionPoint);
        }
    }

    createFurnitureObject(position) {
        const furnitureData = this.selectedFurniture;
        const geometry = furnitureData.geometry();
        const material = furnitureData.material();
        
        // Apply material to all meshes in the group
        geometry.traverse((child) => {
            if (child.isMesh) {
                child.material = material.clone();
                child.castShadow = true;
                child.receiveShadow = true;
            }
        });

        const furnitureObject = new THREE.Object3D();
        furnitureObject.add(geometry);
        furnitureObject.position.copy(position);
        furnitureObject.userData = {
            id: furnitureData.id,
            name: furnitureData.name,
            category: furnitureData.category,
            price: furnitureData.price
        };

        this.scene.add(furnitureObject);
        this.placedFurniture.push(furnitureObject);

        // Reset cursor
        document.body.style.cursor = 'default';
    }

    handleMouseMove(event) {
        const rect = this.renderer.domElement.getBoundingClientRect();
        this.mouse.x = ((event.clientX - rect.left) / rect.width) * 2 - 1;
        this.mouse.y = -((event.clientY - rect.top) / rect.height) * 2 + 1;

        this.raycaster.setFromCamera(this.mouse, this.camera);
        const intersects = this.raycaster.intersectObjects(this.placedFurniture);

        if (intersects.length > 0) {
            if (this.intersected !== intersects[0].object) {
                this.intersected = intersects[0].object;
                document.body.style.cursor = 'pointer';
            }
        } else {
            if (this.intersected) {
                this.intersected = null;
                document.body.style.cursor = this.selectedFurniture ? 'crosshair' : 'default';
            }
        }
    }

    onARSelectStart() {
        if (this.selectedFurniture && this.reticle.visible) {
            const position = new THREE.Vector3();
            this.reticle.getWorldPosition(position);
            this.createFurnitureObject(position);
        }
    }

    onARSelectEnd() {
        // Handle AR selection end if needed
    }

    resetView() {
        this.camera.position.set(5, 5, 5);
        this.controls.target.set(0, 0, 0);
        this.controls.update();
    }

    toggleGrid() {
        const grid = this.scene.getObjectByName('grid');
        if (grid) {
            grid.visible = !grid.visible;
        }
    }

    saveScene() {
        const sceneData = {
            furniture: this.placedFurniture.map(obj => ({
                id: obj.userData.id,
                position: obj.position.toArray(),
                rotation: obj.rotation.toArray(),
                scale: obj.scale.toArray()
            }))
        };
        
        localStorage.setItem('furnitureScene', JSON.stringify(sceneData));
        alert('Scene saved successfully!');
    }

    loadScene() {
        const sceneData = localStorage.getItem('furnitureScene');
        if (sceneData) {
            const data = JSON.parse(sceneData);
            
            // Clear existing furniture
            this.placedFurniture.forEach(obj => this.scene.remove(obj));
            this.placedFurniture = [];
            
            // Load saved furniture
            data.furniture.forEach(item => {
                const furnitureData = this.furnitureLibrary.find(f => f.id === item.id);
                if (furnitureData) {
                    const geometry = furnitureData.geometry();
                    const material = furnitureData.material();
                    
                    geometry.traverse((child) => {
                        if (child.isMesh) {
                            child.material = material.clone();
                            child.castShadow = true;
                            child.receiveShadow = true;
                        }
                    });

                    const furnitureObject = new THREE.Object3D();
                    furnitureObject.add(geometry);
                    furnitureObject.position.fromArray(item.position);
                    furnitureObject.rotation.fromArray(item.rotation);
                    furnitureObject.scale.fromArray(item.scale);
                    furnitureObject.userData = furnitureData;

                    this.scene.add(furnitureObject);
                    this.placedFurniture.push(furnitureObject);
                }
            });
            
            alert('Scene loaded successfully!');
        } else {
            alert('No saved scene found!');
        }
    }

    onWindowResize() {
        const container = document.getElementById('viewport');
        this.camera.aspect = container.clientWidth / container.clientHeight;
        this.camera.updateProjectionMatrix();
        this.renderer.setSize(container.clientWidth, container.clientHeight);
    }

    animate() {
        requestAnimationFrame(() => this.animate());
        
        if (!this.isARMode) {
            this.controls.update();
        }
        
        this.renderer.render(this.scene, this.camera);
    }
}

// Initialize the app when the page loads
document.addEventListener('DOMContentLoaded', () => {
    // Hide loading screen
    setTimeout(() => {
        document.getElementById('loading-screen').style.display = 'none';
        new FurnitureARApp();
    }, 2000);
});
