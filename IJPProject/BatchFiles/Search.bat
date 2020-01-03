@ECHO ON 
cd ..
set Relativepath=%CD%
set dllPath=bin\Debug\IJPProject.dll
set FullPath=%Relativepath%\%dllPath%
ECHO %FullPath%
if not exist "XmlReports" mkdir "XmlReports"
nunit3-console /result:%Relativepath%\XmlReports\Search.xml %FullPath% --where:cat==Search
