#include <linux/can.h>
#include <linux/can/raw.h>
#include <endian.h>
#include <net/if.h>
#include <sys/ioctl.h>
#include <sys/socket.h>
#include <sys/types.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/types.h> 
#include <sys/socket.h>
#include <netinet/in.h>
#include <pthread.h>
#include <err.h>
#include <stdbool.h>
#include <sys/socket.h>      
#include <errno.h>



#define PARSE_ERROR 3
#define THREAD_ERROR 0
#define CAN_ERROR -3
#define THREAD_SUCCESS 1

// struct can_frame {
//     canid_t can_id;  /* 32 bit CAN_ID + EFF/RTR/ERR flags */
//     __u8    can_dlc; /* frame payload length in byte (0 .. 8) */
//     __u8    __pad;   /* padding */
//     __u8    __res0;  /* reserved / padding */
//     __u8    __res1;  /* reserved / padding */
//     __u8    data[8] __attribute__((aligned(8)));
// };
// Struct used to capture all parts of the data being transmitted from the converted
struct incomingCANMsg {

    unsigned int canID;
    char firstDataBit[8];
    char secondDataBit[8];
};
void *socCANBroadcast(void *recvMsg);
void *socCANRead(void *outputMsg);
int getSize (char * s);