#include <Arduino.h>
#include <unity.h>
#include <Adafruit_MPU6050.h>
#include <Adafruit_Sensor.h>
#include <Wire.h>

// MPU6050-Sensor-Objekt
Adafruit_MPU6050 mpu;

// Test: MPU6050 sollte initialisiert werden können
void test_mpu6050_init(void) {
    bool status = mpu.begin();
    TEST_ASSERT_TRUE_MESSAGE(status, "MPU6050 initialization failed!");
}

// Test: Simulierte digitale Eingänge für Knopfmodule
void test_button_module(void) {
    pinMode(34, INPUT);
    pinMode(27, INPUT);

    // Simulierte Eingabe (HIGH für Modul1, LOW für Modul2)
    digitalWrite(34, HIGH);
    digitalWrite(27, LOW);

    TEST_ASSERT_EQUAL(HIGH, digitalRead(34));
    TEST_ASSERT_EQUAL(LOW, digitalRead(27));

    // Simulierte Eingabe (LOW für beide)
    digitalWrite(34, LOW);
    digitalWrite(27, LOW);

    TEST_ASSERT_EQUAL(LOW, digitalRead(34));
    TEST_ASSERT_EQUAL(LOW, digitalRead(27));
}

// Test: Simulierte Sensordaten vom MPU6050
void test_mpu6050_sensor_data(void) {
    sensors_event_t a, g, temp;
    mpu.getEvent(&a, &g, &temp);

    // Annahme: Die Werte sollten sich innerhalb eines sinnvollen Bereichs befinden
    TEST_ASSERT_FLOAT_WITHIN(10.0, 0.0, a.acceleration.x);
    TEST_ASSERT_FLOAT_WITHIN(10.0, 0.0, a.acceleration.y);
    TEST_ASSERT_FLOAT_WITHIN(10.0, 0.0, a.acceleration.z);
}

void setup() {
    UNITY_BEGIN(); // Starte Unity-Test-Framework
    RUN_TEST(test_mpu6050_init);
    RUN_TEST(test_button_module);
    RUN_TEST(test_mpu6050_sensor_data);
    UNITY_END(); // Beende Testausführung
}

void loop() {
    // Wird nicht benötigt für Unit-Tests
}
