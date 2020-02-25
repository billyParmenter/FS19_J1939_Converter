#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <err.h>
#include <unistd.h>
#include <stdint.h>
#include <stdbool.h>
#include <net/if.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <sys/ioctl.h>
#include <linux/can.h>
#include <linux/can/raw.h>
#include <linux/can/bcm.h>
#include "CANJ1939Conv.h"

#define ARG_ERROR -1
void CANMain(char* argv[], bool rawOrJ1939);

void startupInfo()
{
    printf(	"Options:\n"
	        " -r		Send CAN RAW calls\n"
            " -j		Send CAN J1939 calls\n"
	        " -s		Run Setup\n"

	        "Example :\n"
	        "test [-r]||[-j] vcan0:0x80\n\n");
};