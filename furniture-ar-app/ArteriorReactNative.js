// Arterior React Native App
import React, { useState, useRef } from 'react';
import { View, Text, TouchableOpacity, StyleSheet, Alert } from 'react-native';
import { ViroARScene, ViroARSceneNavigator, ViroBox, ViroMaterials } from '@viro-community/react-viro';

const ArteriorApp = () => {
  const [selectedFurniture, setSelectedFurniture] = useState(null);
  const [furnitureItems, setFurnitureItems] = useState([]);

  const furnitureCatalog = [
    { id: 'chair', name: 'Chair', color: '#8B4513', size: [0.5, 1, 0.5] },
    { id: 'table', name: 'Table', color: '#654321', size: [1, 0.5, 1] },
    { id: 'sofa', name: 'Sofa', color: '#4169E1', size: [2, 0.8, 1] },
  ];

  const ARScene = () => {
    return (
      <ViroARScene>
        {furnitureItems.map((item, index) => (
          <ViroBox
            key={index}
            position={item.position}
            materials={[item.color]}
            scale={item.size}
          />
        ))}
      </ViroARScene>
    );
  };

  const placeFurniture = (furniture) => {
    const newItem = {
      ...furniture,
      position: [0, 0, -1], // Place in front of camera
    };
    setFurnitureItems([...furnitureItems, newItem]);
    Alert.alert('Success', `${furniture.name} placed!`);
  };

  return (
    <View style={styles.container}>
      <ViroARSceneNavigator
        initialScene={{ scene: ARScene }}
        style={styles.arScene}
      />
      
      <View style={styles.uiPanel}>
        <Text style={styles.status}>Point camera at flat surface</Text>
        
        <View style={styles.buttonRow}>
          {furnitureCatalog.map((item) => (
            <TouchableOpacity
              key={item.id}
              style={styles.furnitureButton}
              onPress={() => placeFurniture(item)}
            >
              <Text style={styles.buttonText}>{item.name}</Text>
            </TouchableOpacity>
          ))}
        </View>
        
        <TouchableOpacity
          style={styles.clearButton}
          onPress={() => setFurnitureItems([])}
        >
          <Text style={styles.buttonText}>Clear All</Text>
        </TouchableOpacity>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  arScene: {
    flex: 1,
  },
  uiPanel: {
    position: 'absolute',
    bottom: 0,
    left: 0,
    right: 0,
    backgroundColor: 'rgba(0,0,0,0.8)',
    padding: 20,
  },
  status: {
    color: 'white',
    textAlign: 'center',
    marginBottom: 10,
    fontSize: 16,
  },
  buttonRow: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    marginBottom: 10,
  },
  furnitureButton: {
    backgroundColor: '#667eea',
    padding: 10,
    borderRadius: 5,
    minWidth: 80,
  },
  clearButton: {
    backgroundColor: '#e74c3c',
    padding: 10,
    borderRadius: 5,
    alignItems: 'center',
  },
  buttonText: {
    color: 'white',
    textAlign: 'center',
    fontWeight: 'bold',
  },
});

export default ArteriorApp;
