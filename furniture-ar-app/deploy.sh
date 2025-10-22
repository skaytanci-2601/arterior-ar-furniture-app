#!/bin/bash

# Arterior AR App - Build and Deploy Script
# This script automates the build process for iOS and Android

set -e

echo "🏗️  Arterior AR App Build Script"
echo "================================="

# Check if Unity is installed
UNITY_PATH="/Applications/Unity/Hub/Editor/2022.3.0f1/Unity.app/Contents/MacOS/Unity"
if [ ! -f "$UNITY_PATH" ]; then
    echo "❌ Unity not found at $UNITY_PATH"
    echo "Please install Unity 2022 LTS or update the UNITY_PATH variable"
    exit 1
fi

# Project paths
PROJECT_PATH="$(pwd)"
BUILD_PATH="$PROJECT_PATH/Builds"
IOS_BUILD_PATH="$BUILD_PATH/iOS"
ANDROID_BUILD_PATH="$BUILD_PATH/Android"

# Create build directories
mkdir -p "$IOS_BUILD_PATH"
mkdir -p "$ANDROID_BUILD_PATH"

echo "📱 Building for iOS..."
"$UNITY_PATH" \
    -batchmode \
    -quit \
    -projectPath "$PROJECT_PATH" \
    -buildTarget iOS \
    -executeMethod BuildScript.BuildiOS \
    -logFile "$BUILD_PATH/ios_build.log"

if [ $? -eq 0 ]; then
    echo "✅ iOS build completed successfully"
    echo "📁 iOS build location: $IOS_BUILD_PATH"
else
    echo "❌ iOS build failed. Check $BUILD_PATH/ios_build.log"
fi

echo ""
echo "🤖 Building for Android..."
"$UNITY_PATH" \
    -batchmode \
    -quit \
    -projectPath "$PROJECT_PATH" \
    -buildTarget Android \
    -executeMethod BuildScript.BuildAndroid \
    -logFile "$BUILD_PATH/android_build.log"

if [ $? -eq 0 ]; then
    echo "✅ Android build completed successfully"
    echo "📁 Android build location: $ANDROID_BUILD_PATH"
else
    echo "❌ Android build failed. Check $BUILD_PATH/android_build.log"
fi

echo ""
echo "🎉 Build process completed!"
echo "📋 Next steps:"
echo "   iOS: Open $IOS_BUILD_PATH in Xcode and deploy to device"
echo "   Android: Install APK from $ANDROID_BUILD_PATH"

# Optional: Open build directories
if command -v open &> /dev/null; then
    read -p "Open build directories? (y/n): " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        open "$BUILD_PATH"
    fi
fi