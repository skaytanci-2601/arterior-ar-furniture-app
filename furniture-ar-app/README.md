# 🏠 Arterior - AR Furniture Placement App

[![Unity](https://img.shields.io/badge/Unity-2022.3%20LTS-blue.svg)](https://unity.com)
[![AR Foundation](https://img.shields.io/badge/AR%20Foundation-5.0.0-green.svg)](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest/)
[![Web AR](https://img.shields.io/badge/Web%20AR-A--Frame%20%2B%20AR.js-orange.svg)](https://aframe.io)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A cross-platform AR furniture placement application that allows users to visualize furniture in their real-world environment using augmented reality technology.

## ✨ Features

### 🎯 Core AR Functionality
- **Plane Detection**: Horizontal surface detection with visual grid overlay
- **3D Object Placement**: Tap-to-place furniture models on detected surfaces
- **Object Manipulation**: Move, scale, and rotate placed objects with intuitive gestures
- **Real-time Tracking**: Stable AR tracking with visual feedback

### 🛍️ Product Catalog
- **Furniture Categories**: Seating, Tables, Storage, Lighting, Rugs
- **Product Information**: Dimensions, prices, descriptions
- **External Purchase Links**: Direct integration with furniture retailers
- **Visual Thumbnails**: Product preview images

### 💾 Data Management
- **Save/Load Designs**: Persistent storage of room layouts
- **Screenshot Capture**: Export AR views to device gallery
- **Reset Functionality**: Clear all placed objects
- **Cross-session Persistence**: Restore designs after app restart

### 📱 Platform Support
- **iOS**: ARKit integration with iPhone 6s+ support
- **Android**: ARCore integration with Android 7.0+ support
- **Web AR**: Browser-based AR using A-Frame and AR.js
- **Cross-platform**: Single codebase for multiple platforms

## 🚀 Quick Start

### Web AR Version (No Installation Required)
1. **Open** `web-ar-arterior-enhanced.html` in your mobile browser
2. **Print or display** the AR marker: [Download Marker](https://raw.githubusercontent.com/AR-js-org/AR.js/master/data/images/HIRO.jpg)
3. **Point camera** at the marker
4. **Tap furniture buttons** to place objects
5. **Enjoy** your AR furniture experience!

### Unity Version (Full Features)
1. **Install Unity 2022 LTS**
2. **Install AR packages**: AR Foundation, ARKit XR Plugin, ARCore XR Plugin
3. **Open** `Assets/Arterior/Scenes/Main.unity`
4. **Build** for your target platform (iOS/Android)
5. **Deploy** to device and test

## 📁 Project Structure

```
arterior-ar-furniture-app/
├── Assets/Arterior/                 # Unity project files
│   ├── Scenes/                     # Unity scenes
│   ├── Scripts/                    # C# scripts
│   │   ├── ARPlacementController.cs
│   │   ├── ARObjectManipulator.cs
│   │   ├── CatalogController.cs
│   │   ├── SaveLoadService.cs
│   │   └── UI/                     # UI controllers
│   ├── Data/                       # JSON data files
│   ├── Models/                     # 3D model placeholders
│   └── Prefabs/                    # Unity prefabs
├── web-ar-arterior.html            # Basic Web AR version
├── web-ar-arterior-enhanced.html   # Enhanced Web AR version
├── README.md                       # This file
├── QUICKSTART.md                   # Quick start guide
└── .gitignore                      # Git ignore rules
```

## 🛠️ Technology Stack

### Unity Development
- **Engine**: Unity 2022 LTS
- **AR Framework**: AR Foundation 5.0.0
- **Platform Support**: ARKit (iOS), ARCore (Android)
- **Language**: C#
- **UI System**: Unity UI Toolkit

### Web AR Development
- **Framework**: A-Frame 1.4.0
- **AR Library**: AR.js
- **Languages**: HTML5, CSS3, JavaScript
- **Browser Support**: Chrome, Safari, Firefox, Edge

### Data Management
- **Format**: JSON
- **Storage**: Local files, PlayerPrefs
- **Serialization**: Unity JsonUtility

## 📱 Platform Requirements

### iOS
- **Device**: iPhone 6s or newer
- **OS**: iOS 11.0+
- **Features**: ARKit support required
- **Camera**: Required for AR functionality

### Android
- **Device**: ARCore-compatible device
- **OS**: Android 7.0+ (API level 24)
- **Features**: ARCore support required
- **Camera**: Required for AR functionality

### Web AR
- **Browser**: Modern mobile browser
- **Features**: WebRTC camera access
- **Network**: Internet connection for libraries

## 🔧 Installation & Setup

### Unity Setup
1. **Download Unity Hub** from [unity.com](https://unity.com)
2. **Install Unity 2022 LTS** with mobile build support
3. **Clone this repository**:
   ```bash
   git clone https://github.com/yourusername/arterior-ar-furniture-app.git
   cd arterior-ar-furniture-app
   ```
4. **Open Unity Hub** and add the project
5. **Install required packages** via Package Manager:
   - AR Foundation
   - ARKit XR Plugin (iOS)
   - ARCore XR Plugin (Android)

### Web AR Setup
1. **Clone the repository**
2. **Open** `web-ar-arterior-enhanced.html` in a web browser
3. **No additional setup required!**

## 🎮 Usage Guide

### Basic AR Interaction
1. **Launch** the application
2. **Grant camera permissions** when prompted
3. **Point camera** at a flat surface (floor, table)
4. **Wait for plane detection** (grid overlay appears)
5. **Select furniture** from the catalog
6. **Tap on detected plane** to place object
7. **Use gestures** to manipulate placed objects:
   - **Single finger drag**: Move object
   - **Two finger pinch**: Scale object
   - **Two finger twist**: Rotate object

### Advanced Features
- **Save Design**: Store current room layout
- **Load Design**: Restore saved layout
- **Screenshot**: Capture AR view
- **Product Info**: View furniture details
- **Purchase**: Open external buy links

## 🧪 Testing

### AR Functionality Tests
- [ ] Plane detection in various lighting conditions
- [ ] Object placement accuracy on detected surfaces
- [ ] Gesture controls (move, scale, rotate)
- [ ] Multiple object placement and management
- [ ] AR tracking stability during movement

### UI/UX Tests
- [ ] Onboarding flow completion
- [ ] Catalog loading and product display
- [ ] Bottom sheet expand/collapse functionality
- [ ] Settings persistence between sessions
- [ ] Toast notification display

### Performance Tests
- [ ] 60fps AR tracking performance
- [ ] Memory usage within acceptable limits
- [ ] Battery drain optimization
- [ ] App stability during extended use

## 🤝 Contributing

We welcome contributions from the community! Whether you're fixing bugs, adding features, or improving documentation, your contributions help make Arterior better for everyone.

### Quick Start for Contributors
1. **Fork** the repository
2. **Read** our [Contributing Guide](CONTRIBUTING.md)
3. **Check** [open issues](https://github.com/yourusername/arterior-ar-furniture-app/issues) for tasks
4. **Create** a feature branch (`git checkout -b feature/amazing-feature`)
5. **Make** your changes and test thoroughly
6. **Submit** a Pull Request

### Ways to Contribute
- 🐛 **Bug Reports**: Help us identify and fix issues
- ✨ **New Features**: Add furniture types, UI improvements, AR enhancements
- 📚 **Documentation**: Improve guides, tutorials, and code comments
- 🎨 **UI/UX**: Design improvements and user experience enhancements
- 🧪 **Testing**: Test on different devices and report issues
- 🔧 **Performance**: Optimize code and improve app performance

### Development Guidelines
- Follow Unity coding standards for C# scripts
- Use meaningful commit messages
- Test on multiple devices/platforms
- Update documentation for new features
- Follow our [Code of Conduct](CODE_OF_CONDUCT.md)

### Getting Help
- **GitHub Issues**: For bugs and feature requests
- **GitHub Discussions**: For questions and general discussion
- **Pull Requests**: For code review and technical discussion

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **Unity Technologies** for AR Foundation
- **A-Frame** team for Web AR framework
- **AR.js** contributors for browser AR support
- **Apple** for ARKit platform
- **Google** for ARCore platform

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/yourusername/arterior-ar-furniture-app/issues)
- **Discussions**: [GitHub Discussions](https://github.com/yourusername/arterior-ar-furniture-app/discussions)
- **Documentation**: [Wiki](https://github.com/yourusername/arterior-ar-furniture-app/wiki)

## 🔮 Roadmap

### Version 1.1
- [ ] Multi-plane support (walls, ceilings)
- [ ] Advanced lighting and shadows
- [ ] More furniture categories
- [ ] Improved gesture controls

### Version 1.2
- [ ] Collaborative AR sessions
- [ ] Room measurement tools
- [ ] Furniture customization
- [ ] Integration with furniture retailers

### Version 2.0
- [ ] AI-powered room analysis
- [ ] Voice commands
- [ ] Haptic feedback
- [ ] Advanced physics simulation

---

**Made with ❤️ for the AR community**

[![GitHub stars](https://img.shields.io/github/stars/yourusername/arterior-ar-furniture-app?style=social)](https://github.com/yourusername/arterior-ar-furniture-app/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/yourusername/arterior-ar-furniture-app?style=social)](https://github.com/yourusername/arterior-ar-furniture-app/network)