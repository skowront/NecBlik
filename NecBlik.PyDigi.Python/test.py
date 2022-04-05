from digi.xbee.devices import XBeeDevice

# TODO: Replace with the serial port where your local module is connected to.
PORT = "COM4"
# TODO: Replace with the baud rate of your local module.
BAUD_RATE = 9600
import time;

print(" +-----------------------------------------+")
print(" | XBee Python Library Receive Data Sample |")
print(" +-----------------------------------------+\n")

device = XBeeDevice(PORT, BAUD_RATE)

try:
    device.open()
    xnet = device.get_network()
    xnet.start_discovery_process()
    while xnet.is_discovery_running():
        time.sleep(0.5);
    devices = xnet.get_devices();

    def data_receive_callback(xbee_message):
        print("From %s >> %s" % (xbee_message.remote_device.get_64bit_addr(),
                                 xbee_message.data.decode()))

    device.add_data_received_callback(data_receive_callback)
    address = "0013A20040A739ED";
    b = bytes("GetValue"+"\0",encoding="utf-8");
    devs = device.get_network().get_devices()
    rdevice = None
    for dev in devs:
        if(str(dev.get_64bit_addr()) == address):
            rdevice = dev
    device.send_data(rdevice,b)

    #print("Waiting for data...\n")
    #input()

finally:
    pass;

