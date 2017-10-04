import code
from nose.tools import assert_raises
from nose.tools import assert_equals

class TestCipher:
    def test_add(self):
        assert_equals(code.add(5, 0), 5)

    def test_divide(self):
        assert_raises(ValueError, code.divide, 5, 0)
        assert_equals(code.divide(5, 1), 5)