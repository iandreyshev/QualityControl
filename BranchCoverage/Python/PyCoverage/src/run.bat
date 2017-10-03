@echo off
set START_TEST=python -m nose --with-coverage --cover-branches test.py
set CREATE_HTML=python -m coverage html
set CREATE_XML=python -m coverage xml
set OPEN_HTML="htmlcov/index.html"
set OPEN_XML="coverage.xml"

%START_TEST%

%CREATE_HTML%
%CREATE_XML%

%OPEN_HTML%