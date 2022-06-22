#include "Printers.h"
#include "stdio.h"
//Xbee library defines max packet size = 110 bytes (100 bytes + overhead)-> #define MAX_FRAME_DATA_SIZE 110
//Therefore a fork of this library was attached with max sending value equal to 255. 
//https://github.com/andrewrapp/xbee-arduino/blob/wiki/DevelopersGuide.md
//#include "XBee.h"
struct RecievedData
{
	String payload;
	bool recieved = false;
};
unsigned long perfTime =0;
const int perfBufferSize = 60;
char perfBuffer[perfBufferSize];

//create the XBee object
XBee xbee = XBee();

//SH + SL Address of receiving XBee
//XBeeAddress64 addr64 = XBeeAddress64(0x0013A200, 0x40A739ED);
XBeeAddress64 addr64 = XBeeAddress64(0x000000000, 0x000000000);
ZBTxRequest zbTx;
ZBTxStatusResponse txStatus = ZBTxStatusResponse();
ZBRxResponse rx = ZBRxResponse();;

int statusLed = 13;
int errorLed = 13;

const char SetValueCommand [] = "SetValue";
const char EchoCommand [] = "Echo";
const char HoldCommand [] = "Toggle";
const char GetValueCommand [] = "GetValue";
const char GetChangingValueCommand [] = "GetCValue";

bool hold = true;
double StoredChangingValue = 0.0;
double degrees = 0;
unsigned long lastDataSent = 0;
int dataSendingInterval = 1; //in Seconds
double Amplitude = 10;
double DegreeIncrement = 1;

double GetStoredChangingValue()
{
	return StoredChangingValue;
}

void SetStoredChangingValue(double newValue) 
{
	StoredChangingValue = newValue;
}

void ToggleHold()
{
	if (hold == true)
	{
		hold = false;
	}
	else
	{
		hold = true;
	}
}

void OnTickChangeStoredChangingValue()
{
  perfTime = micros();

	if (abs(millis() - lastDataSent) > dataSendingInterval * 1000 && hold == false)
	{
		double radians = degrees * 1000 / 57296;
		SetStoredChangingValue((sin(radians)) * Amplitude);
		SendValue("StoredChangingValue:" + String(GetStoredChangingValue()));
		lastDataSent = millis();
		Serial.print("StoredChangingValue:");
		Serial.println(GetStoredChangingValue());
	}
	degrees+=DegreeIncrement;

  perfTime=micros()-perfTime;
	Serial.print("C#TD:Changing value simulation time:");
  Serial.println(perfTime);
	clearPerfBuffer();
}

int StoredValue = 31337;

int GetStoredValue()
{
	return StoredValue;
}

void SetStoredValue(int newValue)
{
	StoredValue = newValue;
	return;
}

void SendValue(String value)
{
	uint8_t* payload;
	payload = new uint8_t[value.length()+1];
	value.toCharArray((char*)payload,value.length()+1);
	Serial.print("Sending: ");
	Serial.println(value);
	Serial.print("DataLength: ");
	Serial.println(value.length()*sizeof(uint8_t));
	zbTx = ZBTxRequest(addr64, payload, sizeof(uint8_t)*value.length());

	xbee.send(zbTx);
	delete payload;

	if (xbee.readPacket(500)) {
		Serial.println("Response");
		if (xbee.getResponse().getApiId() == ZB_TX_STATUS_RESPONSE) {
			xbee.getResponse().getZBTxStatusResponse(txStatus);
			if (txStatus.getDeliveryStatus() == SUCCESS) {
				Serial.println("Response success");
			}
			else {
				Serial.println("Packet not delivered");
			}
		}
	}
	else if (xbee.getResponse().isError()) {
		Serial.print("Error reading packet.  Error code: ");
		Serial.println(xbee.getResponse().getErrorCode());
	}
	else {
		Serial.println("Local xbee error");
	}
}

