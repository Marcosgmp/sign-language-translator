import cv2
import csv
import os
import sys
import time

# makes src/ visible when running from the project root
sys.path.append('.')
from src.detection.hand_detector import HandDetector

SAMPLES_PER_SIGN = 200
OUTPUT_PATH = "data/processed/dataset.csv"

# J and Z require motion — a single frame cannot represent them
SIGNS = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "K",
         "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
         "V", "W", "X", "Y"]


def setup_output_file():
    os.makedirs("data/processed", exist_ok=True)

    file_exists = os.path.isfile(OUTPUT_PATH)
    if not file_exists:
        with open(OUTPUT_PATH, "w", newline="") as f:
            writer = csv.writer(f)
            # 63 columns for landmarks (21 points × x, y, z) + 1 for the label
            header = [f"{axis}{i}" for i in range(21) for axis in ["x", "y", "z"]]
            header.append("label")
            writer.writerow(header)
        print(f"Created new dataset file at {OUTPUT_PATH}")
    else:
        print(f"Appending to existing dataset at {OUTPUT_PATH}")


def count_existing_samples(label):
    # avoids restarting collection from zero if the script was interrupted
    if not os.path.isfile(OUTPUT_PATH):
        return 0

    count = 0
    with open(OUTPUT_PATH, "r") as f:
        reader = csv.DictReader(f)
        for row in reader:
            if row["label"] == label:
                count += 1
    return count


def collect_sign(detector, cap, label):
    already_collected = count_existing_samples(label)
    if already_collected >= SAMPLES_PER_SIGN:
        print(f"  '{label}' already complete, skipping.")
        return

    samples_needed = SAMPLES_PER_SIGN - already_collected
    collected = 0

    print(f"\n--- Sign: {label} ---")
    print(f"  {already_collected}/{SAMPLES_PER_SIGN} already collected, {samples_needed} remaining.")
    print("  Starting in 3 seconds...")
    time.sleep(3)

    with open(OUTPUT_PATH, "a", newline="") as f:
        writer = csv.writer(f)

        while collected < samples_needed:
            ret, frame = cap.read()
            if not ret:
                continue

            annotated_frame, landmarks = detector.detect(frame)

            if landmarks is not None:
                # each row is one sample: 63 landmark values + the sign label
                writer.writerow(list(landmarks) + [label])
                collected += 1

            progress_text = f"{label}: {already_collected + collected}/{SAMPLES_PER_SIGN}"
            cv2.putText(annotated_frame, progress_text, (10, 40),
                        cv2.FONT_HERSHEY_SIMPLEX, 1.2, (0, 200, 100), 2)

            cv2.imshow("Data Collection", annotated_frame)

            if cv2.waitKey(1) & 0xFF == ord('q'):
                print("  Interrupted.")
                return

    print(f"  Done — {samples_needed} new samples saved for '{label}'.")


def main():
    setup_output_file()

    detector = HandDetector()
    cap = cv2.VideoCapture(0)

    print("=== SLT Data Collection ===")
    print(f"Signs: {SIGNS}")
    print(f"Samples per sign: {SAMPLES_PER_SIGN}")
    print("Press Q to stop. Progress is saved automatically.\n")

    for sign in SIGNS:
        collect_sign(detector, cap, sign)

    cap.release()
    cv2.destroyAllWindows()
    print("\nCollection complete. Dataset saved to", OUTPUT_PATH)


if __name__ == "__main__":
    main()