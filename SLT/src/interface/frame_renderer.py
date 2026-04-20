import cv2


class FrameRenderer:
    """
    Responsible for drawing visual elements over the video frame.

    This class centralizes all screen rendering logic to keep the main
    application flow focused only on orchestration.
    """

    def draw(self, frame, text, sign):
        """
        Draws the current sign, translated text and control hints.

        Parameters:
            frame (ndarray): processed video frame
            text (str): accumulated translated sentence
            sign (str): current recognized sign

        Returns:
            ndarray: frame ready for display
        """
        h, w = frame.shape[:2]

        # Creates a semi-transparent footer for better text readability.
        overlay = frame.copy()
        cv2.rectangle(overlay, (0, h - 100), (w, h), (0, 0, 0), -1)
        cv2.addWeighted(overlay, 0.5, frame, 0.5, 0, frame)

        # Displays the current detected sign.
        if sign:
            cv2.putText(
                frame,
                sign,
                (20, h - 30),
                cv2.FONT_HERSHEY_SIMPLEX,
                2.0,
                (0, 200, 100),
                3
            )

        # Displays the accumulated translated text.
        cv2.putText(
            frame,
            text if text else "...",
            (120, h - 30),
            cv2.FONT_HERSHEY_SIMPLEX,
            1.2,
            (255, 255, 255),
            2
        )

        # Displays keyboard shortcuts.
        cv2.putText(
            frame,
            "BACKSPACE: delete | C: clear | Q: quit",
            (10, 30),
            cv2.FONT_HERSHEY_SIMPLEX,
            0.55,
            (200, 200, 200),
            1
        )

        # Resizes the frame only for display purposes.
        return cv2.resize(frame, (960, 720))