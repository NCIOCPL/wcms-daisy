@echo off

REM Script to run Blu82 on all Daisy files to create inputs

set origDir="%cd%"
set outDir=%origDir%\Output
rmdir /S /Q %outDir%
mkdir %outDir%
cd %outDir%
FOR %%G IN (..\..\Excel-Scripts\*.xls) DO (call :handleFile %%G)
cd %origDir%
GOTO :eof

:handleFile
 set filename=%1
 REM Cleanup Filename
 set filename=%filename:..\..\Excel-Scripts\=%
 set filename=%filename:.xls=%
 mkdir %filename%
 cd %filename%
 echo.Running Blu: ..\..\Excel-Scripts\%filename%.xls
 ..\..\Blu82\Blu82.exe ..\..\..\Excel-Scripts\%filename%.xls
 cd ..
 GOTO :eof
 
