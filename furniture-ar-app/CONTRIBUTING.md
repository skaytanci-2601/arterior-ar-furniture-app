# Contributing to Arterior AR Furniture App

Thank you for your interest in contributing to Arterior! This document provides guidelines and instructions for contributing to the project.

## 🚀 Getting Started

### Prerequisites
- Unity 2022 LTS (for Unity development)
- Git installed on your system
- A GitHub account
- Basic knowledge of C# (for Unity scripts) or HTML/CSS/JavaScript (for Web AR)

### Setting Up Development Environment

1. **Fork the repository**
   - Click the "Fork" button on the GitHub repository page
   - Clone your fork: `git clone https://github.com/YOURUSERNAME/arterior-ar-furniture-app.git`

2. **Set up upstream remote**
   ```bash
   git remote add upstream https://github.com/ORIGINALOWNER/arterior-ar-furniture-app.git
   ```

3. **Create a development branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

## 🛠️ Development Setup

### Unity Development
1. **Install Unity 2022 LTS**
2. **Install required packages**:
   - AR Foundation
   - ARKit XR Plugin (iOS)
   - ARCore XR Plugin (Android)
3. **Open the project** in Unity
4. **Test on device** before submitting changes

### Web AR Development
1. **Open** `web-ar-arterior-enhanced.html` in a browser
2. **Test** on mobile devices
3. **Ensure** AR functionality works properly

## 📝 How to Contribute

### Types of Contributions
- 🐛 **Bug fixes**: Fix issues reported in GitHub Issues
- ✨ **New features**: Add new furniture types, UI improvements, etc.
- 📚 **Documentation**: Improve README, add tutorials, fix typos
- 🎨 **UI/UX**: Improve user interface and experience
- 🔧 **Performance**: Optimize code and improve performance

### Contribution Process

1. **Check existing issues**
   - Look for issues labeled "good first issue" or "help wanted"
   - Comment on issues you want to work on

2. **Create a new issue** (if needed)
   - Describe the bug or feature request
   - Use appropriate labels
   - Provide steps to reproduce (for bugs)

3. **Make your changes**
   - Follow the coding standards
   - Test thoroughly
   - Update documentation if needed

4. **Submit a Pull Request**
   - Create a descriptive PR title
   - Explain what changes you made
   - Link to related issues
   - Request review from maintainers

## 📋 Coding Standards

### C# (Unity Scripts)
- Use meaningful variable and function names
- Add XML documentation for public methods
- Follow Unity naming conventions
- Use proper indentation (4 spaces)

### HTML/CSS/JavaScript (Web AR)
- Use semantic HTML
- Follow responsive design principles
- Use modern JavaScript (ES6+)
- Comment complex logic

### Git Commit Messages
- Use clear, descriptive messages
- Start with a verb (Add, Fix, Update, Remove)
- Keep under 50 characters for the title
- Add detailed description if needed

Example:
```
Add: New furniture category for outdoor items

- Added outdoor furniture to catalog.json
- Created new UI elements for outdoor category
- Updated documentation with new features
```

## 🧪 Testing Guidelines

### Before Submitting
- [ ] Test on multiple devices (iOS/Android)
- [ ] Verify AR functionality works
- [ ] Check UI responsiveness
- [ ] Test save/load functionality
- [ ] Ensure no console errors

### Testing Checklist
- [ ] Plane detection works in various lighting
- [ ] Object placement is accurate
- [ ] Gesture controls respond properly
- [ ] UI elements are touch-friendly
- [ ] Performance is acceptable

## 📁 Project Structure

```
arterior-ar-furniture-app/
├── Assets/Arterior/          # Unity project files
│   ├── Scripts/             # C# scripts
│   ├── Scenes/              # Unity scenes
│   ├── Data/                # JSON data files
│   └── Prefabs/             # Unity prefabs
├── web-ar-arterior.html     # Web AR version
├── README.md               # Main documentation
└── CONTRIBUTING.md         # This file
```

## 🐛 Reporting Issues

### Bug Reports
When reporting bugs, please include:
- **Device**: iPhone/Android model and OS version
- **Browser**: If using Web AR version
- **Steps to reproduce**: Detailed steps
- **Expected behavior**: What should happen
- **Actual behavior**: What actually happens
- **Screenshots**: If applicable

### Feature Requests
For new features, please include:
- **Description**: What you want to add
- **Use case**: Why it would be useful
- **Mockups**: Visual examples if possible
- **Implementation ideas**: How you think it could work

## 🏷️ Issue Labels

- `bug`: Something isn't working
- `enhancement`: New feature or request
- `documentation`: Improvements to documentation
- `good first issue`: Good for newcomers
- `help wanted`: Extra attention needed
- `question`: Further information is requested

## 🔄 Pull Request Process

1. **Update your fork** with latest changes
   ```bash
   git fetch upstream
   git checkout main
   git merge upstream/main
   ```

2. **Create your feature branch**
   ```bash
   git checkout -b feature/amazing-feature
   ```

3. **Make your changes** and commit
   ```bash
   git add .
   git commit -m "Add: Amazing feature"
   ```

4. **Push to your fork**
   ```bash
   git push origin feature/amazing-feature
   ```

5. **Create Pull Request** on GitHub

## 📞 Getting Help

- **GitHub Issues**: For bugs and feature requests
- **GitHub Discussions**: For questions and general discussion
- **Email**: [Your email] for direct contact

## 📄 License

By contributing to Arterior, you agree that your contributions will be licensed under the MIT License.

## 🙏 Recognition

Contributors will be:
- Listed in the README.md
- Mentioned in release notes
- Given credit in project documentation

Thank you for contributing to Arterior! 🎉
