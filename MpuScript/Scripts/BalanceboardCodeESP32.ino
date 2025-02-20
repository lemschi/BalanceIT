#include <Adafruit_MPU6050.h>
#include <Adafruit_Sensor.h>
#include <Wire.h>
 
Adafruit_MPU6050 mpu; //lybery objekt mpu
 
void setup()
{
  Serial.begin(115200);
  Serial.println("ESP_ON");

 
  while (!Serial)
    delay(20);
 
  if (!mpu.begin())
  {
    Serial.println("ERROR - MPU6050 nicht gefunden");
    while (1) {
      delay(20); //woatn das da Mpu online is
    }
  }
  Serial.println("MPU6050 gefunden");
 
}
 
 
 
void loop()
{
 
  //--------------------------Knopfmodulerkennung-------------------------------
    int modul1_read = digitalRead(34);
    int modul2_read = digitalRead(27);
 
    if (modul1_read == 1)
    {
      Serial.println("1"); //WICHTIG UNITY DATA HINZUFUEGEN bei beiden!
    }
 
    if (modul2_read == 1)
    {
      Serial.println("2");
    }
 
    //ANDGATE beide miasn 0 sei
    if (modul1_read == 0 && modul2_read == 0)
    {
      Serial.println("0");
    }
 
    //--------------------------GYRO Values-------------------------------
     sensors_event_t a, g, temp;
  mpu.getEvent(&a, &g, &temp);
  // asugabe DAten 
  Serial.print(a.acceleration.x);
  Serial.print(",");
  Serial.print(a.acceleration.y);
  Serial.print(",");
  Serial.print(a.acceleration.z);
 
 
 
 
  delay(100); 
}
