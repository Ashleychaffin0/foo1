set fooImporter=C:\LRS\$LeadsLightning\LLWS_Importer\LLWS_Importer\LLWS_Importer
set fooTestLLImporter=C:\LRS\C#\TestLLImporter\TestLLImporter
wsdl /v http://198.64.249.219/WsWingate/WsCcLeads.asmx /out:%fooImporter%
xcopy /Y %fooImporter%\WsCcLeads.cs %fooTestLLImporter%
wsdl /v http://198.64.249.219/LeadsLightningWS/LLWS1.asmx /out:%fooTestLLImporter%
set fooImporter=
set fooTestLLImporter=
@rem sdl /v http://198.64.249.6/LeadsLightningWS/LLWS1.asmx
@rem wsdl /v http://www.leadslightning.com/leadslightningWS/LLWS1.asmx