# miniproject-2025
IoT 미니프로젝트 2025

# Raspberry Pi 3D(3차원) 거리 측정 (VL53L0X + Servo Motor)

- 라즈베리파이와 VL53L0X 거리 센서, 2축 서보모터를 활용한 저비용 3D 거리 측정 프로젝트
- 센서 데이터를 실시간으로 수집해 3D 좌표로 변환하고, Python으로 시각화

---

##  주요 기능

- 2축 서보로 회전하며 거리 측정  
- 거리 + 각도 → 3D 좌표 변환  
- `matplotlib` 기반 실시간 3D 포인트맵 시각화  
- Raspberry Pi GPIO, I2C, PWM 연동  
- 저전력, 저비용 구성

---

## 부품 구성 목록

| 장치               | 설명                             | 추천 모델 / 키워드                        |
|--------------------|----------------------------------|-------------------------------------------|
| 서보모터 1         | 수평 회전용 (θ 축)               | SG90 / MG90S                              |
| 서보모터 2         | 수직 회전용 (φ 축)               | SG90 / MG90S                              |
| 거리 센서          | 적외선 TOF 거리 측정             | VL53L0X                                   |
| 라즈베리파이       | 제어 + 연산 + 시각화             | Raspberry Pi 4 (2GB~4GB)                  |
| Micro SD 카드      | OS 및 코드 저장                  | 16GB 이상, Class 10                       |
| 서보 드라이버보드  | 안정적인 PWM 제어                | PCA9685 (16채널)                          |
| 전원 공급장치      | 서보용 외부 전원                 | 5V 2A 어댑터 / 보조배터리 / 파워뱅크      |
| 서보 마운트 구조물 | 회전구조 지지대                  | 2축 회전 프레임 / 아크릴 / 3D 프린팅 파츠 |
| 점퍼 케이블 세트   | 모듈 연결용                      | Male-Female 점퍼케이블                    |
| 브레드보드 + 저항  | 시제품 연결 테스트               | 미니 브레드보드 / 기본 키트               |


## 프로젝트 구성

### 필요 이론

- 서보 θ (수평축) → 0° ~ 180° 회전

- 서보 φ (수직축) → 0° ~ 90° 회전

    ```
    x​=d⋅sin(ϕ)⋅cos(θ)
    y=d⋅sin(ϕ)⋅sin(θ)
    z=d⋅cos(ϕ)
    ```

### GPIO 핀

| Pin 번호 | GPIO (BCM) 번호  | 기능          | 설명                                         |
|----------|------------------|---------------|----------------------------------------------|
| 1        | -                | 3.3V 전원     | 3.3V 전압 출력 (센서 등 전원 공급용)         |
| 2        | -                | 5V 전원       | 5V 전압 출력 (서보모터 등 고전력 부품에 사용)|
| 3        | GPIO2            | I2C SDA       | I2C 데이터 라인 (VL53L0X 등 센서 통신용)     |
| 4        | -                | 5V 전원       | 5V 전압 출력 (2번째 핀)                      |
| 5        | GPIO3            | I2C SCL       | I2C 클럭 라인 (I2C 통신 동기화 역할)         |
| 6        | -                | GND           | 접지(Ground), 회로의 기준점 역할             |
| 7        | GPIO4            | GPIO          | 일반 디지털 입출력 핀                        |
| 8        | GPIO14           | UART TX       | 시리얼 통신 송신 (Serial Transmit)           |
| 9        | -                | GND           | 접지                                         |
| 10       | GPIO15           | UART RX       | 시리얼 통신 수신 (Serial Receive)            |
| 11       | GPIO17           | GPIO          | 일반 디지털 입출력 핀                        |
| 12       | GPIO18           | PWM 가능      | PWM 제어 가능 핀 (서보 모터 제어 등)         |
| 13       | GPIO27           | GPIO          | 일반 디지털 입출력 핀                        |
| 14       | -                | GND           | 접지                                         |
| 15       | GPIO22           | GPIO          | 일반 디지털 입출력 핀                        |
| 16       | GPIO23           | GPIO          | 일반 디지털 입출력 핀                        |
| 17       | -                | 3.3V 전원     | 추가 3.3V 출력                               |
| 18       | GPIO24           | GPIO          | 일반 디지털 입출력 핀                        |
| 19       | GPIO10           | SPI MOSI      | SPI 데이터 송신 핀 (Master Out Slave In)     |
| 20       | -                | GND           | 접지                                         |
| 21       | GPIO9            | SPI MISO      | SPI 데이터 수신 핀 (Master In Slave Out)     |
| 22       | GPIO25           | GPIO          | 일반 디지털 입출력 핀                        |
| 23       | GPIO11           | SPI SCLK      | SPI 클럭 신호 핀                             |
| 24       | GPIO8            | SPI CE0       | SPI 장치 선택 신호 (Chip Enable 0)           |
| 25       | -                | GND           | 접지                                         |
| 26       | GPIO7            | SPI CE1       | SPI 장치 선택 신호 (Chip Enable 1)           |
| 27       | GPIO0            | ID EEPROM SDA | HAT용 ID EEPROM 통신 핀 (SDA)                |
| 28       | GPIO1            | ID EEPROM SCL | HAT용 ID EEPROM 통신 핀 (SCL)                |
| 29       | GPIO5            | GPIO          | 일반 디지털 입출력 핀                        |
| 30       | -                | GND           | 접지                                         |
| 31       | GPIO6            | GPIO          | 일반 디지털 입출력 핀                        |
| 32       | GPIO12           | PWM 가능      | PWM 제어 가능 핀                             |
| 33       | GPIO13           | PWM 가능      | PWM 제어 가능 핀                             |
| 34       | -                | GND           | 접지                                         |
| 35       | GPIO19           | PWM 가능      | PWM 제어 가능 핀                             |
| 36       | GPIO16           | GPIO          | 일반 디지털 입출력 핀                        |
| 37       | GPIO26           | GPIO          | 일반 디지털 입출력 핀                        |
| 38       | GPIO20           | GPIO          | 일반 디지털 입출력 핀                        |
| 39       | -                | GND           | 접지                                         |
| 40       | GPIO21           | GPIO          | 일반 디지털 입출력 핀                        |