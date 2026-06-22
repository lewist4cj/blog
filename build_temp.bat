@echo off
cd /d F:\workspace\csharp\blog-server\Blog.Api
dotnet build --no-restore
echo Exit code: %ERRORLEVEL%
