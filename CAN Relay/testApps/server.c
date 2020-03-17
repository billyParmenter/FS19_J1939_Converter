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
#include <netdb.h>

int main(int argc, char *argv[])
{
	int sockfd, newsockfd;
	char buffer[256];
	socklen_t clien;
	struct sockaddr_in serv_addr, client_addr;

	sockfd = socket(AF_INET, SOCK_STREAM, 0);
	if(sockfd < 0)
	{
		printf("Error: Could not open socket\n");
		exit(0);
	}

}
