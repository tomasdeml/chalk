
SourceGear Vault Client API
10 March 2008
--------

As of Vault 4.1/ Fortress 1.1 there is now a SUPPORTED API to code against.  
The library to code against in VaultClientIntegrationLib and you can find 
examples using it in the Development Tips section of the SourceGear support 
forum at: http://support.sourcegear.com/index.php?c=8 at any time.  That is 
also the location for asking question about coding against the API.

History of the VaultClientIntegrationLib.

Before Vault 4.0, the main interface for writing API applications was 
VaultClientOperationsLib, which was overly complex for most users.  To create a 
simpler API, we split the command line client in half.  The half that processed 
command line options stayed, and the half that performed operations was pushed 
into VaultClientIntegrationLib.

SourceGear is currently shipping four products that are built on the 
VaultClientIntegrationLib.  They are:

1. The Command Line Client.
2. The Vault/Fortress NAnt tasks.
3. The Vault/Fortress Ant tasks.
4. The Eclipse IDE client.


Question: "Do you have a way of accessing the Vault Client
API from non-Windows platforms?"

Yes, you may use the version of this library which is compiled into a jar.  You 
can download it and all of the runtime jars that support it in the Java CLC zip 
archive at:  http://www.sourcegear.com/vault/downloads.html

When building against the jar, there are some adjustments that must be made.  
Any member which is public in C# will have a generated get_member() and 
set_member() method.  Otherwise, most of using the library is the same, since 
it was designed to use strings, ints and bools whenever possible.


Question: "What are the files in this folder?"

    vault.exe

	The Vault command-line client.

    VaultClientIntegrationLib.dll 
	The new API library.

    VaultClientOperationsLib.dll 
        
        The old library for the Vault Client API.  If you need more control 
	than VaultClientIntegrationLib provides, you will need to use this 
	library.

    VaultClientNetLib.dll
    VaultLib.dll
    VCDiff.dll

	These are additional libraries needed by OpsLib.

    VaultClientIntegrationLib.xml
	
	This is the XML documentation file for VaultClientIntegrationLib.  
	When you use VaultClientOperationsLib.dll within Visual Studio.NET, 
	the IDE will use this file to provide Intellisense help.

    VaultCmdLineClient.cs

	The source code for the Vault command-line client.  This code
	effectively serves as sample code for how to call 
	VaultClientIntegrationLib.

    Source Code for VaultClientIntegrationLib.
	The source files are provided for your reference.

    VaultClientIntegrationLib.chm

	Documentation for the Vault Client API.

