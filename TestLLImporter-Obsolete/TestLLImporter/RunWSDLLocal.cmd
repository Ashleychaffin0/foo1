set fooImporter=C:\LRS\$LeadsLightning\LLWS_Importer\LLWS_Importer\LLWS_Importer
set fooTestLLImporter=C:\LRS\C#\TestLLImporter\TestLLImporter

wsdl /v http://localhost/WebServices/WsCcLeads.asmx /out:%fooImporter%
xcopy /Y %fooImporter%\WsCcLeads.cs %fooTestLLImporter%
wsdl /v http://localhost/WebServices/LLWS1.asmx /out:%fooTestLLImporter%


@rem wsdl /v http://localhost/WebServices/WsCcLeads.asmx /out:C:\LRS\$LeadsLightning\LLWS_Importer\LLWS_Importer\LLWS_Importer
@rem wsdl /v http://localhost/WebServices/LLWS1.asmx /out:C:\LRS\C#\TestLLImporter\TestLLImporter

set fooImporter=
set fooTestLLImporter=
@
@
@rem wsdl /v http://www.leadslightning.com/LLWSTest/LLWS1.asmx
@rem wsdl /v http://www.leadslightning.com/leadslightningWS/LLWS1.asmx