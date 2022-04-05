from digi.xbee.devices import XBeeDevice
import time;


xbee = XBeeDevice("COM4", 9600)
xbee.open()

# Define callback.
def my_data_received_callback(xbee_message):
    address = xbee_message.remote_device.get_64bit_addr()
    data = xbee_message.data.decode("utf8")
    print("Received data from %s: %s" % (address, data))

xnet = xbee.get_network()
xnet.start_discovery_process()
while xnet.is_discovery_running():
    time.sleep(0.5);

# Add the callback.
xbee.add_data_received_callback(my_data_received_callback)

address = "0013A20040A739ED";
b = bytes("GetValue"+"\0",encoding="utf-8");
devs = xbee.get_network().get_devices()
rdevice = None
for dev in devs:
    if(str(dev.get_64bit_addr()) == address):
        rdevice = dev
xbee.send_data(rdevice,b)
