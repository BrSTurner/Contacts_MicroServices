@echo off

echo.
echo ========================================================
echo  Construindo imagens Docker...
echo ========================================================
echo.

docker-compose build

.\SetupNoBuild.bat