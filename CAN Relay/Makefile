# all: 		Reader
# Reader:		main.o CANJ1939Conv.o SocketFunc.o
# 			gcc -o Reader main.o CANJ1939Conv.o SocketFunc.o

# main.o	:	main.c	 includes.h
# 			gcc -c main.c 

# CANJ1939Conv.o	:	CANJ1939Conv.c	 CANJ1939Conv.h
# 			gcc -c CANJ1939Conv.c 

# SocketFunc.o	:	SocketFunc.c	SocketFunc.h
# 			gcc -c SocketFunc.c 
# clean	:
# 			rm *.o Reader 


CC = g++
LD = g++
CFLAGS = -c -g -Wall 
LDFLAGS = -pthread -o 
OBJ = $(OBJDIR)SocketFunc.o $(OBJDIR)CANJ1939Conv.o $(OBJDIR)sockCANFunc.o $(OBJDIR)main.o 
EXE = Reader
BINDIR = bin/
INCDIR = inc/
OBJDIR = obj/
SRCDIR = src/

$(BINDIR)$(EXE): $(OBJ)
	$(LD) $(OBJ) $(LDFLAGS) $(BINDIR)$(EXE)

$(OBJDIR)CANJ1939Conv.o: $(SRCDIR)CANJ1939Conv.c $(INCDIR)CANJ1939Conv.h
	$(CC) $(CFLAGS) $(SRCDIR)CANJ1939Conv.c $(LDFLAGS) $(OBJDIR)CANJ1939Conv.o

$(OBJDIR)SocketFunc.o: $(SRCDIR)SocketFunc.c $(INCDIR)SocketFunc.h
	$(CC) $(CFLAGS) $(SRCDIR)SocketFunc.c $(LDFLAGS) $(OBJDIR)SocketFunc.o 

$(OBJDIR)sockCANFunc.o: $(SRCDIR)sockCANFunc.c $(INCDIR)sockCANFunc.h
	$(CC) $(CFLAGS) $(SRCDIR)sockCANFunc.c $(LDFLAGS) $(OBJDIR)sockCANFunc.o 

$(OBJDIR)main.o: $(SRCDIR)main.c $(INCDIR)includes.h
	$(CC) $(CFLAGS) $(SRCDIR)main.c $(LDFLAGS)  $(OBJDIR)main.o 



clean:
	rm -f $(OBJDIR)*.o
	rm -f $(BINDIR)*
			