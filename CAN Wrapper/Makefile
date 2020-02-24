all: 		Reader
Reader:		startup.o
			g++ -o Reader startup.o

startup.o	:	startup.c	 ProgramDisplay.c reader.h
			gcc startup.c ProgramDisplay.c -o startup


clean	:
			rm startup.o Reader 



# program: main.o Planet.o Body.o
#     g++ main.o Planet.o Body.o -o program

# main: main.cpp
#     g++ -c main.cpp

# Body.o: Body.cpp
#     g++ -c Body.cpp 

# Planet.o: Planet.cpp
#     g++ -c Planet.cpp 