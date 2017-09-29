package ru.iandreyshev.cipher;

public class CVigenere {
    public static void encrypt(String message, String mask, EncryptionType workType) {
        if (!isMessageValid(message)) {
            throw new IllegalArgumentException("Invalid message string");
        } else if (!isMaskValid(mask)) {
            throw new IllegalArgumentException("Invalid mask string");
        }

        enterEncrypt(message, mask);
    }

    private CVigenere() {}

    private static boolean isMessageValid(String message) {
        return true;
    }
    private static boolean isMaskValid(String mask) {
        return true;
    }
    private static void  enterEncrypt(String message, String mask) {

    }
}
