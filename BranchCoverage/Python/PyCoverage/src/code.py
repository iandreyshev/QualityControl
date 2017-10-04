def add(x, y):
    return x + y

def divide(x, y):
    if (y == 0):
        raise ValueError('Divide by zero!')
    return x / y