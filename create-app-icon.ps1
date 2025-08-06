# Create App Icon Script
Add-Type -AssemblyName System.Drawing

# Create a 32x32 icon with a mouse cursor design
$bitmap = New-Object System.Drawing.Bitmap(32, 32)
$graphics = [System.Drawing.Graphics]::FromImage($bitmap)

# Set high quality rendering
$graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$graphics.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality

# Clear background to transparent
$graphics.Clear([System.Drawing.Color]::Transparent)

# Create mouse cursor shape
$cursorColor = [System.Drawing.Color]::White
$outlineColor = [System.Drawing.Color]::Black
$brush = New-Object System.Drawing.SolidBrush($cursorColor)
$pen = New-Object System.Drawing.Pen($outlineColor, 1)

# Define cursor points (arrow shape)
$points = [System.Drawing.Point[]]@(
    [System.Drawing.Point]::new(4, 4),   # Top
    [System.Drawing.Point]::new(4, 24),  # Bottom left
    [System.Drawing.Point]::new(10, 18), # Middle left
    [System.Drawing.Point]::new(16, 28), # Bottom right
    [System.Drawing.Point]::new(20, 26), # Bottom right tip
    [System.Drawing.Point]::new(14, 16), # Middle right
    [System.Drawing.Point]::new(24, 14)  # Right
)

# Draw filled cursor
$graphics.FillPolygon($brush, $points)
# Draw outline
$graphics.DrawPolygon($pen, $points)

# Add a small "hide" symbol (X)
$xPen = New-Object System.Drawing.Pen([System.Drawing.Color]::Red, 2)
$graphics.DrawLine($xPen, 20, 4, 28, 12)
$graphics.DrawLine($xPen, 28, 4, 20, 12)

# Clean up
$brush.Dispose()
$pen.Dispose()
$xPen.Dispose()

# Save as PNG first
$bitmap.Save("CursorCloak.UI\Resources\app-icon.png", [System.Drawing.Imaging.ImageFormat]::Png)

# Convert to ICO
$icon = [System.Drawing.Icon]::FromHandle($bitmap.GetHicon())
$iconStream = New-Object System.IO.FileStream("CursorCloak.UI\Resources\app-icon.ico", [System.IO.FileMode]::Create)
$icon.Save($iconStream)

$iconStream.Close()
$graphics.Dispose()
$bitmap.Dispose()
$icon.Dispose()

Write-Host "App icon created successfully!" -ForegroundColor Green
