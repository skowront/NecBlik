from digi.xbee.devices import XBeeDevice;
from digi.xbee.models.protocol import IPProtocol;
from digi.xbee.models.options import DiscoveryOptions;
import string;
import numpy as np;
import time;

class Coordinator:
    def __init__(self,port:str,baudRate:int):
        self.port = port;
        self.baudRate = baudRate;
        self.devices = np.empty(0);
        self.xbee: XBeeDevice = XBeeDevice(self.port, self.baudRate);
        print("Initializing");

    def DiscoverDevices(self):
        self.xbee.open()
        self.xnet = self.xbee.get_network();
        self.xnet.start_discovery_process();
        while self.xnet.is_discovery_running():
            time.sleep(0.5);
        self.devices = self.xnet.get_devices();
        self.xbee.close();

    def add_expl_data_received_callback(self,action):
        self.xbee.add_expl_data_received_callback(action);
        return;

    def Open(self):
        if(not self.xbee.is_open()):
            self.xbee.open();

    def Close(self):
        if(self.xbee.is_open()):
            self.xbee.close();
