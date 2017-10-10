#################################################################

#replace this with build env var
$buildNumber = 123 

#set path for peripherals dir
$path = "Source\Additional_Peripheral_Libraries"

#################################################################

$pattern = '\[assembly: AssemblyVersion\("(.*)"\)\]'
$dirs = Get-ChildItem $path -Directory

ForEach($dir in $dirs) {

    $fullpath = "$path/$dir"

    if(Test-Path -Path "$fullpath/*" -Include *.nuspec){
        if(Test-Path "$fullpath/Properties/AssemblyInfo.cs"){
            
            $contents = Get-Content "$fullpath/Properties/AssemblyInfo.cs"
            foreach($content in $contents){

                if($content -match $pattern){
                    try{
                        $fileVersion = [version]$matches[1]
                    }
                    catch [System.Exception]{
                        Write-Host("ERROR - Could not parse file version from assembly for $dir. skipping NuGet publish.")
                        continue
                    }

                    if(!(Test-Path "SourceVersions")){
                        mkdir SourceVersions
                    }

                    if(!(Test-Path "SourceVersions/$dir.txt")){
                        "0.0.0" > "SourceVersions/$dir.txt"
                    }

                    $currentVersion = Get-Content("SourceVersions/$dir.txt");
                    $currentVersion = [version]$currentVersion
                    
                    $newVersion = "{0}.{1}.{2}" -f $fileVersion.Major, $fileVersion.Minor, ($buildNumber)
                    $newVersion > "SourceVersions/$dir.txt"

                    #if($fileVersion.Major -gt $currentVersion.Major -or $fileVersion.Minor -gt $currentVersion.Minor){
                        Write-Host("Major/Minor version bumped detected - publishing NuGet for $dir")
                        $msbuild = '"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"'
                        iex ("& {0} {1}" -f $msbuild, "$fullpath/$dir.csproj")
                        c:\utils\nuget pack $fullpath/$dir.csproj -Version $newVersion -BasePath $fullpath -OutputDirectory SourcePackages 
                    #}
                }
            }
        }
    }
}