@echo off

REM Script to Clean Log Files

set origDir="%cd%"
set outDir=%origDir%\Output
cd %outDir%
FOR /D %%G IN (*) DO (call :handleFolder %%G)
cd %origDir%
GOTO :eof

:handleFolder
 set folder=%1
 cd %folder%
 echo.Handling Folder %folder%
 del Monikers.xml
 del LOG-*.xml
 del *.log
 cd ..
 GOTO :eof
 
