#  _____ ____   ____      _
# |_   _/ ___| / ___|___ | | ___  _ __ ___
#   | || |    | |   / _ \| |/ _ \| '__/ __|
#   | || |___ | |__| (_) | | (_) | |  \__ \
#   |_| \____(_)____\___/|_|\___/|_|  |___/
#
# Build file

$ErrorActionPreference="Stop"

#         ↓↓↓↓↓
$VERSION="2.0.0"

$BUILD_NUMBER = [System.Environment]::GetEnvironmentVariable('BUILD_NUMBER')
$FULL_VERSION = "$VERSION.$BUILD_NUMBER"

$NUGET_SOURCE="gitea NuGet"
$GITEA_API_KEY = [System.Environment]::GetEnvironmentVariable('GITEA_API_KEY')

Push-Location src
try {
    dotnet clean /p:Version=$FULL_VERSION
    if(!$?) { exit 1 }

    dotnet restore /p:Version=$FULL_VERSION
    if(!$?) { exit 1 }

    dotnet build /p:Configuration=Release /p:Version=$FULL_VERSION
    if(!$?) { exit 1 }

    dotnet pack /p:Configuration=Release /p:Version=$FULL_VERSION
    if(!$?) { exit 1 }

    dotnet nuget push --source $NUGET_SOURCE --api-key=$GITEA_API_KEY "TC.Colors\bin\Release\TC.Colors.$FULL_VERSION.nupkg"
    if(!$?) { exit 1 }
}
finally
{
    Pop-Location
}
