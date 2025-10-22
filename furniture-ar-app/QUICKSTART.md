# Arterior - Quick Start Guide

## Getting Started

This guide will help you set up and run the Arterior AR furniture placement app.

## Prerequisites

- Unity 2022 LTS or later
- iOS device with ARKit support (iPhone 6s or newer)
- Android device with ARCore support (Android 7.0+)

## Installation Steps

### 1. Unity Setup
1. Download and install Unity 2022 LTS
2. Open Unity Hub
3. Create a new 3D project named "Arterior"

### 2. Package Installation
1. Open Package Manager (`Window > Package Manager`)
2. Install the following packages:
   - AR Foundation
   - ARKit XR Plugin (for iOS)
   - ARCore XR Plugin (for Android)
   - XR Plugin Management

### 3. Project Configuration
1. Copy the provided Assets folder to your Unity project
2. Open `Assets/Arterior/Scenes/Main.unity`
3. Configure XR settings:
   - Go to `Edit > Project Settings > XR Plug-in Management`
   - Enable ARKit (iOS) and ARCore (Android)

### 4. Build Settings
1. Add Main scene to build (`File > Build Settings`)
2. Select target platform (iOS or Android)
3. Configure Player Settings:
   - Bundle Identifier: `com.yourcompany.arterior`
   - Camera Usage Description: "This app uses the camera for AR furniture placement"

## Running the App

### iOS Build
1. Switch to iOS platform
2. Build and Run to Xcode
3. Configure signing in Xcode
4. Deploy to ARKit-compatible device

### Android Build
1. Switch to Android platform
2. Build APK
3. Install on ARCore-compatible device

## Usage

1. **Launch**: Open the app and complete onboarding
2. **Scan**: Point camera at flat surface (floor/table)
3. **Place**: Tap catalog button, select furniture, tap to place
4. **Manipulate**: Drag to move, pinch to scale, twist to rotate
5. **Save**: Use save button to store your design

## Troubleshooting

- **No planes detected**: Ensure good lighting and flat surfaces
- **Objects not placing**: Check if plane detection is active
- **Performance issues**: Close other apps, restart device
- **Build errors**: Verify Unity version and package installation

## Support

For detailed documentation, see the main README.md file.

---

**Version**: 1.0.0  
**Unity**: 2022 LTS  
**Platforms**: iOS 11+, Android 7.0+