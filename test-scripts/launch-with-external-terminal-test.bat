:: Windows Terminal
:: https://apps.microsoft.com/detail/9N0DX20HK701?hl=en-us&gl=CA&ocid=pdpshare

echo off
set build_path=".\vs-launch-external-terminal\bin\Debug\net8.0\vs-launch-external-terminal.exe"
set terminal=""
echo Building program...

cd ".\vs-launch-external-terminal\"
:: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-build
dotnet build "vs-launch-external-terminal.sln"
cd ..
echo Build complete
echo Run program
echo " "

Powershell.exe -executionpolicy remotesigned -File  C:\Users\SE\Desktop\ps.ps1
%build_path%

echo on
pause