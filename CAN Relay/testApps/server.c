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
#define MAX 80 
#define PORT 8080 
#define SA struct sockaddr 
int main(int argc, char *argv[])
{
	int sockfd, len, connfd;
	char buffer[MAX];
	socklen_t clien;
	struct sockaddr_in servaddr, cli;

	sockfd = socket(AF_INET, SOCK_STREAM, 0);
	if(sockfd < 0)
	{
		printf("Error: Could not open socket\n");
		exit(0);
	}
	 bzero(&servaddr, sizeof(servaddr)); 
  
    // assign IP, PORT 
    servaddr.sin_family = AF_INET; 
    servaddr.sin_addr.s_addr = htonl(INADDR_ANY); 
    servaddr.sin_port = htons(PORT); 
  
    // Binding newly created socket to given IP and verification 
    if ((bind(sockfd, (SA*)&servaddr, sizeof(servaddr))) != 0) { 
        printf("socket bind failed...\n"); 
        exit(0); 
    } 
    else
        printf("Socket successfully binded..\n"); 
  
	while(1)
	{
		// Now server is ready to listen and verification 
		if ((listen(sockfd, 5)) != 0) { 
			printf("Listen failed...\n"); 
			exit(0); 
		} 
		else
			printf("Server listening..\n"); 
		len = sizeof(cli); 
	
		// Accept the data packet from client and verification 
		connfd = accept(sockfd, (SA*)&cli, &len); 
		if (connfd < 0) { 
			printf("server acccept failed...\n"); 
			exit(0); 
		} 
		else
			printf("server acccept the client...\n"); 
	
        bzero(buffer, MAX); 
		// read the message from client and copy it in buffer 
		read(sockfd, buffer, sizeof(buffer)); 
		// print buffer which contains the client contents 
		printf("From client: %s\n", buffer); 
		bzero(buffer, MAX); 
	}

    close(sockfd); 
}
