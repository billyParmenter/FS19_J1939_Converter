@echo off

REM
REM FILE          : Setup.bat
REM PROJECT       : FS19_J1939_Converter
REM PROGRAMMER    : Oloruntoba 'Samuel' Lagunju
REM DATE          : 2020-02-09
REM DESCRIPTION   : This is a batch script that 
REM                 1. Zips and moves a folder to the mods folder of farming simulator 2019
REM                 2.  



REM File Relocation process
echo Setting up Mod...
@echo off
call ModRelocationScript.bat zipDirItems -source ..\FS_DataReader\ -destination "C:\Users\%username%\Documents\My Games\FarmingSimulator2019\mods\"DataReader.zip -keep yes -force no

echo Executing Farming Simulator 2019...
REM Running farming simulator
start steam://rungameid/787860

:UserInput
    SET /p id = "Enter Next to continue (once in a vehicle with Mod enabled): "
    REM Incorrect syntax command.
    IF /I "%id%"=="Next"( 
        echo Starting Converter! 
        REM   Running Converter filer
        REM     echo Starting Converter!
        REM  D:\>TIMEOUT /T 200
        start ..\Converter\J1939Converter\bin\Debug\J1939Converter.exe
    )

    REM ELSE (
    REM     echo Invalid Input!
    REM     goto UserInput)
