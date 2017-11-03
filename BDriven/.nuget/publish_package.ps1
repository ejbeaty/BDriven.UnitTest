
$projectFile = Get-ChildItem -Path .. -filter "*.csproj" | sort LastWriteTime | select -last 1
Write-Host "Packing nuget package from $($projectFile)"
iex ".\nuget.exe pack ..\$($projectFile)"

$pkgfile = Get-ChildItem  -filter "*.nupkg" | sort LastWriteTime | select -last 1
Write-Host "Pushing file $($pkgFile)"

iex ".\nuget.exe push $($pkgfile) -Source https://www.nuget.org/api/v2/package"




