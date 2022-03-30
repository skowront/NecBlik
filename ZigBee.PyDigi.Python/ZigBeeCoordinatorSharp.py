from ZigBeeCoordinator import Coordinator

coordinator: Coordinator = None;

def Initialize(port:str,baudRate:int):
    coordinator = Coordinator(port,baudRate);
    return coordinator;

def GetVersion(coordinator:Coordinator):
    coordinator.Open();
    r = coordinator.xbee.get_firmware_version();
    coordinator.Close();
    return r;

def GetAddress(coordinator:Coordinator):
    coordinator.Open();
    r = coordinator.xbee.get_64bit_addr();
    coordinator.Close();
    return r;

def Test():
    coordinator = Coordinator("COM4",9600);
    coordinator.Open();
    print(GetVersion(coordinator));
    coordinator.Close();