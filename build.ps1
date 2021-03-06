param([string] $build_version )

function Run-Task
{
    param(
        [scriptblock] $block
    )

    & $block

    if($LASTEXITCODE -ne 0)
    {
        exit $LASTEXITCODE
    }
}

Run-Task { dotnet build src/nlog.indentexception.sln -c Release }

Get-ChildItem ./src/*.pp -Recurse | ForEach-Object { Remove-Item $_ }

$files = (Get-ChildItem -Path ./src/ -Filter *.cs -File)

$files | ForEach-Object { 
    $in = (Get-Content $_.FullName).Replace('YourNamespace.', '$rootnamespace$.');
    Set-Content ($_.FullName + ".pp") -Value '// <auto-generated/>', $in;
}

New-Item -ItemType Directory -Force -Path bin  | Out-Null

'packing ' + $build_version
& nuget.exe pack src/nlog.indentexception.nuspec -OutputDirectory bin -ExcludeEmptyDirectories -version $build_version -properties "Configuration=Release"

