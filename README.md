# Team Osiris - FS19_J1939_Converter
The FAST system uses Farm Simulator 2019 to generate telemetry data. The system will convert the raw values into J1939 messages, broadcast them on the CAN network and then display the messages in a graphical manner. There are 4 parts to the system: 
 - <b>FS_DataReader</b></br>
 This is the mod that will get values from the driven tracktor and save them to a file for the FAST_Converter.
 - <b>FAST_Converter</b></br>
The FAST_Converter takes the raw from the file that the FS_DataReader generates. Based on what the raw value is representing the Converter will get the corresponding PGN and SPN information from the database. The Converter will then use the information from the database to generate a message following the J1939 protocol.
 - <b>CAN Relay</b></br>
The CAN Relay is the part of the system that resides on the Linux operating system. The CAN Relay will recieve messages from the converter and broadcast them to the CAN network. The CAN Relay will also read from the CAN network and send them to the FAST_UI
 - <b>FAST_UI</b></br>
The FAST_UI will get messages from the CAN Relay, parse them for the PGN, based on the PGN it will query the database for information to convert the J1939 messages into raw values. The FAST_UI will then display the values on a graph.
</br>
</br>
There are a few different ways to run the FAST system:</br>

1. Full
2. Without FS
3. Without Linux
4. Without Database (No internet)

The system can also run with any combination of 2-4.

## prerequisites
Farming Simulator 2019 </br>

## Installation
### Getting Oracle VirtualBox

1.	Go to https://www.virtualbox.org 
2.	Click Download
3.	Click on Windows hosts
4.	Run the downloaded .exe
5.	Follow the installation wizard

### Getting the Virtual Machine

1.	Go to: https://drive.google.com/file/d/1_5uy8hCdMDC8YeB4XDLB_iRQ1Q3miWOW/view?usp=sharing 
2.	Download Ubuntu.ova

### Importing the Virtual Machine into VirtualBox

1.	Open VirtualBox
2.	Click on File>Import Appliance…
3.	Navigate to the Ubuntu.ova file that was downloaded
4.	Click next
5.	Click Import

### Getting the FAST System

1.	Go to https://github.com/billyParmenter/FS19_J1939_Converter
2.	Download the zip
3.	Extract the zip contents

### Add the Mod to Farm Simulator

1.	Copy the FS_DataReader folder
2.	Now you need to locate the mods folder. Usually, it’s located here: C:\Users\[Your Username]\Documents\My Games\FarmingSimulator2019\mods
3.	Open the folder and paste the folder
4.	Right click on FS_DataReader and click on send to>Compressed(Zipped) folder

## Authors

[Sam Lagunju](https://github.com/SamueLagunju) </br>
[Connor Lynch](https://github.com/GetLynched) </br>
[Mike Ramoutsakis](https://github.com/jrmoca) </br>
[Billy Parmenter](https://github.com/billyParmenter)
