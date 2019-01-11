set fooImporter=C:\LRS\$LeadsLightning\LLWS_Importer\LLWS_Importer\LLWS_Importer
set fooTestLLImporter=C:\LRS\C#\TestLLImporter\TestLLImporter
wsdl /v http://67.199.7.2/WsWingate/WsCcLeads.asmx /out:%fooImporter%
xcopy /Y %fooImporter%\WsCcLeads.cs %fooTestLLImporter%
wsdl /v http://67.199.7.2/LeadsLightningWS/LLWS1.asmx /out:%fooTestLLImporter%
set fooImporter=
set fooTestLLImporter=
@
@
@rem wsdl /v http://67.199.7.2/LeadsLightningWS_WsCallPlugin/WsCallPlugin.asmx
@rem wsdl /v http://localhost/WebServices/LLWS1.asmx
@rem wsdl /v http://63.134.235.22/LeadsLightningWS/LLWS1.asmx
@rem -- Don't use: @rem wsdl /v http://63.134.235.22/LLWSTest/LLWS1.asmx
@rem wsdl /v http://www.leadslightning.com/LLWSTest/LLWS1.asmx
@rem wsdl /v http://www.leadslightning.com/leadslightningWS/LLWS1.asmx