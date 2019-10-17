SOAP-Tests:
-----------
Global.asax.cs muss einen Soap-Recording-Listener registrieren
Cic.OpenOne.Common.Util.Extension.SoapLoggingExtension.addSoapMessageHandler(new SoapTestMessageHandler());
dies ist momentan immer der Fall.

Es werden dann alle folgenden Soap-Requests in einen Ordner geloggt, wenn ein Ordner
 "\\..\\..\\UnitTest\\resources"
 oder
 "\\..\\UnitTest\\resources"
 vorhanden ist.

 Die Soap-Calls werden mit Nummer und Name versehen, die Zieladresse wird mit DESTINATION_SERVER ersetzt.

 Für automatisches Testing müssen diese Dateien im UnitTest/resources-Ordner eingecheckt werden. Alternativ könnte man das Projekt auf einen Testrechner
 kopieren und dort laufen lassen. (mstest UnitTest\bin\Release\UnitTest.dll)

 Jenkins ruft momentan den im SoapTestRunner eingestellten Host http://se-dnexec/OpenLeaseGateONE/" mit den in resources eingecheckten SOAPS
 auf und jeder fehlerhafte Aufruf ist in Jenkins dann mit Fehlermeldung ersichtlich.
 
 Der gesamte Fehleroutput ist in
 \\se-comp\jobs\CIC.OpenLeaseGateCRM\workspace\UnitTest\resources
 <orgdateiname>.err 
 ersichtlich.
 