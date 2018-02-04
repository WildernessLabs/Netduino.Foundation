#################################################################

param(
    [Parameter(Mandatory=$True,Position=1)]
    [string]$NFVersion
)

#set path for source dir
$path = "Peripheral_Libs"
$subPath = "Driver"

#################################################################

$pattern = '\[assembly: AssemblyVersion\("(.*)"\)\]'
$dirs = Get-ChildItem $path -Directory

ForEach($dir in $dirs) {

    $fullpath = "$path/$dir/$subPath"
    
    if(Test-Path -Path "$fullpath/*" -Include *.nuspec){
        
        # prep the nuspec update
        $file = Get-ChildItem "$fullpath/*.nuspec" -file | Select-Object DirectoryName, name
        $xml = [xml](Get-Content ("{0}\{1}" -f $file.DirectoryName, $file.Name))
        $xml.SelectSingleNode("//package/metadata/dependencies/dependency[@id='Netduino.Foundation']").Attributes['version'].Value = $NFVersion
        
        # prep the assemblyversion update
        if(Test-Path "$fullpath/Properties/AssemblyInfo.cs"){
            
            $match = ""
            $replacement = ""
            $contents = Get-Content "$fullpath/Properties/AssemblyInfo.cs"
            foreach($content in $contents){

                if($content -match $pattern){
                    try{
                        $fileVersion = [version]$matches[1]
                        if($fileVersion.Major -gt 0 -or $fileVersion.Minor -gt 0){
                            $match = $fileVersion.ToString()
                            $replacement = "{0}.{1}.{2}.{3}" -f $fileVersion.Major, ($fileVersion.Minor+1), $fileVersion.Build, $fileVersion.Revision
                        }
                    }
                    catch [System.Exception]{
                        Write-Host("ERROR - Could not parse file version from assembly for $dir. skipping NuGet publish.")
                        continue
                    }
                }
                
            }

            # if the assembly verion is is greater than "0.0" save the nuspec and assemblyinfo.cs files
            if($math -ne "" -and $replacement -ne ""){
                Write-Host($fullpath)
                $contents = $contents -creplace $match, $replacement
                $contents | Set-Content "$fullpath/Properties/AssemblyInfo.cs" -encoding UTF8
                $xml.save(("{0}\{1}" -f $file.DirectoryName, $file.Name))
            }
        }
    }
}