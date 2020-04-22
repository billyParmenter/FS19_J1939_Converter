#!/bin/bash
sudo modprobe vcan
sudo ip link add vcan0 type vcan
sudo ip link set vcan0 up
./CAN\ Relay/bin/canRelay -c -s
