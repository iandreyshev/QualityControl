def isString(object):
    return isinstance(object, str)

def isInteger(object):
    return isinstance(object, int)

def validateArgs(str, key):
    if not isString(str):
        ValueError('First argument not string')
    elif not isInteger(key):
        ValueError('Second argument not number')

def encrypt(str, key):
    validateArgs(str, key)