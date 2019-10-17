simpleservice-Error Testing:
https://BNOW-DEV-BOSEAI/OFiSWeb-R6/StreamService.svc


EAI:
added enabledProtocols->works
                <application path="/OFiSWebServices" applicationPool="DefaultAppPool" enabledProtocols="https">
				
Remove bindings other than https:->works
                    <binding protocol="http" bindingInformation="*:80:" />
                    <binding protocol="net.tcp" bindingInformation="808:*" />
                    <binding protocol="net.pipe" bindingInformation="*" />
                    <binding protocol="net.msmq" bindingInformation="localhost" />
                    <binding protocol="msmq.formatname" bindingInformation="localhost" />
                    <binding protocol="https" bindingInformation="*:443:" />
				
				
				Authentication von 

<authentication>

                <anonymousAuthentication enabled="true" userName="IUSR" />

                <basicAuthentication />

                <clientCertificateMappingAuthentication />

                <digestAuthentication />

                <iisClientCertificateMappingAuthentication />

                <windowsAuthentication />

            </authentication>

			
			auf
			<authentication>

                <anonymousAuthentication enabled="true" userName="IUSR" />

                <basicAuthentication enabled="false" />

                <clientCertificateMappingAuthentication enabled="false" />

                <digestAuthentication enabled="false" />

                <iisClientCertificateMappingAuthentication enabled="false">
                </iisClientCertificateMappingAuthentication>

                <windowsAuthentication enabled="false">
                    <providers>
                        <add value="Negotiate" />
                        <add value="NTLM" />
                    </providers>
                </windowsAuthentication>

            </authentication>
			
			-> works
			
			
			
<location path="Default Web Site/OFiSWeb-R6">
        <system.webServer>
            <security>
                <authentication>
                    <anonymousAuthentication userName="" />
                </authentication>
                <access sslFlags="Ssl" />
            </security>
            <serverRuntime uploadReadAheadSize="2147483647" />
        </system.webServer>
    </location>	
->works

<location path="Default Web Site/OFiSWeb-R6">
        <system.webServer>
            <security>
                <authentication>
                    <anonymousAuthentication userName="" />
                </authentication>
                <access sslFlags="Ssl,SslNegotiateCert" />
            </security>
            <serverRuntime uploadReadAheadSize="2147483647" />
        </system.webServer>
    </location>	
	SslNegotiateCert->doenst work in simpleservice

Fazit:			
WICHTIG: Für Simpleservice muss am BOS SSL erforderlich + Clientzertifikate IGNORIEREN angehakt sein!





How to configure self-hosted WCF server endpoint

When working with WCF, you have a choice between hosting the server end of the application in IIS or in your own managed code process. Hosting the server endpoint in your own process is referred to as “self-hosting”. One of the ways to configure this is to wrap host binding code in a Windows service. When this Windows service starts, it kicks off a thread, which sets up host binding and opens the listener. IIS is not needed to host WCF application.

That said, before self-hosted endpoint can actually bind to a port, you need to configure security on the server to allow your process to bind to a port. If you use SSL to provide transport level security, you also need to configure SSL on the server. Here’s how to do this without IIS.
Enable WCF host port binding

    On Windows Server 2008, open up command prompt in elevated mode
    Enter the following command:

netsh http add urlacl url=http://+:888/ user=\Everyone

Replace the port number (888) with the port number that you wish to use for this application. This command will enable “everyone” on this server to bind and listen on port 888, regardless of WCF application URL.

If you do not run this command and use UAC on the server, you will receive an error message that your process does not have permissions to bind to the chosen namespace.
Import server certificate

Next you need to import a server certificate that will be used to provide SSL transport. If such certificate already exists, verify that it is trusted by the server itself, that it is not expired, and that the server has corresponding private key. Verifying that certificate is trusted is especially important if you are using self-signed certificates.

To import a certificate:

    Open MMC, then go to File – Add/Remove snap-in – select and add Certificates snap in – when prompted, select Computer Account and then Local Computer
    Expand Personal certificate store
    Right-click on the Personal certificate store, click All Tasks – Import
    Go through the wizard and make sure to select the following options:
        Make private key exportable = no
        Automatically select store locations for certificates = yes
    Once certificate is imported, double click on it and confirm that it is trusted, that private key is in the store, and that it is not expired

Valid certificate

Next, click on Details tab, and then scroll to the bottom and find Thumbprint. You need to copy this thumbprint value as we will need it in the next step.
Configure WCF port binding to use SSL certificate

So this is where the rubber hits the road. Run the following command to add server certificate to your WCF self-hosted listener:

netsh http add sslcert ipport=0.0.0.0:888 certhash=1dca86867481b22c8f15a134df62af649cc3343a clientcertnegotiation=enable appid={02639d71-0935-35e8-9d1b-9dd1a2a34627}

Note that in this command we specify certificate hash value as a HEX string without spaces. This is our certificate’s thumbprint value.

We also must indicate, which application is allowed to use this SSL binding. This is done via appid parameter. Tricky bit is to actually find this appid for your application. To do this:

    Open your WCF host project in Visual Studio .NET 2010 or similar Microsoft IDE
    Start debug and break your code execution at any point
    Run this statement in Immediate window:

?System.Reflection.Assembly.GetExecutingAssembly.GetType().GUID.ToString

The resulting output will provide the value of the GUID that uniquely identifies your .NET application code. This is the value you need to provide with appid parameter when running netsh command above.

Note that in the netsh command above, we configured this listener to negotiate (accept) client certificates. This is important if you want to implement PKI-based authentication in your WCF application. We will have another post on this shortly.