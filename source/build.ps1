param(
	[String] $configuration = "Release"
)

## Include Tools Scripts
. .\Tools\Copy-ToZip.ps1
. .\Tools\New-Zip.ps1
. .\Tools\Tools.ps1

# Build project
C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe JSParser.sln /t:Build /p:Configuration=$configuration

# Unzip and zip again to shrink its size
$myPath = Split-Path -Parent $MyInvocation.MyCommand.Path;
$compiledPackageFileName = $myPath + "\JsParser.Package\bin\$configuration\JsParser.Package.vsix"
$compiledPackageFileNameZip = $compiledPackageFileName + '.zip'
$tempFolder = $myPath + "\temp" + [Guid]::NewGuid().ToString()
$packageResult = $myPath + "\JsParser.Package.4.0.1.$configuration.vsix"
$packageResultZip = $myPath + "\JsParser.Package.vsix.zip"

if (Test-Path $packageResult)
{
	del $packageResult
}

if (Test-Path $packageResultZip)
{
	del $packageResultZip
}

if (Test-Path $compiledPackageFileNameZip)
{
	del $compiledPackageFileNameZip
}

if (Test-Path $tempFolder)
{
	del $tempFolder -recurse
}
$newPath = Mkdir $tempFolder

Copy-Item $compiledPackageFileName $compiledPackageFileNameZip
UnzipFileToFolder $compiledPackageFileNameZip $tempFolder

dir $tempFolder | Copy-ToZip –zipfile $packageResultZip
Rename-Item $packageResultZip $packageResult

del $tempFolder -recurse