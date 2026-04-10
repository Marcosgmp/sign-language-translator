import mediapipe as mp
import numpy as np

class HandDetector:
    """
    Responsabilidade: detectar mãos em um frame e extrair landmarks.
    Não sabe de onde veio o frame nem o que será feito com os dados.
    """

    def __init__(self, max_maos=1, confianca_deteccao=0.7, confianca_rastreamento=0.5):
        self._mp_hands = mp.solutions.hands
        self._mp_draw = mp.solutions.drawing_utils

        self._hands = self._mp_hands.Hands(
            max_num_hands=max_maos,
            min_detection_confidence=confianca_deteccao,
            min_tracking_confidence=confianca_rastreamento
        )

    def detectar(self, frame_bgr):
        """
        Recebe um frame BGR (formato padrão do OpenCV).
        Retorna (frame_anotado, landmarks) onde:
          - frame_anotado: frame com os pontos desenhados
          - landmarks: np.array de shape (63,) ou None se nenhuma mão detectada
        """
        # MediaPipe espera RGB, OpenCV entrega BGR — conversão obrigatória
        frame_rgb = frame_bgr[:, :, ::-1]  # inverte os canais sem copiar memória

        resultado = self._hands.process(frame_rgb)

        landmarks = None

        if resultado.multi_hand_landmarks:
            hand = resultado.multi_hand_landmarks[0]  # pega a primeira mão detectada

            # Desenha os pontos e conexões no frame original
            self._mp_draw.draw_landmarks(
                frame_bgr,
                hand,
                self._mp_hands.HAND_CONNECTIONS
            )

            landmarks = self._extrair_vetor(hand)

        return frame_bgr, landmarks

    def _extrair_vetor(self, hand_landmarks):
        """
        Converte os 21 landmarks em um vetor flat de 63 valores.
        Formato: [x0, y0, z0, x1, y1, z1, ..., x20, y20, z20]
        """
        pontos = []
        for landmark in hand_landmarks.landmark:
            pontos.extend([landmark.x, landmark.y, landmark.z])
        return np.array(pontos, dtype=np.float32)