#include "../inc/includes.h"

static const char optstring[] = ":if:rjs";

int main(int argc, char *argv[])
{
    int opt, error;
	pthread_t socketThread;
	int portNumber = atoi(argv[2]);

    if(argc <= 1)
    {
        startupInfo();
        return ARG_ERROR;
    }

    //Argument parsing
	while ((opt = getopt(argc, argv, optstring)) != -1)
	{
		switch (opt) 
		{
			case 'r':
                // CANMain(argv, true);
				break;
            case 'j':
                // CANMain(argv, false);
                break;
			//Server mode
	        case 's':
				//Creating socket server connection
				if(SocketSetup(portNumber) == SOCKET_ERROR)
				{
					printf("SocketS Error! Check Logs for more info.");
				}
				break;
			default:
                startupInfo();
				break;
		}
	}

    
    return 0;
}






// void CANMain(char* argv[], bool rawOrJ1939)
// {
// 	int ret;
// 	int sock;
// 	int j;
// 	int verbose = 1;
// 	uint8_t dat[128];
// 	int todo_send = 8;
// 	int todo_broadcast = 1;
//     struct ifreq ifr;
//     struct sockaddr_can addr;
//     struct can_frame frame;
//     struct sockaddr_can sockname = {
//         .can_family = AF_CAN,
//         .can_addr.j1939 = {
//             .addr = J1939_NO_ADDR,
//             .name = J1939_NO_NAME,
//             .pgn = J1939_NO_PGN,
//         },
//     };

//     struct sockaddr_can peername = {
//         .can_family = AF_CAN,
//         .can_addr.j1939 = {
//             .addr = J1939_NO_ADDR,
//             .name = J1939_NO_NAME,
//             .pgn = J1939_NO_PGN,
//         },
//     };

//     //Set source
// 	if (argv[optind]) 
// 	{
// 		if (strcmp("-", argv[optind]))
// 		{
// 			libj1939_parse_canaddr(argv[optind], &sockname);
// 			libj1939_parse_canaddr(argv[optind], &peername);
// 		}

// 		++optind;
// 	}

//     //open socket
  
//     sock = ret = socket(PF_CAN, SOCK_DGRAM, CAN_J1939);

//     //iF rawOrJ1939 is True, create CAN RAW socket
//     if(rawOrJ1939)
//     {        
// 	    sock = ret = socket(PF_CAN, SOCK_RAW, CAN_RAW);
//         strcpy(ifr.ifr_name, "vcan0");
//         fprintf(stderr, "- socket(PF_CAN, SOCK_RAW, CAN_RAW);\n");
//     }

// 	if (ret < 0)
// 	{
// 		fprintf(stderr, "Try: test -h for help\n");
// 		err(1, "socket(j1939)");
// 	}

// 	//Set socket settings
// 	if (verbose)
// 	{
// 		fprintf(stderr, "- setsockopt(, SOL_SOCKET, SO_BROADCAST, %d, %zd);\n",
// 			todo_broadcast, sizeof(todo_broadcast));
// 	}
// 	ret = setsockopt(sock, SOL_SOCKET, SO_BROADCAST, &todo_broadcast, sizeof(todo_broadcast));
//     if (ret < 0)
// 	{
// 		fprintf(stderr, "Try: test -h for help\n");
// 		err(1, "setsockopt: filed to set broadcast");
// 	}
	
//     //iF rawOrJ1939 is True, Get the index of the network interface
//     if(rawOrJ1939)
//     {        
// 	    ret = ioctl(sock, SIOCGIFINDEX, &ifr);
//         fprintf(stderr, "- ioctl(sock, SIOCGIFINDEX, &ifr);\n");
//         printf("vcan0 at index %d\n", ifr.ifr_ifindex);
//     }

// 	//Bind socket
// 	if (verbose)
// 	{
// 		fprintf(stderr, "- bind(, %s, %zi);\n", libj1939_addr2str(&sockname), sizeof(sockname));
// 	}
// 	ret = bind(sock, (void *)&sockname, sizeof(sockname));
// 	if (ret < 0)
// 	{
// 		fprintf(stderr, "Try: test -h for help\n");
// 		err(1, "bind()");
// 	}

//     //iF rawOrJ1939 is True, Get the index of the network interface
//     if(rawOrJ1939)
//     {     
//         addr.can_family  = AF_CAN;
//         addr.can_ifindex = ifr.ifr_ifindex;   
// 	    ret = bind(sock, (struct sockaddr *)&addr, sizeof(addr));
//         printf("\33[2K\r");
//         printf("Rawbind\n");
//     }


// 	//Make data
// 	for (j = 0; j < sizeof(dat); ++j)
// 	{
// 		dat[j] = ((2*j) << 4) + ((2*j+1) & 0xf);
//         if(rawOrJ1939){frame.data[j] = ((2*j) << 4) + ((2*j+1) & 0xf);}
// 	}

// 	//Send data
// 	if (verbose)
// 	{
// 		fprintf(stderr, "- send(, <dat>, %i, 0);\n", todo_send);
// 	}
// 	ret = send(sock, dat, todo_send, 0);
// 	if (ret < 0)
// 	{
//         if(rawOrJ1939)
//         {
//             frame.can_id  = 0x80;
//             frame.can_dlc = 8;
//             ret = write(sock, &frame, sizeof(struct can_frame));
//             if (ret < 0)
//             {
//                 fprintf(stderr, "Try: test -h for help\n");
//                 err(1, "write is busted too");
//             }
//         }
//         fprintf(stderr, "Try: test -h for help\n");
//         err(1, "sendto");
// 	}
// }