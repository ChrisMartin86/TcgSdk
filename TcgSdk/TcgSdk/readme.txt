- This assembly uses the Json.NET library found at http://www.newtonsoft.com/json version 9.0.1. You'll need this library for this SDK to work properly.

- To set up event log -

Create a registry key

HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\eventlog\Application\TcgSdk
Create a string value inside this

Name it EventMessageFile, set its value to

    C:\Windows\Microsoft.NET\Framework\v2.0.50727\EventLogMessages.dll


- Event ID Codes -

0 - Unknown Error
1 - Http Response Error
2 - Deserialization Error
3 - Not Implemented Error