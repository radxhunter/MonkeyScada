#define trigPin 12
#define echoPin 11
 
void setup() {
  Serial.begin (9600);
  pinMode(trigPin, OUTPUT); //Pin, do którego podłączymy trig jako wyjście
  pinMode(echoPin, INPUT); //a echo, jako wejście
  pinMode(2, OUTPUT); //Wyjście dla buzzera
}
 
void loop() {  
  int staracurrentLength = 0;
  int currentLength = getDistance();
  Serial.print(currentLength);
  Serial.print("\n");
  delay(400);
  
  zakres(10, 25, currentLength); //Włącz alarm, jeśli w odległości od 10 do 25 cm od czujnika jest przeszkoda  
  delay(500);
} 


 
int getDistance() {
  long czas, dystans;
 
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);
 
  czas = pulseIn(echoPin, HIGH);
  dystans = czas / 58;
 
  return dystans;
}
 
void zakres(int a, int b, int currentLength) {
  int jakDaleko = currentLength;
  if ((jakDaleko > a) && (jakDaleko < b)) {
      digitalWrite(8, HIGH); //Włączamy diodę
  } else {
      digitalWrite(8, LOW); //Wyłączamy diodę, gdy obiekt poza zakresem
  }
}
