import os
import sys
import cv2

os.environ["QT_QPA_PLATFORM"] = "xcb"
os.environ["GDK_BACKEND"] = "x11"

from src.capture.camera_manager import CameraManager
from src.interface.frame_renderer import FrameRenderer
from src.detection.hand_detector import HandDetector
from src.recognition.sign_recognizer import SignRecognizer
from src.translation.text_builder import TextBuilder


def main():
    camera = CameraManager()
    renderer = FrameRenderer()
    detector = HandDetector()
    recognizer = SignRecognizer()
    builder = TextBuilder()

    if not camera.is_open():
        print("Erro ao abrir a câmera.")
        sys.exit(1)

    while True:
        frame = camera.read()

        if frame is None:
            continue

        processed_frame, landmarks = detector.detect(frame)

        sign = None
        if landmarks is not None:
            sign = recognizer.predict(landmarks)
            builder.update(sign)

        display_frame = renderer.draw(
            processed_frame,
            builder.text,
            sign
        )

        cv2.imshow("SLT - Sign Language Translator", display_frame)

        key = cv2.waitKey(1) & 0xFF

        if key == ord("q"):
            break
        elif key == 8:
            builder.backspace()
        elif key == ord("c"):
            builder.clear()

    camera.release()
    cv2.destroyAllWindows()


if __name__ == "__main__":
    main()