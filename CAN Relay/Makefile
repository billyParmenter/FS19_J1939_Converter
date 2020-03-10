all: 		Reader
Reader:		startup.o CANJ1939Conv.o
			gcc -o Reader startup.o CANJ1939Conv.o

startup.o	:	startup.c	 includes.h
			gcc -c startup.c 

CANJ1939Conv.o	:	CANJ1939Conv.c	 CANJ1939Conv.h
			gcc -c CANJ1939Conv.c 
clean	:
			rm *.o Reader 