

function ZipFolder(
	[Parameter(Mandatory=$true)][string] $srcPath, 
	[Parameter(Mandatory=$true)][string] $zipFileName
	)
{
	Write-Host "-- Zipping $srcPath to $zipFileName"
	Copy-ToZip –file $srcPath –zipfile $zipFileName –hideProgress
}

function UnzipFileToFolder(
	[Parameter(Mandatory=$true)][string] $filename, 
	[Parameter(Mandatory=$true)][string] $dest
	)
{
	#http://serverfault.com/questions/18872/how-to-zip-unzip-files-in-powershell
	Write-Host "Unzipping $filename to $dest"
	
	$ShellApplication = new-object -com shell.application 
	
	$zip_file = $ShellApplication.namespace($filename) 
	$destination = $ShellApplication.namespace($dest) 
	
	$destination.Copyhere($zip_file.items())
}