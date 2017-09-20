import sympy
x = sympy.Symbol("x")
y = sympy.Symbol("y")
z = sympy.Symbol("z")
sympy.limit(sympy.sin(x)/x, x, 0)
m = sympy.Matrix([[1, x, x**2], [1, y, y**2], [1, z, z**2]])

a = sympy.Symbol("a")
b = sympy.Symbol("b")
c = sympy.Symbol("c")
d = sympy.Symbol("d")
e = sympy.Symbol("e")

m3 = sympy.Matrix([[a, b, c], [c, a, b], [b, c, a]])

m4 = sympy.Matrix([[a, b, c, d], [d, a, b, c], [c, d, a, b], [b, c, d, a]])

m5 = sympy.Matrix([[a, b, c, d, e], [e, a, b, c, d], [d, e, a, b, c], [c, d, e, a, b], [b, c, d, e, a]])