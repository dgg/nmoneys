ECHO OFF
rmdir ..\NUnit.Runners.lite

mklink /D ..\NUnit.Runners.lite %CD%
ECHO ON