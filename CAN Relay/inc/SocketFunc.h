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
#include "sockCANFunc.h"



#define SOCKET_ERROR 0
#define THREAD_ERROR 0
#define SOCKET_SUCCESS 1
#define THREAD_SUCCESS 1


bool SocketSetup(int portNumber);