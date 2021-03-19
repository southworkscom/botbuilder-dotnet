using namespace System.IO.Compression

param
( 
    [string]$Path,
    [Parameter(Mandatory=$True)]
    [string]$Name,
    [string]$Version
)

if (![string]::IsNullOrEmpty($Path)) {
    Set-Location -Path $Path
    $Path = $Path.TrimEnd('\')
} else {
    $Path = Get-Location
}

$ApiCompatPath = "$Path\ApiCompat"
$InstallResult = $false
$ApiCompatDownloadRequestUri = 'https://pkgs.dev.azure.com/dnceng/public/_apis/packaging/feeds/dotnet-eng/nuget/packages/Microsoft.DotNet.ApiCompat/versions/6.0.0-beta.21168.3/content?api-version=6.0-preview.1'

$DownloadLatestVersion = {
    Write-Host ">> Attempting to install latest version`n" -ForegroundColor cyan
    
    # Get latest version suffix
    $DllData = nuget search $DllName -PreRelease
    $LatestVersion = [regex]::match($DllData,"(?<=$DllName \| ).*?(?=\s)").Value
    
    # Store command into a variable to handle error output from nuget
    $NugetInstallCommand = 'nuget install $DllName -OutputDirectory "$Path\ApiCompat\Contracts" -Version $LatestVersion'
    
    # Run command and store outputs into variables
    Invoke-Expression $NugetInstallCommand -ErrorVariable InstallCommandError -OutVariable InstallCommandOutput 2>&1 >$null
    
    $script:InstallResult = ($InstallCommandOutput -match 'Added package' -or $InstallCommandOutput -match 'already installed')
    if ($InstallResult) {
        Write-Host ">> Success`n" -ForegroundColor green        
        
        # Update version with latest suffix
        $script:Version = $LatestVersion
    }
}

$DownloadFixedVersions = {
    # Remove version sufix if any
    $script:LocalVersion = $Version
    $script:Version = $Version -replace '-local'
    
    Write-Host ">> Attempting to download GA specific version = $Version`n" -ForegroundColor cyan
    
    # Install corresponding nuget package to "Contracts" folder    
    # Store command into a variable to handle error output from nuget
    $NugetInstallCommand = 'nuget install $DllName -Version $Version -OutputDirectory "$Path\ApiCompat\Contracts" -Verbosity detailed'
    
    # Run command and store outputs into variables
    Invoke-Expression $NugetInstallCommand -ErrorVariable InstallCommandError -OutVariable InstallCommandOutput 2>&1 >$null
    
    # Check package existance by searching on the output strings that would match only if the package is installed
    $script:InstallResult = ($InstallCommandOutput -match 'Added package' -or $InstallCommandOutput -match 'already installed')
    
    # If GA version doesn't exist, attempt to download specific preview version
    if(!$InstallResult) {
        Write-Host ">> Failed`n" -ForegroundColor red
        Write-Host ">> Attempting to install specific preview version = $Version-preview`n" -ForegroundColor cyan
        
        # Store command into a variable to handle error output from nuget
        $NugetInstallCommand = 'nuget install $DllName -Version "$Version-preview" -OutputDirectory "$Path\ApiCompat\Contracts" -Verbosity detailed'
        
        # Run command and store outputs into variables
        Invoke-Expression $NugetInstallCommand -ErrorVariable InstallCommandError -OutVariable InstallCommandOutput 2>&1 >$null
        $script:InstallResult = ($InstallCommandOutput -match 'Added package' -or $InstallCommandOutput -match 'already installed')
        
        # If specific preview version doesn't exist, attempt to download latest version (including preview)
        if ($InstallResult) {
            Write-Host ">> Success`n" -ForegroundColor green
            
            # If previous install is successful, we append -preview to version.
            $script:Version = "$Version-preview"
        } else {
            # If specific versions failed, download latest
            Write-Host ">> Failed`n" -ForegroundColor red
            &$DownloadLatestVersion
        }
    } else {
        Write-Host ">> Success`n" -ForegroundColor green
    }
}

$CheckApiCompatAvailability = {
    $Result = $false
    
    # It exists, but might be being downloaded/extracted by another process. Wait for that process to finish
    1..10 | ForEach-Object {
        if ((Test-Path "$ApiCompatPath\tools") -and !(Test-Path "$ApiCompatPath\$ZipFile" -PathType Leaf)) {
            # Zipfile has been extracted
            $Result = $true
            # If I place this break here, for some reason the whole script stops running :(
            # break
        } else {
            Write-Warning "Attempting to get lock of $ZipFile"
            Start-Sleep -Seconds 6
        }
    }

    if (!$Result) {
        Write-Error "Timed out attempting to reach unzipped files from $ZipFile"
        exit 2
    }

    Write-Host "Successfully reached unzipped files from $ZipFile"
}

$DownloadApiCompat = {
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    cd $ApiCompatPath
    
    # Get Zipfile for ApiCompat
    $ZipFile = "ApiCompat.zip"
    $ZipPath = "$ApiCompatPath\$ZipFile"
    $DestinationPath= "$Path\ApiCompat"
    $TargetEntry = "netcoreapp3.1"
    
    # If file doesn't exist, download it
    if (!(Test-Path "$ApiCompatPath\ApiCompat.zip" -PathType Leaf)) {
        try { 
            # Download
            Write-Host "Downloading zip"
            
            try {
                Invoke-RestMethod -Method "GET" -Uri $ApiCompatDownloadRequestUri -OutFile ".\$ZipFile"
            } catch { 
                Write-Error $_
                exit 1 
            }
            
            $Zip = [ZipFile]::OpenRead($ZipPath)
            Start-Sleep -Seconds 30
            
            # Extract files
            $Zip.Entries.Where{ $_.FullName -match "$TargetEntry.*[^/]$" }.ForEach{
                $NewFile = [IO.FileInfo]($DestinationPath,$_.FullName -join "/")
                $NewFile.Directory.Create()
                [ZipFileExtensions]::ExtractToFile($_, $NewFile)
            }
            $Zip.Dispose()
            
            # Remove downloaded zip file.
            Remove-Item $ZipFile
        } catch {
            Write-Warning "CHECK AVAILABILITY FROM CATCH"
            &$CheckApiCompatAvailability
        }
    } else {
        Write-Warning "CHECK AVAILABILITY FROM ELSE"
        &$CheckApiCompatAvailability
    }
    cd $Path
}

$WriteToLog = {
    $ResultMessage = "$DllName LOCAL: $LocalVersion | UPSTREAM: $Version => $ApiCompatResult"
    Write-Host $ResultMessage -ForegroundColor green
    
    # Create a Mutex for all process to be able to share same log file
    $mutexName = "LogFileMutex" #'A unique name shared/used across all processes that need to write to the log file.'
    $mutex = New-Object 'Threading.Mutex' $false, $mutexName
    
    #Grab the mutex. Will block until this process has it.
    $mutex.WaitOne();
    
    try
    {
        # Now it is safe to write to log file
        Add-Content $OutputDirectory $ResultMessage
    } finally {
        $mutex.ReleaseMutex()
    }
}

# Get specific dll file from built solution
$Dll = Get-ChildItem "$Path\**\$Name\bin\Debug\**\**" -Filter "$Name.dll" | % { $_.FullName }
if ([string]::IsNullOrEmpty($Dll)){
    $Dll = Get-ChildItem "$Path\**\**\$Name\bin\Debug\**\**" -Filter "$Name.dll" | % { $_.FullName }
}

# Prepare copy statement to move dll to "Implementations" folder for comparing later
$DllDestination = if (Test-Path "$ApiCompatPath\Implementations" -PathType Container) { "$ApiCompatPath\Implementations" } else { New-item -Name "Implementations" -Type "directory" -Path $ApiCompatPath }
$CopyLocalDllToDestination = 'Copy-Item $Dll -Destination $DllDestination'
$DllName = [IO.Path]::GetFileNameWithoutExtension($Dll)

if ([string]::IsNullOrEmpty($Version)) {
    &$DownloadLatestVersion
} else {
    &$DownloadFixedVersions
}

# No reason to continue if package could not be installed
if (!$InstallResult) {
    Write-Error "Failed to download package $DllName with version $Version`n"
    exit 0
}

# Package to compare to has been downloaded, proceed to copy local version for comparisson
try { 
    Invoke-Expression $CopyLocalDllToDestination 
} catch { 
    Write-Error ">> Local dll was not found. Try building your project or solution."
    exit 3
}

Write-Host ">> Dll '$Name' successfully copied to $ApiCompatPath\Implementations`n" -ForegroundColor cyan

# Get specific dll file from nuget package
$PackageName = "$DllName.$Version"
$Package = Get-ChildItem "$ApiCompatPath\Contracts\$PackageName\lib\**\*.dll" -Filter "*.dll" -Recurse
$PackageDestination = if (Test-Path "$ApiCompatPath\Contracts\NugetDlls" -PathType Container) { "$ApiCompatPath\Contracts\NugetDlls" } else { New-item -Name "NugetDlls" -Type "directory" -Path $ApiCompatPath\Contracts }
Copy-Item $Package -Destination $PackageDestination

# Download ApiCompat
if (!(Test-Path "$ApiCompatPath\tools")) {
    &$DownloadApiCompat
}

# Run ApiCompat
$ApiCompatResult = (.\ApiCompat\tools\netcoreapp3.1\Microsoft.DotNet.ApiCompat.exe "$ApiCompatPath\Contracts\NugetDlls\$DllName.dll" --impl-dirs "$ApiCompatPath\Implementations\$DllName.dll") -replace 'TypesMustExist', "`nTypesMustExist"
$OutputDirectory = if (Test-Path "$ApiCompatPath\ApiCompatResult.txt") { "$ApiCompatPath\ApiCompatResult.txt" } else { New-item -Name "ApiCompatResult.txt" -Type "file" -Path $ApiCompatPath }
Write-Host ">> Saving ApiCompat output to $OutputDirectory`n" -ForegroundColor cyan

# Add result to txt file for better accessibility
&$WriteToLog