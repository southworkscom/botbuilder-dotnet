# # libraries that are in the botbuilder-dotnet repo
# $Contracts = "Microsoft.Bot.Builder.AI.Luis,Microsoft.Bot.Builder.AI.QnA,Microsoft.Bot.Builder.ApplicationInsights,Microsoft.Bot.Builder.Azure,Microsoft.Bot.Builder.Dialogs,Microsoft.Bot.Builder.Integration.ApplicationInsights.Core,Microsoft.Bot.Builder.Integration.AspNet.Core,Microsoft.Bot.Builder.TemplateManager,Microsoft.Bot.Builder.Testing,Microsoft.Bot.Builder,Microsoft.Bot.Configuration,Microsoft.Bot.Connector,Microsoft.Bot.Schema,Microsoft.Bot.Streaming"

# # all the libraries that detects for this version
# # $Contracts = "Microsoft.Bot.Builder.Adapters.Facebook,Microsoft.Bot.Builder.Adapters.Slack,Microsoft.Bot.Builder.Adapters.Twilio,Microsoft.Bot.Builder.Adapters.Webex,Microsoft.Bot.Builder.AI.Luis,Microsoft.Bot.Builder.AI.QnA,Microsoft.Bot.Builder.Integration.ApplicationInsights.Core,Microsoft.Bot.Builder.Integration.ApplicationInsights.WebApi,Microsoft.Bot.Builder.Integration.AspNet.Core,Microsoft.Bot.Builder.Integration.AspNet.WebApi,AdaptiveExpressions,Microsoft.Bot.Builder,Microsoft.Bot.Builder.ApplicationInsights,Microsoft.Bot.Builder.Azure,Microsoft.Bot.Builder.Azure.Blobs,Microsoft.Bot.Builder.Dialogs,Microsoft.Bot.Builder.Dialogs.Adaptive,Microsoft.Bot.Builder.Dialogs.Adaptive.Teams,Microsoft.Bot.Builder.Dialogs.Declarative,Microsoft.Bot.Builder.TemplateManager,Microsoft.Bot.Builder.Testing,Microsoft.Bot.Configuration,Microsoft.Bot.Connector,Microsoft.Bot.Schema,Microsoft.Bot.Streaming"

# # version that is in the botbuilder-dotnet repo

# # didnt found the proper version in the registry source
# # ,Microsoft.Bot.Builder.AI.Orchestrator,Microsoft.Bot.Builder.Runtime,Microsoft.Bot.Builder.Parsers.LU,Microsoft.Bot.Builder.Azure.Queues,Microsoft.Bot.Builder.Dialogs.Debugging,Microsoft.Bot.Builder.Dialogs.Adaptive.Testing,Microsoft.Bot.Builder.Integration.Runtime,Microsoft.Bot.Builder.LanguageGeneration.Plugins


# # Script to create a package.config, download the Nuget dependencies and copy the .dlls into one folder
# $Xml = "<?xml version=""1.0"" encoding=""utf-8""?>`n<packages>`n"
# $Contracts.Split(",") | ForEach-Object {
#     $Library = $_.Trim()
#     $Xml += "  <package id=""" + $Library + """ version=""" + $Version + """/>`n"
# }
# $Xml += "</packages>"
# New-Item -Path ./LastMajorVersionBinary -Name "packages.config" -ItemType "file" -Value $Xml -Force

# nuget install ./LastMajorVersionBinary/packages.config -OutputDirectory LastMajorVersionBinary/NugetPackages

# Copy-Item -Path "LastMajorVersionBinary/NugetPackages\**\lib\**\*.dll" -Destination  LastMajorVersionBinary/ -Recurse -Force

# Remove-item -Path LastMajorVersionBinary/NugetPackages -Recurse -Force

# This doesnt get recognized by PowerShell -file 'file.ps1' command
# Param ([string]$dll)

$SolutionDir = $args[0]
$Dll = $args[1]
$Version = "4.6.3"

$LastVersionFolder = "$($SolutionDir)LastMajorVersionBinary"
$TempDLLFolder = "$LastVersionFolder\Temp\$Dll"

$Xml =  "<?xml version=""1.0"" encoding=""utf-8""?>`n<packages>`n"
$Xml += "  <package id=""" + $Dll + """ version=""" + $Version + """/>`n"
$Xml += "</packages>"

New-Item -Path "$TempDLLFolder" -Name "packages.config" -ItemType "file" -Value $Xml -Force

nuget install "$TempDLLFolder\packages.config" -OutputDirectory $TempDLLFolder

# nuget install $Dll -Version $Version -OutputDirectory $TempDLLFolder

Copy-Item -Path "$TempDLLFolder\**\lib\**\*.dll" -Destination  $LastVersionFolder -Recurse -Force

Remove-item -Path $TempDLLFolder -Recurse -Force
