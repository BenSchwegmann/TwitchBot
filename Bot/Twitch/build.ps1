cd ..\..\CommentDetector
.\CommentDetector.exe
cd ..\Bot\Twitch
dotnet publish -o ..\..\Build -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true ..\Bot.csproj