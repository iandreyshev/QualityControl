import cipher
from nose.tools import assert_raises
from nose.tools import assert_equals

class TestCipher:
    def test_raise(self):
        assert_raises(ValueError, cipher.encrypt(1, 1), 1, 1)
