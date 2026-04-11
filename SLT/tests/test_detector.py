# tests/test_detector.py

import cv2
import sys
sys.path.append('.')

from src.detection.hand_detector import HandDetector

detector = HandDetector()
cap = cv2.VideoCapture(0)

print("Show your hand to the camera. Press Q to quit.")

while True:
    ret, frame = cap.read()
    if not ret:
        break

    annotated_frame, landmarks = detector.detect(frame)

    if landmarks is not None:
        print(f"Landmarks extracted: shape={landmarks.shape}, wrist={landmarks[:3]}")
    else:
        print("No hand detected")

    cv2.imshow("HandDetector Test", annotated_frame)

    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()