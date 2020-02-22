/* SPDX-License-Identifier: GPL-2.0-only */
/*
 * Copyright (c) 2013 EIA Electronics
 *
 * Authors:
 * Kurt Van Dijck <kurt.van.dijck@eia.be>
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the version 2 of the GNU General Public License
 * as published by the Free Software Foundation
 */

#include <signal.h>
#include <time.h>
#include <inttypes.h>
#include <errno.h>
#include <string.h>
#include <stdlib.h>
#include <stdio.h>

#include <unistd.h>
#include <getopt.h>
#include <err.h>
#include <sys/time.h>
#include <sys/socket.h>
#include <net/if.h>

#include "libj1939.h"

static const char help_msg[] =
	"testj1939: demonstrate j1939 use\n"
	"Usage: test FROM TO\n"
	" FROM / TO	- or [IFACE][:[SA][,[PGN][,NAME]]]\n"
	"Options:\n"
	" -v		Print relevant API calls\n"
	"Example:\n"
	"test -v vcan0:0x80\n\n";

static const char optstring[] = "?vbBPos::rep:cnw::";


int main(int argc, char *argv[])
{
	int ret;
	int sock;
	int opt;
	int j;
	int verbose = 0;

	struct sockaddr_can sockname = {
		.can_family = AF_CAN,
		.can_addr.j1939 = {
			.addr = J1939_NO_ADDR,
			.name = J1939_NO_NAME,
			.pgn = J1939_NO_PGN,
		},
	};

	struct sockaddr_can peername = {
		.can_family = AF_CAN,
		.can_addr.j1939 = {
			.addr = J1939_NO_ADDR,
			.name = J1939_NO_NAME,
			.pgn = J1939_NO_PGN,
		},
	};

	uint8_t dat[128];
	int todo_send = 8;
	int todo_broadcast = 1;

	//Argument parsing
	while ((opt = getopt(argc, argv, optstring)) != -1)
	{
		switch (opt) 
		{
			case 'v':
				verbose = 1;
				break;
			default:
				fputs(help_msg, stderr);
				exit(1);
				break;
		}
	}

	//Set source
	if (argv[optind]) 
	{
		if (strcmp("-", argv[optind]))
		{
			libj1939_parse_canaddr(argv[optind], &sockname);
			libj1939_parse_canaddr(argv[optind], &peername);
		}

		++optind;
	}
	
	//open socket
	if (verbose)
	{
		fprintf(stderr, "- socket(PF_CAN, SOCK_DGRAM, CAN_J1939);\n");
	}
	sock = ret = socket(PF_CAN, SOCK_DGRAM, CAN_J1939);
	if (ret < 0)
	{
		fprintf(stderr, "Try: test -h for help\n");
		err(1, "socket(j1939)");
	}

	//Set broadcast socket settings
	if (verbose)
	{
		fprintf(stderr, "- setsockopt(, SOL_SOCKET, SO_BROADCAST, %d, %zd);\n",
			todo_broadcast, sizeof(todo_broadcast));
	}
	ret = setsockopt(sock, SOL_SOCKET, SO_BROADCAST, &todo_broadcast, sizeof(todo_broadcast));
	if (ret < 0)
	{
		fprintf(stderr, "Try: test -h for help\n");
		err(1, "setsockopt: filed to set broadcast");
	}
	
	//Bind socket
	if (verbose)
	{
		fprintf(stderr, "- bind(, %s, %zi);\n", libj1939_addr2str(&sockname), sizeof(sockname));
	}
	ret = bind(sock, (void *)&sockname, sizeof(sockname));
	if (ret < 0)
	{
		fprintf(stderr, "Try: test -h for help\n");
		err(1, "bind()");
	}

	//Make data
	for (j = 0; j < sizeof(dat); ++j)
	{
		dat[j] = ((2*j) << 4) + ((2*j+1) & 0xf);
	}

	//Send data
	if (verbose)
	{
		fprintf(stderr, "- send(, <dat>, %i, 0);\n", todo_send);
	}
	ret = send(sock, dat, todo_send, 0);
	if (ret < 0)
	{
		fprintf(stderr, "Try: test -h for help\n");
		err(1, "sendto");
	}
	

	return 0;
}

