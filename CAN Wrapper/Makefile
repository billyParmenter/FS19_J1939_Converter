all: 		Reader
Reader:		startup.o
			cc -o Reader startup.o

startup.o	:	startup.cpp	 reader.h
			cc -c startup.cpp 

clean	:
			rm startup.o Reader 