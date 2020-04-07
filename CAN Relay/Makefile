
CC = g++
LD = g++
CFLAGS = -c -g -Wall 
LDFLAGS = -pthread -o 
OBJ = $(OBJDIR)SocketFunc.o  $(OBJDIR)sockCANFunc.o $(OBJDIR)Logger.o $(OBJDIR)main.o 
EXE = canRelay
BINDIR = bin/
INCDIR = inc/
OBJDIR = obj/
SRCDIR = src/

$(BINDIR)$(EXE): $(OBJ)
	$(LD) $(OBJ) $(LDFLAGS) $(BINDIR)$(EXE)

# $(OBJDIR)CANJ1939Conv.o: $(SRCDIR)CANJ1939Conv.c $(INCDIR)CANJ1939Conv.h
# 	$(CC) $(CFLAGS) $(SRCDIR)CANJ1939Conv.c $(LDFLAGS) $(OBJDIR)CANJ1939Conv.o

$(OBJDIR)SocketFunc.o: $(SRCDIR)SocketFunc.c $(INCDIR)SocketFunc.h
	$(CC) $(CFLAGS) $(SRCDIR)SocketFunc.c $(LDFLAGS) $(OBJDIR)SocketFunc.o 

$(OBJDIR)sockCANFunc.o: $(SRCDIR)sockCANFunc.c $(INCDIR)sockCANFunc.h
	$(CC) $(CFLAGS) $(SRCDIR)sockCANFunc.c $(LDFLAGS) $(OBJDIR)sockCANFunc.o 

$(OBJDIR)main.o: $(SRCDIR)main.c $(INCDIR)includes.h
	$(CC) $(CFLAGS) $(SRCDIR)main.c $(LDFLAGS)  $(OBJDIR)main.o 

$(OBJDIR)Logger.o: $(SRCDIR)Loggers.c
	$(CC) $(CFLAGS) $(SRCDIR)Loggers.c $(LDFLAGS)  $(OBJDIR)Logger.o 

clean:
	rm -f $(OBJDIR)*.o
	rm -f $(BINDIR)*
			