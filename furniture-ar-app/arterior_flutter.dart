// Arterior Flutter App
import 'package:flutter/material.dart';
import 'package:arcore_flutter_plugin/arcore_flutter_plugin.dart';

class ArteriorApp extends StatefulWidget {
  @override
  _ArteriorAppState createState() => _ArteriorAppState();
}

class _ArteriorAppState extends State<ArteriorApp> {
  ArCoreController? arCoreController;
  List<ArCoreNode> furnitureNodes = [];
  
  final furnitureCatalog = [
    {'name': 'Chair', 'color': Colors.brown, 'size': [0.5, 1.0, 0.5]},
    {'name': 'Table', 'color': Colors.brown[800], 'size': [1.0, 0.5, 1.0]},
    {'name': 'Sofa', 'color': Colors.blue, 'size': [2.0, 0.8, 1.0]},
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Stack(
        children: [
          // AR View
          ArCoreView(
            onArCoreViewCreated: _onArCoreViewCreated,
            enableTapRecognizer: true,
          ),
          
          // UI Panel
          Positioned(
            bottom: 0,
            left: 0,
            right: 0,
            child: Container(
              color: Colors.black.withOpacity(0.8),
              padding: EdgeInsets.all(20),
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Text(
                    'Point camera at flat surface',
                    style: TextStyle(color: Colors.white, fontSize: 16),
                    textAlign: TextAlign.center,
                  ),
                  SizedBox(height: 10),
                  
                  // Furniture Buttons
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                    children: furnitureCatalog.map((item) {
                      return ElevatedButton(
                        onPressed: () => _placeFurniture(item),
                        style: ElevatedButton.styleFrom(
                          backgroundColor: Colors.blue[600],
                        ),
                        child: Text(
                          item['name'],
                          style: TextStyle(color: Colors.white),
                        ),
                      );
                    }).toList(),
                  ),
                  
                  SizedBox(height: 10),
                  
                  // Clear Button
                  ElevatedButton(
                    onPressed: _clearFurniture,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: Colors.red,
                    ),
                    child: Text(
                      'Clear All',
                      style: TextStyle(color: Colors.white),
                    ),
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  void _onArCoreViewCreated(ArCoreController controller) {
    arCoreController = controller;
    arCoreController!.onPlaneTap = _onPlaneTapped;
  }

  void _onPlaneTapped(List<ArCoreHitTestResult> hits) {
    if (hits.isEmpty) return;
    
    final hit = hits.first;
    final position = hit.pose.translation;
    
    // Place selected furniture at tapped position
    if (furnitureCatalog.isNotEmpty) {
      _placeFurnitureAtPosition(furnitureCatalog[0], position);
    }
  }

  void _placeFurniture(Map<String, dynamic> furniture) {
    if (arCoreController == null) return;
    
    // Place furniture at center of screen
    final position = [0.0, 0.0, -1.0];
    _placeFurnitureAtPosition(furniture, position);
  }

  void _placeFurnitureAtPosition(Map<String, dynamic> furniture, List<double> position) {
    final node = ArCoreNode(
      shape: ArCoreBox(
        size: furniture['size'],
        materials: [ArCoreMaterial(color: furniture['color'])],
      ),
      position: position,
    );
    
    arCoreController!.addArCoreNode(node);
    setState(() {
      furnitureNodes.add(node);
    });
    
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(content: Text('${furniture['name']} placed!')),
    );
  }

  void _clearFurniture() {
    for (var node in furnitureNodes) {
      arCoreController!.removeNode(nodeName: node.name);
    }
    setState(() {
      furnitureNodes.clear();
    });
  }

  @override
  void dispose() {
    arCoreController?.dispose();
    super.dispose();
  }
}

void main() {
  runApp(MaterialApp(
    home: ArteriorApp(),
    theme: ThemeData.dark(),
  ));
}
