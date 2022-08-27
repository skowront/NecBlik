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
	zbTx = ZBTxRequest(addr64, payload, sizeof(uint8_t)*value.length());

	xbee.send(zbTx);
	delete payload;

	if (xbee.readPacket(500)) {
		if (xbee.getResponse().getApiId() == ZB_TX_STATUS_RESPONSE) {
			xbee.getResponse().getZBTxStatusResponse(txStatus);
			if (txStatus.getDeliveryStatus() == SUCCESS) {
			}
			else {
			}
		}
	}
	else if (xbee.getResponse().isError()) {
		Serial.println(xbee.getResponse().getErrorCode());
	}
	else {
	}
}

RecievedData RecieveValue(int timeout = 1)
{
	xbee.readPacket(timeout);
	if (xbee.getResponse().isAvailable())
	{
		if (xbee.getResponse().getApiId() == ZB_RX_RESPONSE)
		{
			xbee.getResponse().getZBRxResponse(rx);
			if (rx.getOption() == ZB_PACKET_ACKNOWLEDGED)
			{
				uint8_t* payload;
				payload = rx.getData();
				RecievedData rd;
				rd.recieved = true;
				rd.payload = (char*)payload;
				return rd;
			}
			else
			{
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
		recieved.payload.toCharArray(rxBuffer, rxBufferSize);
		if (strcmp(rxBuffer, GetValueCommand) == 0)
		{
			SendValue("Value:"+String(GetStoredValue()));
		}
		else if (strcmp(rxBuffer, GetChangingValueCommand) == 0)
		{
			SendValue("ChangingValue:" + String(GetStoredChangingValue()));
		}
		else if (strcmp(rxBuffer, HoldCommand) == 0)
		{
			ToggleHold();
		}
		else
		{
			recieved.payload.substring(0,strlen(SetValueCommand)).toCharArray(rxBuffer, rxBufferSize);
			if (strcmp(rxBuffer, SetValueCommand) == 0)
			{
				if (recieved.payload.length() < 10)
				{
				}
				else
				{
					int newValue = recieved.payload.substring(strlen(SetValueCommand)+1, recieved.payload.length()).toInt();
					SetStoredValue(newValue);
					//SendValue("New value set");
				}
			}
			else
			{
				recieved.payload.substring(0, strlen(EchoCommand)).toCharArray(rxBuffer, rxBufferSize);
				if (strcmp(rxBuffer, EchoCommand) == 0)
				{
					SendValue(recieved.payload);
				}
				else
				{
				}
			}
		}
		unsigned long serviceEndTime = millis();
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

	Serial.begin(115200);
	Serial.println("Serial initialized.");
	Serial1.begin(115200);
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
  
  delay(1000);
}

void clearPerfBuffer()
{
	for (int i = 0; i < perfBufferSize; i++)
	{
		perfBuffer[i] = 0;
	}
}
