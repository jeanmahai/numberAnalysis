@echo off

cd ..

set currentPath=%cd%\SohoWeb.AngularFramework\www

echo current path : %currentPath%

set excludePath=%cd%\SohoWeb.AngularFramework\exclude.txt

echo exclude path : %excludePath%

set targetpath=%cd%\SohoWeb.WebMgt

echo target path : %targetpath%

::xcopy %currentPath%\controllers\*.* %targetpath%\ScriptController\*.* /s/y/exclude:%excludePath%

::if not exist %targetpath%\bower_components\ (xcopy %currentPath%\bower_components\*.* %targetpath%\bower_components\ /s/y)

::if not exist %targetpath%\res\main.js (xcopy %currentPath%\main\main.js %targetpath%\res\ /s/y)

xcopy %currentPath%\main\app.js %targetpath%\res\ /s/y

::xcopy %currentPath%\main\main.css %targetpath%\res\ /s/y

pause