RecievedData RecieveValue(int timeout = 1000)
{
	xbee.readPacket(timeout);
	if (xbee.getResponse().isAvailable())
	{
		Serial.println("Recieved a message");
		if (xbee.getResponse().getApiId() == ZB_RX_RESPONSE)
		{
			xbee.getResponse().getZBRxResponse(rx);
			if (rx.getOption() == ZB_PACKET_ACKNOWLEDGED)
			{
				Serial.println("Sender got an ACK");
				uint8_t* payload;
				payload = rx.getData();
				RecievedData rd;
				rd.recieved = true;
				Serial.println((char*)payload);
				rd.payload = (char*)payload;
				return rd;
			}
			else
			{
				Serial.println("Sender did not get an ACK");
			}
		}
	}
	RecievedData rd; rd.recieved = false;
	return rd;
}

void HandleRemoteCommunication()
{
	const int rxBufferSize = 512;
	char rxBuffer[rxBufferSize];
	unsigned long serviceStartTime = millis();

  perfTime = micros();

	RecievedData recieved = RecieveValue();

	if (recieved.recieved == true)
	{
		Serial.println("Analyzing recieved packet.");
		recieved.payload.toCharArray(rxBuffer, rxBufferSize);
		if (strcmp(rxBuffer, GetValueCommand) == 0)
		{
			Serial.println("GetValue command recieved");
			Serial.println("Sending stored value");
			SendValue("Value:"+String(GetStoredValue()));
		}
		else if (strcmp(rxBuffer, GetChangingValueCommand) == 0)
		{
			Serial.println("GetCValue command recieved");
			Serial.println("Sending stored changing value");
			SendValue("ChangingValue:" + String(GetStoredChangingValue()));
		}
		else if (strcmp(rxBuffer, HoldCommand) == 0)
		{
			Serial.println("Hold command recieved");
			Serial.print("Toggling hold. Current value -> ");
			Serial.println(hold);
			ToggleHold();
		}
		else
		{
			recieved.payload.substring(0,strlen(SetValueCommand)).toCharArray(rxBuffer, rxBufferSize);
			if (strcmp(rxBuffer, SetValueCommand) == 0)
			{
				Serial.println("SetValue command recieved");
				if (recieved.payload.length() < 10)
				{
					Serial.println("Wrong request");
				}
				else
				{
					int newValue = recieved.payload.substring(strlen(SetValueCommand)+1, recieved.payload.length()).toInt();
					SetStoredValue(newValue);
					Serial.print("Setting value to:");
					Serial.println(newValue);
					SendValue("New value set");
				}
			}
			else
			{
				recieved.payload.substring(0, strlen(EchoCommand)).toCharArray(rxBuffer, rxBufferSize);
				if (strcmp(rxBuffer, EchoCommand) == 0)
				{
					Serial.println("Echo command recieved");
					Serial.print("Echoing message of length:");
					Serial.println(recieved.payload.length());
					SendValue(recieved.payload);
				}
				else
				{
					Serial.println("Unknown command recieved");
					Serial.print("RxBuffer contains:");
					Serial.println(rxBuffer);
				}
			}
		}
		unsigned long serviceEndTime = millis();
		Serial.print("ServiceTime[ms]:");
		Serial.println(serviceEndTime-serviceStartTime);
	}
	//delete rxBuffer;

  perfTime=micros()-perfTime;
	Serial.print("C#TD:Service time:");
  Serial.println(perfTime);
	clearPerfBuffer();
}

void setup() {
	perfTime = micros();
	pinMode(statusLed, OUTPUT);
	pinMode(errorLed, OUTPUT);

	Serial.begin(9600);
	Serial.println("Serial initialized.");
	Serial1.begin(9600);
	Serial.println("Serial Xbee communication initialized.");


	xbee.setSerial(Serial1);
	perfTime = micros() - perfTime;
	sprintf(perfBuffer, "C#TD:Initialization time:%d", perfTime);
	Serial.println(perfBuffer);
	clearPerfBuffer();
}

void loop() {
  unsigned long loopTime = micros();

	HandleRemoteCommunication();
	OnTickChangeStoredChangingValue();

  loopTime = micros()-loopTime;
	Serial.print("C#TD:Loop time:");
  Serial.println(loopTime);
	clearPerfBuffer();
}

void clearPerfBuffer()
{
	for (int i = 0; i < perfBufferSize; i++)
	{
		perfBuffer[i] = 0;
	}
}
