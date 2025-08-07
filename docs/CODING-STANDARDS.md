# Coding Standards

This document outlines the coding standards and best practices to be followed in the CursorCloak project.

## Inno Setup Scripts (.iss)

### 1. No Emojis or Special Characters

To ensure cross-platform compatibility and prevent rendering issues in the installer, do not use emojis or non-standard special characters in `.iss` files. These characters can cause unexpected display problems, as seen in the installer UI.

**Example of what to avoid:**

```iss
FinishedLabel=CursorCloak v1.0.2 has been successfully installed!%n%n🎉 READY TO USE:%n• Launch CursorCloak as administrator
```

**Correct implementation:**

```iss
FinishedLabel=CursorCloak v1.0.2 has been successfully installed!%n%nREADY TO USE:%n- Launch CursorCloak as administrator
```
