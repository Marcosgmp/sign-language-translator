import cv2


class CameraManager:
    """
    Manages webcam initialization, frame capture and resource release.

    This class isolates all camera-related responsibilities from the main
    application flow to improve maintainability and readability.
    """

    def __init__(self, index=0):
        # Opens the selected camera using the native Linux V4L2 backend.
        self.cap = cv2.VideoCapture(index, cv2.CAP_V4L2)

        # Configures the capture stream for lower latency and better compatibility.
        self.cap.set(cv2.CAP_PROP_FOURCC, cv2.VideoWriter_fourcc(*"MJPG"))
        self.cap.set(cv2.CAP_PROP_FRAME_WIDTH, 640)
        self.cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)
        self.cap.set(cv2.CAP_PROP_BUFFERSIZE, 1)

    def is_open(self):
        """
        Checks whether the camera device was successfully initialized.
        """
        return self.cap.isOpened()

    def read(self):
        """
        Captures and returns the latest available frame.

        Returns:
            ndarray | None: current frame or None if capture fails.
        """
        ret, frame = self.cap.read()
        return frame if ret else None

    def release(self):
        """
        Releases the camera resource.
        """
        self.cap.release()