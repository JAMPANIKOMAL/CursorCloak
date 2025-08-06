# Create Application Icon Script
# This PowerShell script creates a professional CursorCloak icon

Add-Type -AssemblyName System.Drawing

# Create icon sizes: 16x16, 32x32, 48x48, 256x256
$sizes = @(16, 32, 48, 256)
$iconPath = ".\CursorCloak.UI\Resources\CursorCloak.ico"

# Create Resources directory if it doesn't exist
$resourcesDir = ".\CursorCloak.UI\Resources"
if (!(Test-Path $resourcesDir)) {
    New-Item -ItemType Directory -Path $resourcesDir -Force
}

Write-Host "Creating CursorCloak application icon..." -ForegroundColor Yellow

# Create a bitmap for drawing
$bitmap = New-Object System.Drawing.Bitmap(256, 256)
$graphics = [System.Drawing.Graphics]::FromImage($bitmap)

# Enable anti-aliasing for smooth graphics
$graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic

# Define colors
$backgroundColor = [System.Drawing.Color]::FromArgb(14, 18, 24)  # Dark background #0e1218
$primaryColor = [System.Drawing.Color]::FromArgb(0, 120, 215)    # Blue #0078D7
$accentColor = [System.Drawing.Color]::FromArgb(230, 237, 243)   # Light #e6edf3
$shadowColor = [System.Drawing.Color]::FromArgb(100, 0, 0, 0)    # Semi-transparent black

# Create brushes and pens
$backgroundBrush = New-Object System.Drawing.SolidBrush($backgroundColor)
$primaryBrush = New-Object System.Drawing.SolidBrush($primaryColor)
$accentBrush = New-Object System.Drawing.SolidBrush($accentColor)
$shadowBrush = New-Object System.Drawing.SolidBrush($shadowColor)
$primaryPen = New-Object System.Drawing.Pen($primaryColor, 3)
$accentPen = New-Object System.Drawing.Pen($accentColor, 2)

try {
    # Fill background
    $graphics.FillRectangle($backgroundBrush, 0, 0, 256, 256)
    
    # Draw main cursor icon shape (stylized cursor with cloak effect)
    $center = 128
    $cursorSize = 80
    
    # Draw shadow effect
    $shadowOffset = 4
    $shadowRect = New-Object System.Drawing.Rectangle($center - $cursorSize/2 + $shadowOffset, $center - $cursorSize/2 + $shadowOffset, $cursorSize, $cursorSize)
    $graphics.FillEllipse($shadowBrush, $shadowRect)
    
    # Draw main cursor circle
    $cursorRect = New-Object System.Drawing.Rectangle($center - $cursorSize/2, $center - $cursorSize/2, $cursorSize, $cursorSize)
    $graphics.FillEllipse($primaryBrush, $cursorRect)
    $graphics.DrawEllipse($accentPen, $cursorRect)
    
    # Draw cursor arrow inside
    $arrowSize = 30
    $arrowPoints = @(
        [System.Drawing.Point]::new($center - $arrowSize/2, $center - $arrowSize/2),
        [System.Drawing.Point]::new($center + $arrowSize/2, $center),
        [System.Drawing.Point]::new($center, $center + $arrowSize/2),
        [System.Drawing.Point]::new($center - $arrowSize/4, $center + $arrowSize/4)
    )
    $graphics.FillPolygon($accentBrush, $arrowPoints)
    
    # Draw "cloak" effect - diagonal lines suggesting invisibility
    $cloakPen = New-Object System.Drawing.Pen($accentColor, 2)
    $cloakPen.DashStyle = [System.Drawing.Drawing2D.DashStyle]::Dash
    
    for ($i = 0; $i -lt 256; $i += 20) {
        $graphics.DrawLine($cloakPen, $i, 0, $i + 50, 50)
        $graphics.DrawLine($cloakPen, 0, $i, 50, $i + 50)
    }
    
    # Draw outer ring for polish
    $outerRing = New-Object System.Drawing.Rectangle(20, 20, 216, 216)
    $graphics.DrawEllipse($primaryPen, $outerRing)
    
    Write-Host "Icon design created successfully!" -ForegroundColor Green
    
    # Save as PNG first
    $pngPath = ".\CursorCloak.UI\Resources\CursorCloak.png"
    $bitmap.Save($pngPath, [System.Drawing.Imaging.ImageFormat]::Png)
    Write-Host "PNG icon saved: $pngPath" -ForegroundColor Green
    
    # Note: Converting to ICO format requires additional tools
    Write-Host "To convert to ICO format, use an online converter or ImageMagick:" -ForegroundColor Yellow
    Write-Host "  1. Upload $pngPath to https://convertio.co/png-ico/" -ForegroundColor Gray
    Write-Host "  2. Download as CursorCloak.ico" -ForegroundColor Gray
    Write-Host "  3. Place in $resourcesDir" -ForegroundColor Gray
    
} finally {
    # Clean up resources
    $graphics.Dispose()
    $bitmap.Dispose()
    $backgroundBrush.Dispose()
    $primaryBrush.Dispose()
    $accentBrush.Dispose()
    $shadowBrush.Dispose()
    $primaryPen.Dispose()
    $accentPen.Dispose()
    $cloakPen.Dispose()
}

Write-Host "Icon creation completed!" -ForegroundColor Green
