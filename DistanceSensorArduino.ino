#define trigPin 12
#define echoPin 11
 
void setup() {
  Serial.begin (9600);
  pinMode(trigPin, OUTPUT); //distance sensor - output
  pinMode(echoPin, INPUT); //distance sensor - input
  pinMode(2, OUTPUT); //out for buzzer
}
 
void loop() {  
  int staracurrentLength = 0;
  int currentLength = getDistance();
  Serial.print(currentLength);
  Serial.print("\n");
  delay(400);
  
  range(15, 40, currentLength); //turn alarm when an object is in length from 15 to 40 centimeters
  delay(500);
} 

int getDistance() {
  long timeElapsed, distance;
 
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);
 
  timeElapsed = pulseIn(echoPin, HIGH);
  distance = timeElapsed / 58;
 
  return distance;
}
 
void range(int a, int b, int currentLength) {
  int calculatedDistance = currentLength;
    if ((calculatedDistance > a) && (calculatedDistance < b)) {
      digitalWrite(8, HIGH); // Turn on the diod
  } else {
      digitalWrite(8, LOW); //Turn off the diod, when object is out of range 
  }
}
