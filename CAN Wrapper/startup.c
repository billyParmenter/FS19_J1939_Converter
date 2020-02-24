/*
* FILE        : startup.cpp
* PROJECT     : CAN Reader
* DATE        : 2020-02-21
* DESCRIPTION : Controls command line parsing,
*/
#include "reader.h"
//Class includes..
#include "ProgramDisplay.h"

int main(int argc, char* argv[])
{
    int opt;
    if(argc <= 1)
    {
        StartupInfo();
        return ARG_ERROR;
    }

    //Argument parsing
	while ((opt = getopt(argc, argv, "nt:")) != -1)
	{
		switch (opt) 
		{
            //Sending CAN_RAW data
			case 'r':
				break;
            //Sending CAN_J1939 data
            case 'j':
				break;
            case 's':
				//Potential Setup mode
				break;
			default:
                StartupInfo();
				break;
		}
	}

    // //CAN_RAW WRAPPING
    // int s;
    // int nbytes;
    // struct sockaddr_can addr;
    // struct can_frame frame;
    // struct ifreq ifr;

    // const char *ifname = "vcan0";

    // if((s = socket(PF_CAN, SOCK_RAW, CAN_RAW)) < 0) {
    //     perror("Error while opening socket");
    //     return -1;
    // }

    // strcpy(ifr.ifr_name, ifname);
    // ioctl(s, SIOCGIFINDEX, &ifr);
    
    // addr.can_family  = AF_CAN;
    // addr.can_ifindex = ifr.ifr_ifindex;

    // printf("%s at index %d\n", ifname, ifr.ifr_ifindex);

    // if(bind(s, (struct sockaddr *)&addr, sizeof(addr)) < 0) {
    //     perror("Error in socket bind");
    //     return -2;
    // }

    // frame.can_id  = 0x123;
    // frame.can_dlc = 2;
    // frame.data[0] = 0x11;
    // frame.data[1] = 0x22;

    // nbytes = write(s, &frame, sizeof(struct can_frame));

    // printf("Wrote %d bytes\n", nbytes);
    
    // return 0;
}