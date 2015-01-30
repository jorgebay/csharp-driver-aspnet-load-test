@echo off
set branch=%1
if "%branch%"=="" set branch="master"
if not exist csharp-driver git clone https://github.com/datastax/csharp-driver.git
cd csharp-driver
git pull origin
git checkout %branch%
cd ..
echo Test with driver branch %branch%
rem msbuild /v:q /nologo /property:Configuration=Release csharp-driver\src\Cassandra.sln
rem msbuild /v:q /nologo /property:Configuration=Release Website\Website.sln