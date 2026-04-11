# scripts/train_model.py

import pandas as pd
import numpy as np
import joblib
import os
from sklearn.ensemble import RandomForestClassifier
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score, classification_report, confusion_matrix
import matplotlib.pyplot as plt
import seaborn as sns

DATASET_PATH = "data/processed/dataset.csv"
MODEL_PATH = "models/sign_classifier.pkl"


def load_dataset():
    df = pd.read_csv(DATASET_PATH)

    # separate features from the label before any processing
    labels = df["label"].values
    landmarks = df.drop(columns=["label"]).values

    return landmarks, labels


def normalize(landmarks):
    """
    Subtracts the wrist position (point 0) from all other points so the model
    learns hand shape rather than hand position on screen.
    Without this, the same sign in different screen positions would look like
    different inputs to the model.
    """
    normalized = landmarks.copy()
    for i in range(len(normalized)):
        # wrist coordinates are the first 3 values: x0, y0, z0
        wrist = normalized[i, :3]
        # reshape to (21, 3) to subtract wrist from each of the 21 points
        points = normalized[i].reshape(21, 3)
        points = points - wrist
        normalized[i] = points.flatten()
    return normalized


def train(X_train, y_train):
    # n_estimators is the number of trees in the forest
    # more trees = more stable predictions, but slower training
    # 100 is a reliable default for this size of dataset
    model = RandomForestClassifier(n_estimators=100, random_state=42)
    model.fit(X_train, y_train)
    return model


def evaluate(model, X_test, y_test):
    predictions = model.predict(X_test)
    accuracy = accuracy_score(y_test, predictions)

    print(f"\nAccuracy: {accuracy * 100:.2f}%")
    print("\nDetailed report:")
    print(classification_report(y_test, predictions))

    plot_confusion_matrix(y_test, predictions, model.classes_)

    return accuracy


def plot_confusion_matrix(y_test, predictions, classes):
    """
    Visualizes which signs the model confuses with each other.
    Dark cells on the diagonal = correct predictions.
    Any bright cell off the diagonal = a confusion worth investigating.
    """
    cm = confusion_matrix(y_test, predictions, labels=classes)

    plt.figure(figsize=(14, 12))
    sns.heatmap(cm, annot=True, fmt="d", cmap="Blues",
                xticklabels=classes, yticklabels=classes)
    plt.xlabel("Predicted")
    plt.ylabel("Actual")
    plt.title("Confusion Matrix")
    plt.tight_layout()

    os.makedirs("data", exist_ok=True)
    plt.savefig("data/confusion_matrix.png")
    print("\nConfusion matrix saved to data/confusion_matrix.png")
    plt.show()


def main():
    print("Loading dataset...")
    X, y = load_dataset()
    print(f"  {len(X)} samples, {len(set(y))} signs: {sorted(set(y))}")

    print("\nNormalizing landmarks...")
    X = normalize(X)

    # stratify ensures each sign is proportionally represented in both
    # train and test sets — prevents the split from being accidentally unbalanced
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42, stratify=y
    )
    print(f"  Train: {len(X_train)} samples | Test: {len(X_test)} samples")

    print("\nTraining Random Forest...")
    model = train(X_train, y_train)

    print("\nEvaluating...")
    accuracy = evaluate(model, X_test, y_test)

    if accuracy >= 0.80:
        joblib.dump(model, MODEL_PATH)
        print(f"\nModel saved to {MODEL_PATH}")
    else:
        print(f"\nAccuracy below 80% — model not saved.")
        print("Consider collecting more samples or checking for labeling errors.")


if __name__ == "__main__":
    main()