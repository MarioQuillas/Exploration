#from math import sin, cos, radians
#import sys

#def make_dots(x):
#    return ' '*int(10*cos(radians(x)) + 10) + 'o'

#assert make_dots(90) == '          o'
#assert make_dots(180) == 'o'

#def main():
#    for i in range(100000):
#        s = make_dots(i)
#        print(s)

import sys
import argparse
from socket import *

def printBanner(connSock, tgtPort):
    try:
        # send data to target
        if(tgtPort == 80):
            connSock.send("GET HTTP/1.1 \r\n")
        else:
            connSock.send("\r\n")
        #receive data from target
        results = connSock.recv(4096)
        #print the banner
        print('[+] Banner:' + str(results))
    except :
        print('[-] Banner not available\n')
    

def connScan(tgtHost, tgtPort):
    try:
        #Create the socket object
        connSock = socket(AF_INET, SOCK_STREAM)

        #Try to connect with the target
        connSock.connect((tgtHost, tgtPort))

        print('[+] %d tcp open'% tgtPort)
        printBanner(connSock, tgtPort)
    except :
        #Print the failure results
        print('[+] %d tcp closed'% tgtPort)
    finally:
        #close the socket object
        connSock.close()

def portScan(tgtHost, tgtPorts):
    try:
        #if -a was not an ip address this will resolve to an ip/ if it's an ip address that's 
        tgtIP = gethostbyname(tgtHost)
    except:
        print("[-] Error: Unknown Host")
        exit(0)

    try:
        #if the domain can be resolved that's good, the result will be something like : 
        tgtName = gethostbyaddr(tgtIP)
        print("[+]--- Scan result for : " + tgtName[0] + " ---")
    except :
        print("[+]--- Scan result for : " + tgtIP + " ---")

    setdefaulttimeout(10)
    
    #For each port number call the connScan function
    for tgtPort in tgtPorts:
        connScan(tgtHost, int(tgtPort))

def main():
    # Parse the command line arguments
    parser = argparse.ArgumentParser('Smart TCP Client Scanner')
    parser.add_argument("-a", "--address", type=str, help="The target IP address")
    parser.add_argument("-p", "--port", type=str, help="The port number to connect")
    args = parser.parse_args()

    # Store the arguments values
    ipaddress = args.address
    portNumbers = args.port.split(',')

    # Call the Port Scan function
    portScan(ipaddress, portNumbers)

if __name__ == "__main__":
    sys.exit(int(main() or 0))