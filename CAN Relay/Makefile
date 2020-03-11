all: 		Reader
Reader:		main.o CANJ1939Conv.o SocketFunc.o
			gcc -o Reader main.o CANJ1939Conv.o SocketFunc.o

main.o	:	main.c	 includes.h
			gcc -c main.c 

CANJ1939Conv.o	:	CANJ1939Conv.c	 CANJ1939Conv.h
			gcc -c CANJ1939Conv.c 

SocketFunc.o	:	SocketFunc.c	SocketFunc.h
			gcc -c SocketFunc.c 
clean	:
			rm *.o Reader 