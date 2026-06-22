@echo off
chcp 65001 >nul
echo ========================================
echo   Blog.Server - Windows AOT 发布
echo ========================================
echo.

:: 清理旧产物
if exist "publish" rmdir /s /q publish

:: 发布 AOT
dotnet publish Blog.Api/Blog.Api.csproj ^
  -c Release -r win-x64 ^
  --self-contained

:: 删除调试符号
del /f /q publish\*.pdb 2>nul

echo.
echo ========================================
echo   ✅ 发布完成！
echo   产物: publish\Blog.Api.exe
echo ========================================
pause
