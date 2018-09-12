Write-Host $env:Build_SourcesDirectory
Write-Host Add Microsoft.VisualStudio.Coverage.Analysis.dll
[System.Reflection.Assembly]::LoadFrom("C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\PrivateAssemblies\Microsoft.VisualStudio.Coverage.Analysis.dll")

New-Item $env:Build_SourcesDirectory\TestResults\Coverage -ItemType Directory
$coverage = Get-ChildItem -Include "*.coverage" -Recurse | Select -Exp FullName -First 1
Write-Host $coverage

$info = [Microsoft.VisualStudio.Coverage.Analysis.CoverageInfo]::CreateFromFile($coverage)
$data = $info.BuildDataSet()

Write-Host coverage results "$env:Build_SourcesDirectory\TestResults\Coverage\coverage.xml"
$data.WriteXml("$env:Build_SourcesDirectory\TestResults\Coverage\coverage.xml")

dotnet tool install dotnet-reportgenerator-globaltool --tool-path tools --version 4.0.0-rc4
Write-Host tools\reportgenerator.exe -reports:"$env:Build_SourcesDirectory\TestResults\Coverage\coverage.xml" -targetdir:"$env:Build_SourcesDirectory\TestResults\Coverage\Reports" -tag:$env:Build_BuildNumber -reportTypes:htmlInline
tools\reportgenerator.exe -reports:"$env:Build_SourcesDirectory\TestResults\Coverage\coverage.xml" -targetdir:"$env:Build_SourcesDirectory\TestResults\Coverage\Reports" -tag:$env:Build_BuildNumber -reportTypes:htmlInline

Write-Host dir $env:Build_SourcesDirectory\TestResults
dir $env:Build_SourcesDirectory\TestResults

Write-Host dir $env:Build_SourcesDirectory\TestResults\Coverage\Reports
dir $env:Build_SourcesDirectory\TestResults\Coverage\Reports
