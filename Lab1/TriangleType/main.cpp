#include <iostream>
#include <exception>
#include <string>

namespace
{
	const int ARGUMENTS_COUNT = 4;

	enum TriangleType
	{
		NONE,
		COMMON,
		ISOSCELES,
		EQUILATERAL,
	};
	struct TriangleSides
	{
		float a;
		float b;
		float c;
	};
}

TriangleSides ConvertArgs(int argc, char* argv[]);
TriangleType CalculateTriangleType(TriangleSides sides);
void Print(TriangleType type);

int main(int argc, char* argv[])
{
	try
	{
		const TriangleSides& sides = ConvertArgs(argc, argv);
		const TriangleType& type = CalculateTriangleType(sides);
		Print(type);
	}
	catch (std::exception const& e)
	{
		std::cout << e.what() << std::endl;
		return 1;
	}

	return 0;
}

TriangleSides ConvertArgs(int argc, char* argv[])
{
	if (argc < ARGUMENTS_COUNT)
	{
		throw std::invalid_argument("Invalid arguments count. Use: TriangleType.exe <a> <b> <c>.");
	}

	const float& a = std::stof(argv[1]);
	const float& b = std::stof(argv[2]);
	const float& c = std::stof(argv[3]);

	return { a, b, c };
}

TriangleType CalculateTriangleType(TriangleSides sides)
{
	if (sides.b + sides.c < sides.a ||
		sides.a + sides.c < sides.b ||
		sides.a + sides.b < sides.c)
	{
		return TriangleType::NONE;
	}

	if (sides.a == sides.b && sides.b == sides.c)
	{
		return TriangleType::EQUILATERAL;
	}

	if (sides.a == sides.b ||
		sides.a == sides.c ||
		sides.b == sides.c)
	{
		return TriangleType::ISOSCELES;
	}

	return TriangleType::COMMON;
}

void Print(TriangleType type)
{
	switch (type)
	{
	case TriangleType::NONE:
		std::cout << "It is impossible to compose a triangle." << std::endl;
		break;

	case TriangleType::COMMON:
		std::cout << "Triangle is common." << std::endl;
		break;

	case TriangleType::ISOSCELES:
		std::cout << "Triangle is isosceles." << std::endl;
		break;

	case TriangleType::EQUILATERAL:
		std::cout << "Triangle is equilateral" << std::endl;
		break;

	default:
		break;
	}
}