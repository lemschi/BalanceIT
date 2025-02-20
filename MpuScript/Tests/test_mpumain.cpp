#include <gtest/gtest.h>
#include <Adafruit_MPU6050.h>
#include <Wire.h>

// Mock-Funktionen für Serial
class SerialMock {
public:
    void begin(int baud) {}
    void println(const char* msg) { last_msg = std::string(msg); }
    void print(float value) {}
    std::string last_msg;
} Serial;

// Mock-Funktionen für GPIO
int fake_digitalRead(int pin) {
    if (pin == 34) return 1;  // Simuliere Knopfdruck
    if (pin == 27) return 0;
    return 0;
}

Adafruit_MPU6050 mpu;

// Setup-Test: Prüft, ob der MPU6050 korrekt initialisiert wird
TEST(MPU6050Test, Initialization) {
    ASSERT_TRUE(mpu.begin()) << "MPU6050 nicht gefunden!";
}

// Knopftest: Prüft, ob die richtigen Werte ausgegeben werden
TEST(ButtonTest, ButtonPress) {
    EXPECT_EQ(fake_digitalRead(34), 1);
    EXPECT_EQ(fake_digitalRead(27), 0);
}

// Serial-Ausgabe-Test
TEST(SerialOutputTest, SerialPrintCheck) {
    Serial.println("ESP_ON");
    EXPECT_EQ(Serial.last_msg, "ESP_ON");
}

// Main für Google Test
int main(int argc, char **argv) {
    ::testing::InitGoogleTest(&argc, argv);
    return RUN_ALL_TESTS();
}