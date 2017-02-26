#include <SoftwareSerial.h>
#include <LiquidCrystal.h>
#include <Engine.h>

SoftwareSerial ESP(9, 10);
LiquidCrystal LCD(12, 11, 5, 4, 3, 2);
Engine* engine;
EngineData data;

void setup() {
  ESP.begin(115200);
  ESP.setTimeout(5000);

  LCD.begin(16, 2);
  LCD.noCursor();
  
  Serial.begin(115200);
  Serial.setTimeout(5000);

  ESP.println("AT");
  delay(1000);
  
  ESP.println("AT+RST");
  delay(1000);
  
  Serial.println("ESP8266 Demo");
  Serial.println("Setting up... \n");

  engine = new Engine(ESP, LCD, Serial);
}

void loop() { 
  engine -> update(data);
}

