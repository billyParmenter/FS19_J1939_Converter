
/*
* FILE        : reader.h
* PROJECT     : CAN Reader
* DATE        : 2020-02-21
* DESCRIPTION : Contents in this file are constants and macro definitions,
                declarations of external variables and complex data types
*/

//Regular Includes
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>

//SocketCAN includes
#include <net/if.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <sys/ioctl.h>
#include <linux/can.h>
#include <linux/can/raw.h>
#include <linux/can/bcm.h>




#define ARG_ERROR -1