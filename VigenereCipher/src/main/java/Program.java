import ru.iandreyshev.cipher.*;

class Program {
    public static void main(String[] args) {
        try
        {
            EncryptionType type = EncryptionType.ENCODE;
            CVigenere.encrypt("message", "mask", type);
        } catch (Exception e) {
            System.out.println(e.getMessage());
            System.exit(EXIT_FAILURE);
        }
        System.exit(EXIT_SUCCESS);
    }

    private static final int EXIT_SUCCESS = 0;
    private static final int EXIT_FAILURE = 1;
}
