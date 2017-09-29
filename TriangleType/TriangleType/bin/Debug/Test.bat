@echo off
chcp 1251
set PROGRAM="TriangleType.exe"

echo: Тест: Запуск без полного набора параметров (0, 1, 2 параметра)
%PROGRAM%
if %ERRORLEVEL% EQU 0 goto err           
%PROGRAM% 1
if %ERRORLEVEL% EQU 0 goto err           
%PROGRAM% 1 2
if %ERRORLEVEL% EQU 0 goto err

echo: Тест: Нечисловой формат параметров
%PROGRAM% 3!! 4 5
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 3 4!! 5
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 3 4 5!!
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 3!! 4!! 5
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 3 4!! 5!!
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 3!! 4 5!!
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 3!! 4!! 5!!
if %ERRORLEVEL% EQU 0 goto err

echo: Тест: Сторона с отрицательной длинной
%PROGRAM% -3 4 5
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 3 -4 5
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 3 4 -5
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% -3 -4 5
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 3 -4 -5
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% -3 4 -5
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% -3 -4 -5
if %ERRORLEVEL% EQU 0 goto err

echo: Тест: Длина стороны больше, чем это возможно для типа данных
set OVERFLOW_NUM="999999999999999999999999999999999999999"
%PROGRAM% %OVERFLOW_NUM% 1 1
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 1 %OVERFLOW_NUM% 1
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 1 1 %OVERFLOW_NUM%
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% %OVERFLOW_NUM% %OVERFLOW_NUM% 1
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 1 %OVERFLOW_NUM% %OVERFLOW_NUM%
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% %OVERFLOW_NUM% 1 %OVERFLOW_NUM%
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% %OVERFLOW_NUM% %OVERFLOW_NUM% %OVERFLOW_NUM%
if %ERRORLEVEL% EQU 0 goto err

echo: Тест: Длинна стороны меньше, чем это возможно для типа данных
%PROGRAM% -%OVERFLOW_NUM% 1 1
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 1 -%OVERFLOW_NUM% 1
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 1 1 -%OVERFLOW_NUM%
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% -%OVERFLOW_NUM% -%OVERFLOW_NUM% 1
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% -%OVERFLOW_NUM% 1 -%OVERFLOW_NUM%
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 1 -%OVERFLOW_NUM% -%OVERFLOW_NUM%
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% -%OVERFLOW_NUM% -%OVERFLOW_NUM% -%OVERFLOW_NUM%
if %ERRORLEVEL% EQU 0 goto err

echo: Тест: Пустые параметры
%PROGRAM% "" 1 1
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 1 "" 1
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 1 1 ""
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% "" "" 1
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 1 "" ""
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% "" 1 ""
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% "" "" ""
if %ERRORLEVEL% EQU 0 goto err

echo: Тест: Из сторон нельзя построить треугольник
echo: Стороны: 1 1 10
%PROGRAM% 1 1 10 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\NotTriangle.txt
if ERRORLEVEL 1 goto err
echo: Стороны: 0.1 99 0.1
%PROGRAM% 0.1 99 0.1 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\NotTriangle.txt
if ERRORLEVEL 1 goto err

echo: Тест: Обычный треугольник
echo: Стороны: 3 4 5
%PROGRAM% 3 4 5 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\Common.txt
if ERRORLEVEL 1 goto err
echo: Стороны: 0.3 0.4 0.5
%PROGRAM% 0.3 0.4 0.5 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\Common.txt
if ERRORLEVEL 1 goto err

echo: Тест: Равнобедренный треугольник                                 
echo: Стороны: 5 5 1
%PROGRAM% 5 5 1 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\Isosceles.txt
if ERRORLEVEL 1 goto err
echo: Стороны: 0.45 0.45 0.1
%PROGRAM% 0.45 0.45 0.1 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\Isosceles.txt
if ERRORLEVEL 1 goto err

echo: Тест: Равносторонний треугольник
echo: Стороны: 1 1 1
%PROGRAM% 1 1 1 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\Equilateral.txt
if ERRORLEVEL 1 goto err
echo: Стороны: 0.99 0.99 0.99
%PROGRAM% 0.99 0.99 0.99 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\Equilateral.txt
if ERRORLEVEL 1 goto err

echo: Тест: Треугольник с нулевыми сторонами
echo: Стороны: 0 1 1
%PROGRAM% 0 1 1
if %ERRORLEVEL% EQU 0 goto err
echo: Стороны: 1 0 1
%PROGRAM% 1 0 1
if %ERRORLEVEL% EQU 0 goto err
echo: Стороны: 1 1 0
%PROGRAM% 1 1 0
if %ERRORLEVEL% EQU 0 goto err
echo: Стороны: 1 0 0
%PROGRAM% 1 0 0
if %ERRORLEVEL% EQU 0 goto err
echo: Стороны: 0 1 0
%PROGRAM% 0 1 0
if %ERRORLEVEL% EQU 0 goto err
echo: Стороны: 0 0 1
%PROGRAM% 0 0 1
if %ERRORLEVEL% EQU 0 goto err
echo: Стороны: 0 0 0
%PROGRAM% 0 0 0
if %ERRORLEVEL% EQU 0 goto err

echo Program testing succeeded
exit 0

:err
echo Program testing failed
exit 1
                    