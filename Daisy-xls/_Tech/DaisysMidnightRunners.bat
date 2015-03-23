@echo off

REM Script to run Blu82 on all Daisy files to create inputs

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
 ..\..\Daisy\Daisy.exe _MainScript.xml 2>&1 > %folder%.log
 REM dir 2>&1 > %folder%.log
 cd ..
 GOTO :eof
 
