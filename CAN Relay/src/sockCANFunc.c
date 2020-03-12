#include "../inc/sockCANFunc.h"

void *socCANBroadcast(void *recvMsg)
{
    int broadcastSocket; //Endpoint for communication

    if((broadcastSocket = socket(PF_CAN, SOCK_RAW, CAN_RAW)) < 0) 
    {
        printf("Error");
    }

    pthread_exit(NULL);
}
