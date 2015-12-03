@cd %~dp0
del*.config
del*.dll
del*.pdb
del*.xml
copy ..\src\ExactOnline.Client.Sdk\bin\debug\exact*.* .
copy ..\src\ExactOnline.Client.OAuth\bin\debug\exact*.* .
