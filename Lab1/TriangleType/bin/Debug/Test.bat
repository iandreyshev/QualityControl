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

echo: Тест: Некорректный формат параметров
%PROGRAM% 3 4РУС 5
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 3! 4 5
if %ERRORLEVEL% EQU 0 goto err
%PROGRAM% 3 4 5kkk
if %ERRORLEVEL% EQU 0 goto err

echo: Тест: Сторона с отрицательной длинной
%PROGRAM% 3 -4 5
if %ERRORLEVEL% EQU 0 goto err

echo: Тест: Длина стороны больше, чем это возможно
%PROGRAM% 999999999999999999999999999999999999999999999999999 999999999999999999999999999999999999999999999999999 1
if %ERRORLEVEL% EQU 0 goto err

echo: Тест: Пустые параметры
%PROGRAM% "" "" ""
if %ERRORLEVEL% EQU 0 goto err

echo: Тест: Из сторон нельзя построить треугольник
%PROGRAM% 1 1 10 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\NotTriangle.txt
if ERRORLEVEL 1 goto err
%PROGRAM% 0.1 99 0.1 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\NotTriangle.txt
if ERRORLEVEL 1 goto err

echo: Тест: Обычный треугольник
%PROGRAM% 3 4 5 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\Common.txt
if ERRORLEVEL 1 goto err
%PROGRAM% 0.3 0.4 0.5 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\Common.txt
if ERRORLEVEL 1 goto err

echo: Тест: Равнобедренный треугольник
%PROGRAM% 5 5 1 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\Isosceles.txt
if ERRORLEVEL 1 goto err
%PROGRAM% 0.45 0.45 0.1 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\Isosceles.txt
if ERRORLEVEL 1 goto err

echo: Тест: Равносторонний треугольник
%PROGRAM% 1 1 1 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\Equilateral.txt
if ERRORLEVEL 1 goto err
%PROGRAM% 0.99 0.99 0.99 > %TEMP%\out.txt
fc %TEMP%\out.txt reference\Equilateral.txt
if ERRORLEVEL 1 goto err

echo Program testing succeeded
exit 0

:err
echo Program testing failed
exit 1
