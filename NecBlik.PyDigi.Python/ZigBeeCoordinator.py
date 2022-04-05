from digi.xbee import devices
from digi.xbee.devices import XBeeDevice;
from digi.xbee.models.protocol import IPProtocol;
from digi.xbee.models.options import DiscoveryOptions;
from digi.xbee.models.message import ExplicitXBeeMessage;
import string
from digi.xbee.packets.common import _encode_at_cmd
from digi.xbee.reader import DataReceived;
import numpy as np;
import time

import clr
from System import Action
from System import Object

from serial.serialutil import Timeout;

def BytesToString(data):
    out = str();
    for d in data:
        copy = d
        leftSide = (copy >> 4) << 4;
        print(leftSide);
        if leftSide >> 4 <10:
            out += str(leftSide >> 4)
        elif leftSide >> 4 == 10:
            out += 'A'
        elif leftSide >> 4 == 11:
            out += 'B'
        elif leftSide >> 4 == 12:
            out += 'C'
        elif leftSide >> 4 == 13:
            out += 'D'
        elif leftSide >> 4 == 14:
            out += 'E'
        elif leftSide >> 4 == 15:
            out += 'F'
        rightSide = d - leftSide;
        print(rightSide);
        if rightSide < 10:
            out += str(rightSide >> 4) 
        elif rightSide == 10:
            out += 'A'
        elif rightSide == 11:
            out += 'B'
        elif rightSide == 12:
            out += 'C'
        elif rightSide == 13:
            out += 'D'
        elif rightSide == 14:
            out += 'E'
        elif rightSide == 15:
            out += 'F'
    return out;

class ActionHolder:
    def __init__(self, callback):
        self.callback = callback;

def EmptyFunction(xbee_message):
    print("None");
    return;

#action = Action[Object](EmptyFunction);
#dataReceivedActionHolder = None;

#def my_data_received_callback(xbee_message):
#    if dataReceivedActionHolder == None:
#        print("Data recieved");
#    else:
#        dataReceivedActionHolder.callback.Invoke(xbee_message);
    
    #address = xbee_message.remote_device.get_64bit_addr()
    #data = xbee_message.data.decode("utf8")
    #print("Received data from %s: %s" % (address, data))

class Coordinator:
    def __init__(self,port:str,baudRate:int):
        self.port = port
        self.baudRate = baudRate
        self.devices = np.empty(0)
        self.xbee: XBeeDevice = XBeeDevice(self.port, self.baudRate)
        self.Open()
        print("Initializing")

    def DiscoverDevices(self):
        self.Open();
        self.xnet = self.xbee.get_network()
        self.xnet.start_discovery_process()
        while self.xnet.is_discovery_running():
            time.sleep(0.5);
        self.devices = self.xnet.get_devices()

    def Open(self):
        if(not self.xbee.is_open()):
            self.xbee.open()

    def Close(self):
        if(self.xbee.is_open()):
            self.xbee.close()

    def GetAddress(self):
        self.Open()
        return self.xbee.get_64bit_addr()

    def GetPanId(self):
        self.Open()
        return BytesToString(self.xbee.get_pan_id())

    def GetVersion(self):
        self.Open()
        return self.xbee.get_hardware_version().description

    def SendBroadcastData(self,data):
        self.Open();
        self.xbee.send_data_broadcast(data)

    def Send(self,data,address):
        b = bytes(data+"\0",encoding="utf-8");
        if(address == "" or address == None):
            self.SendBroadcastData(b)
        else:
            devs = self.xbee.get_network().get_devices()
            device = None
            for dev in devs:
                if(str(dev.get_64bit_addr()) == address):
                    device = dev
            if(device == None):
                return;
            self.xbee.send_data(device,b)

#coordinator = Coordinator("COM4",9600)
#coordinator.DiscoverDevices()
#coordinator.xbee.add_data_received_callback(my_data_received_callback)
#coordinator.xbee.add_data_received_callback(EmptyFunction)
#coordinator.Send("GetValue","0013A20040A739ED")






#address = "0013A20040A739ED";
#b = bytes("GetValue"+"\0",encoding="utf-8");
#devs = coordinator.xbee.get_network().get_devices()
#rdevice = None
#for dev in devs:
#    if(str(dev.get_64bit_addr()) == address):
#        rdevice = dev
#coordinator.xbee.send_data(rdevice,b)


