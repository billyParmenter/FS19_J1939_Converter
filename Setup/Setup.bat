/*
* FILE          : Setup.bat
* PROJECT       : FS19_J1939_Converter
* PROGRAMMER    : Oloruntoba 'Samuel' Lagunju
* DATE          : 2020-02-09
* DESCRIPTION   : This is a batch script that 
                    1. Zips and moves a folder to the mods folder of farming simulator 2019
                    2.  
*/

/*

*/

//File Relocation process
echo Setting up Mod...
call ModRelocationScript.bat zipDirItems -source ..\FS_DataReader\ -destination "C:\Users\%username%\Documents\My Games\FarmingSimulator2019\mods\"DataReader.zip -keep yes -force no

echo Executing Farming Simulator 2019...
//Running farming simulator
start steam://rungameid/787860