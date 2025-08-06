# Contributing to CursorCloak

Thank you for your interest in contributing to CursorCloak! This document provides guidelines for contributing to the project.

## Development Setup

### Prerequisites
- Windows 10/11
- .NET 9.0 SDK
- Visual Studio 2022 or VS Code
- Git

### Getting Started
1. Fork the repository
2. Clone your fork:
   ```bash
   git clone https://github.com/your-username/CursorCloak.git
   cd CursorCloak
   ```
3. Build the solution:
   ```bash
   dotnet restore
   dotnet build --configuration Release
   ```
4. Test that everything works by running as administrator

## Development Guidelines

### Code Style
- Follow C# naming conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Include error handling for all external operations

### Testing
- Test all changes with administrator privileges
- Verify hotkey functionality works correctly
- Test cursor hiding/showing in various applications
- Ensure no memory leaks in cursor operations

### Security Considerations
- Never commit sensitive information
- All P/Invoke calls must have proper error handling
- Resource management is critical for GDI objects
- Admin privilege checks must be maintained

## Making Changes

### Before You Start
1. Check existing issues to avoid duplicates
2. Create an issue to discuss major changes
3. Keep changes focused and atomic

### Development Process
1. Create a feature branch:
   ```bash
   git checkout -b feature/your-feature-name
   ```
2. Make your changes
3. Test thoroughly (especially with admin privileges)
4. Commit with clear messages:
   ```bash
   git commit -m "Add feature: description of what was added"
   ```
5. Push to your fork and create a pull request

### Pull Request Guidelines
- Provide a clear description of changes
- Reference any related issues
- Include testing steps
- Ensure no build warnings
- Update documentation if needed

### Commit Message Format
- Use present tense ("Add feature" not "Added feature")
- Use imperative mood ("Move cursor to..." not "Moves cursor to...")
- Limit first line to 72 characters
- Reference issues when applicable

## Types of Contributions

### Bug Fixes
- Clearly describe the issue in your PR
- Include steps to reproduce
- Test the fix thoroughly

### New Features
- Discuss the feature in an issue first
- Keep features focused and well-scoped
- Update documentation and help text

### Documentation
- Fix typos and improve clarity
- Add examples where helpful
- Keep documentation up to date with code changes

### Performance Improvements
- Measure performance before and after
- Document the improvement
- Ensure no functionality is broken

## Code Review Process

1. All changes must be reviewed
2. Maintainers will review PRs as time permits
3. Address feedback promptly
4. Squash commits if requested

## Getting Help

- Create an issue for questions
- Use GitHub Discussions for general questions
- Check existing documentation first

## Recognition

Contributors will be recognized in release notes and may be added to a contributors list.

Thank you for contributing! 🎉
