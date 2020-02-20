all: 	sample
sample:		main.o
			cc -o sample main.o

main.o	:	main.cpp	header.h
			cc -c main.cpp

clean	:
			rm main.o sample 