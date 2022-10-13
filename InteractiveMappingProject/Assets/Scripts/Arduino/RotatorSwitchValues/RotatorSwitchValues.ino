
typedef struct encoder_t {
  int pinA;
  int pinB;

  bool stateA;
  bool stateB;
  bool lastStateA;  

  int pos;
  bool dirCW;
};

int encodersAmount = 3;
encoder_t encoders[6];

void setup() {
  for (int i = 0; i < encodersAmount; i++) {
    setupEncoder(encoders[i], i);
  }
  Serial.begin (9600);
}

void loop() {
  for (int i = 0; i < encodersAmount; i++) {
    updateEncoder(encoders[i], i);
  }
}

void setupEncoder(encoder_t& encoder, int encoderID){
  encoder.pinA = encoderID*2 + 2;
  encoder.pinB = encoderID*2 + 3;
  pinMode(encoder.pinA, INPUT);
  pinMode(encoder.pinB, INPUT);
  /* Read Pin A
  Whatever state it's in will reflect the last position
  */
  encoder.lastStateA = digitalRead(encoder.pinA);
  encoder.pos = 0;
}

void updateEncoder(encoder_t& encoder, int encoderID) {
  encoder.stateA = digitalRead(encoder.pinA);
  encoder.stateB = digitalRead(encoder.pinB);
  if (encoder.stateA != encoder.lastStateA){
    // Send data for this encoder
    Serial.print(encoderID);
    Serial.print("\t");
    
    encoder.dirCW = encoder.stateA != encoder.stateB;
    if (encoder.dirCW) {
      encoder.pos ++;
    }
    else {
      encoder.pos --;
    }
    Serial.print(encoder.dirCW);
    Serial.print("\t");  

    Serial.print(encoder.pos);
    Serial.println();
    
    encoder.lastStateA = encoder.stateA;    
  }
}
