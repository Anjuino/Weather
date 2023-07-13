#include "DHTesp.h" // Click here to get the library: http://librarymanager/All#DHTesp
#ifdef ESP32
#pragma message(THIS EXAMPLE IS FOR ESP8266 ONLY!)
#error Select ESP8266 board.
#endif

DHTesp dht;
#include <ESP8266WiFi.h>

String api_key = "6NHG5LGR2BXBCGUL";     //  Введите свой ключ API записи из ThingSpeak
const char *ssid =  "EasyWay";     // замените на ваш Wi-Fi ssid и ключ wpa2
const char *pass =  "acea1459"; // замените на ваш пароль Wi-Fi
const char* server = "api.thingspeak.com";


WiFiClient client;
 
void setup() 
{
       Serial.begin(115200);
       delay(10);
       dht.setup(16, DHTesp::DHT11);
       Serial.println("Connecting to ");
       Serial.println(ssid);
 
       WiFi.begin(ssid, pass);
 
      while (WiFi.status() != WL_CONNECTED) 
     {
            delay(500);
            Serial.print(".");
     }
      Serial.println("");
      Serial.println("WiFi connected");
 
}
 
void loop() 
{
      delay(dht.getMinimumSamplingPeriod());

  float h = dht.getHumidity();
  float t = dht.getTemperature();
      
              if (isnan(h) || isnan(t)) 
                 {
                     Serial.println("Failed to read from DHT sensor!");
                     if (client.connect(server,80))   //   "184.106.153.149" или api.thingspeak.com
            {  
                   String data_to_send = api_key;
                    data_to_send += "&field3=";
                    data_to_send += 0;
                    data_to_send += "\r\n\r\n";

                   client.print("POST /update HTTP/1.1\n");
                   client.print("Host: api.thingspeak.com\n");
                   client.print("Connection: close\n");
                   client.print("X-THINGSPEAKAPIKEY: " + api_key + "\n");
                   client.print("Content-Type: application/x-www-form-urlencoded\n");
                   client.print("Content-Length: ");
                   client.print(data_to_send.length());
                   client.print("\n\n");
                   client.print(data_to_send);
                   delay(1000);
              }
             }

               if (client.connect(server,80))   //   "184.106.153.149" или api.thingspeak.com
            {  
                   String data_to_send = api_key;
                    data_to_send += "&field1=";
                    data_to_send += h;
                    data_to_send += "&field2=";
                    data_to_send += t;
                    data_to_send += "&field3=";
                    data_to_send += 1;
                    data_to_send += "\r\n\r\n";

                   client.print("POST /update HTTP/1.1\n");
                   client.print("Host: api.thingspeak.com\n");
                   client.print("Connection: close\n");
                   client.print("X-THINGSPEAKAPIKEY: " + api_key + "\n");
                   client.print("Content-Type: application/x-www-form-urlencoded\n");
                   client.print("Content-Length: ");
                   client.print(data_to_send.length());
                   client.print("\n\n");
                   client.print(data_to_send);
                   delay(1000);
                   Serial.print("Temperature: ");
                   Serial.print(t);
                   Serial.print(" degrees Celcius, Humidity: ");
                   Serial.print(h);
                   Serial.println("%. Send to Thingspeak.");
              }
          client.stop();
 
          Serial.println("Waiting...");
  
  delay(60000);
}
