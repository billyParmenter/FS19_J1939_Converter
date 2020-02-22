/*
* FILE        : startup.cpp
* PROJECT     : CAN Reader
* DATE        : 2020-02-21
* DESCRIPTION : Controls command line parsing,
*/
#include "reader.h"


int main(int argc, char* argv[])
{
    //ProgramDisplay* display = ProgramDisplay::getInstance();;

    //checking number of arguments
    if(argc < 1)
    {
        //display->StartupInfo();
        return ARG_ERROR;
    }

    //CAN_RAW WRAPPING
    int s;
    int nbytes;
    struct sockaddr_can addr;
    struct can_frame frame;
    struct ifreq ifr;

    const char *ifname = "vcan0";

    if((s = socket(PF_CAN, SOCK_RAW, CAN_RAW)) < 0) {
        perror("Error while opening socket");
        return -1;
    }

    strcpy(ifr.ifr_name, ifname);
    ioctl(s, SIOCGIFINDEX, &ifr);
    
    addr.can_family  = AF_CAN;
    addr.can_ifindex = ifr.ifr_ifindex;

    printf("%s at index %d\n", ifname, ifr.ifr_ifindex);

    if(bind(s, (struct sockaddr *)&addr, sizeof(addr)) < 0) {
        perror("Error in socket bind");
        return -2;
    }

    frame.can_id  = 0x123;
    frame.can_dlc = 2;
    frame.data[0] = 0x11;
    frame.data[1] = 0x22;

    nbytes = write(s, &frame, sizeof(struct can_frame));

    printf("Wrote %d bytes\n", nbytes);
    
    return 0;
}