
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

#include <arpa/inet.h>
#include "Loggers.h"
#define SA struct sockaddr 

#define PARSE_ERROR 3
#define THREAD_ERROR 0
#define CAN_ERROR -3
#define THREAD_SUCCESS 1
#define CANID_DELIM ' '
#define DATA_SEPERATOR '.'
#define  EOF_MESSAGE_DELIM '\0'; 

#define LRG_BUFSIZ 256  //Definition used for a large size array

#define CAN_ID_LEN 8

#define DEFAULT_DB_PORT 4040
// Struct used to capture all parts of the data being transmitted from the converted
struct incomingCANMsg {

    unsigned int canID;
    char firstDataBit[8];
    char secondDataBit[8];
};


void *socCANBroadcast(void *recvMsg);
void *socCANRead(void* ipToDashboard);
int getSize (char * s);

void socketToDB(char* ipAddress, char* messageToBeSent);
unsigned char asciiToNibble(char canidChar);
char* data2hexstring(unsigned char *data);


int hexstring2data(char *arg, unsigned char *data, int maxdlen);
void sprint_canframe(char *buf , struct canfd_frame *cf, int sep, int maxdlen); 


//CAN-UTILS tools:
const char hex_asc_upper[] = "0123456789ABCDEF";

#define hex_asc_upper_lo(x)    hex_asc_upper[((x) & 0x0F)]
#define hex_asc_upper_hi(x)    hex_asc_upper[((x) & 0xF0) >> 4]

static inline void put_hex_byte(char *buf, __u8 byte)
{
    buf[0] = hex_asc_upper_hi(byte);
    buf[1] = hex_asc_upper_lo(byte);
}

static inline void _put_id(char *buf, int end_offset, canid_t id)
{
    /* build 3 (SFF) or 8 (EFF) digit CAN identifier */
    while (end_offset >= 0) {
        buf[end_offset--] = hex_asc_upper_lo(id);
        id >>= 4;
    }
}

#define put_sff_id(buf, id) _put_id(buf, 2, id)
#define put_eff_id(buf, id) _put_id(buf, 7, id)
