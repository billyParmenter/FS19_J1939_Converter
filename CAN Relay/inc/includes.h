/*
* Filename      :   includes.c
* Version Date  :   2020-04-05
* Programmer    :   Oloruntoba Samuel Lagunju
* Description   :   This file contains the necessary includes for main.c
*/

//Default includes
#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdint.h>
#include <stdbool.h>
#include <err.h>

//Socket includes
#include <sys/socket.h>
#include <net/if.h>
#include <sys/types.h>
#include <sys/ioctl.h>
//SocketCAN includes
#include <linux/can.h>
#include <linux/can/raw.h>
#include <linux/can/bcm.h>
//Threading Includes
#include <pthread.h>

#include "CANJ1939Conv.h"
#include "SocketFunc.h"

#define ARG_ERROR -1
#define SOCKET_ERROR 0
void startupInfo();