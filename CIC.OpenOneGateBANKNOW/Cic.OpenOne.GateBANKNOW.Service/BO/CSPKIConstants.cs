using System;

namespace Cic.OpenOne.GateBANKNOW.Service.BO
{
    /**
        A class listing all the PKI framework related contants.

        Credential aliases begin with prefix CREDENTIAL_ALIAS_.

        Policy identifiers begin with prefix POLICY_.

        Trusted CA group aliases begin with prefix TCAL_GROUP_.

        Certificate mapping keys begin with prefix MAP_KEY_.

        Propagation policy identifiers begin with prefix PROPAGATION_.

        Certificate type identifiers begin with prefix CERT_TYPE_. 
      
      
        CERT_TYPE_MACHINE          This constant is used to identify the machine certificate type.
        CERT_TYPE_SPID          This constant is used to identify the spid certificate type.
        CERT_TYPE_SYSTEM          This constant is used to identify the system certificate type.
        CERT_TYPE_WEBSERVER          This constant is used to identify the webserver certificate type.
        CREDENTIAL_ALIAS_MACHINE          This constant points to the default machine (or dmz server) certificate
        CREDENTIAL_ALIAS_SPID          This constant points to the default spid certificate
        CREDENTIAL_ALIAS_SYSTEM          This constant points to the default system certificate
        CREDENTIAL_ALIAS_WEBSERVER          This constant points to the default webserver certificate
        MAP_KEY_APPPID          Mapping for an application PID (an SPID).
        MAP_KEY_COMMONNAME          Mapping for the Common Name
        MAP_KEY_CS_AF_PROPAGATION_ID          Content of the extension with OID 1.3.6.1.4.1.5095.300.3.2.2
        MAP_KEY_CS_BU          Content of the extension with OID 1.3.6.1.4.1.5095.300.3.3.1
        MAP_KEY_CS_COUNTRY          Content of the extension with OID 1.3.6.1.4.1.5095.300.3.3.2
        MAP_KEY_CS_TRUSTLEVEL          Content of the extension with OID 1.3.6.1.4.1.5095.300.3.3.3
        MAP_KEY_CS_TYPE          Content of the extension with OID 1.3.6.1.4.1.5095.300.3.2.1
        MAP_KEY_CSGID          Mapping for user CS Global ID.
        MAP_KEY_DEFAULT_ALIAS          Returns the credential alias for the default certificate of a certificate type.
        MAP_KEY_DNS          Mapping for the DNS name of device certificate
        MAP_KEY_DOMAIN          Mapping for the Domain of a device certificate
        MAP_KEY_FINGERPRINT          Returns the SHA1 fingerprint of the certificate in the following form: 34:FE:...:8C:59
        MAP_KEY_FQDN          Mapping for the FQDN of a device certificate
        MAP_KEY_HOST_ID_MAPPINGS          Returns the host id mappings extension of a certificate.
        MAP_KEY_HOSTNAME          Mapping for the Hostname of a device certificate
        MAP_KEY_PERSONALNAME          Mapping for the unique personal display name, e.g. hans.muster.0 or hans.muler.
        MAP_KEY_PERSONALPID          Mapping for a personal PID (a NON-SPID).
        MAP_KEY_PID          Mapping for a PID (this can be any PID)
        MAP_KEY_SPID_APPLICATION          Mapping for the application or fqdn part of an SPID certificate.
        MAP_KEY_TYPE          Constant type value that is assigned to a certificate.
        MAP_KEY_UPN          Mapping for a User Principal Name (UPN)
        MAP_KEY_USERNAME          Mapping for user CS Global ID.
      
        POLICY_ANYTYPE_MAPPING          This constant is used to map any certificate to its type specific mapping values.
        POLICY_CS_CLIENT_AUTH          This constant is used to validate a client certificate.
        POLICY_SSL_CLIENT_AUTH          This constant is used to validate a client certificate.
        POLICY_SSL_SERVER_AUTH          This constant is used to validate a server certificate.
        PROPAGATION_CS_STANDARD          Returns the default propagation policy alias.
        TCAL_GROUP_AR4ONLY          This group covers only AR4 CAs.
        TCAL_GROUP_CSINTERNAL          This group covers all CS-internal CA certificates.
        TCAL_GROUP_CSIONLY          This group covers the CS-internal CA certificates without AR4 CAs.
     **/
    public class CSPKIConstants
    {
        /// <summary>
        /// CREDENTIAL_ALIAS_SYSTEM
        /// </summary>
        public static String CREDENTIAL_ALIAS_SYSTEM = "SYSTEM";
        /// <summary>
        /// CREDENTIAL_ALIAS_MACHINE
        /// </summary>
        public static String CREDENTIAL_ALIAS_MACHINE = "MACHINE";
        /// <summary>
        /// CREDENTIAL_ALIAS_SPID
        /// </summary>
        public static String CREDENTIAL_ALIAS_SPID = "SPID";
        /// <summary>
        /// CREDENTIAL_ALIAS_WEBSERVER
        /// </summary>
        public static String CREDENTIAL_ALIAS_WEBSERVER = "WEBSERVER";
        /// <summary>
        /// CERT_TYPE_WEBSERVER
        /// </summary>
        public static String CERT_TYPE_WEBSERVER = "webserver";
        /// <summary>
        /// CERT_TYPE_SPID
        /// </summary>
        public static String CERT_TYPE_SPID = "spid";
        /// <summary>
        /// CERT_TYPE_SYSTEM
        /// </summary>
        public static String CERT_TYPE_SYSTEM = "system";
        /// <summary>
        /// CERT_TYPE_MACHINE
        /// </summary>
        public static String CERT_TYPE_MACHINE = "machine";
        /// <summary>
        /// POLICY_ANYTYPE_MAPPING
        /// </summary>
        public static String POLICY_ANYTYPE_MAPPING = "anytype_mapping";
        /// <summary>
        /// POLICY_CS_CLIENT_AUTH
        /// </summary>
        public static String POLICY_CS_CLIENT_AUTH = "cs_client_auth";
        /// <summary>
        /// POLICY_SSL_CLIENT_AUTH
        /// </summary>
        public static String POLICY_SSL_CLIENT_AUTH = "ssl_client_auth";
        /// <summary>
        /// POLICY_SSL_SERVER_AUTH
        /// </summary>
        public static String POLICY_SSL_SERVER_AUTH = "ssl_server_auth";
        /// <summary>
        /// TCAL_GROUP_CSINTERNAL
        /// </summary>
        public static String TCAL_GROUP_CSINTERNAL = "CSINTERNAL";
        /// <summary>
        /// TCAL_GROUP_CSIONLY
        /// </summary>
        public static String TCAL_GROUP_CSIONLY = "CSI";
        /// <summary>
        /// TCAL_GROUP_AR4ONLY
        /// </summary>
        public static String TCAL_GROUP_AR4ONLY = "AR4";
        /// <summary>
        ///  Mapping for user CS Global ID.
        /// </summary>
        public static String MAP_KEY_USERNAME = "csgid";
        /// <summary>
        ///  Mapping for user CS Global ID.
        /// </summary>
        public static String MAP_KEY_CSGID = "csgid";
        /// <summary>
        /// Mapping for the unique personal display name, e.g. hans.muster.0 or hans.muler.
        /// </summary>
        public static String MAP_KEY_PERSONALNAME = "personalName";
        /// <summary>
        ///  Mapping for a PID (this can be any PID)
        /// </summary>
        public static String MAP_KEY_PID = "pid";
        /// <summary>
        /// Mapping for an application PID (an SPID).
        /// </summary>
        public static String MAP_KEY_APPPID = "appPID";
        /// <summary>
        /// Mapping for a personal PID (a NON-SPID).
        /// </summary>
        public static String MAP_KEY_PERSONALPID = "personalPID";
        /// <summary>
        ///  Mapping for a User Principal Name (UPN)
        /// </summary>
        public static String MAP_KEY_UPN = "upn";
        /// <summary>
        ///  Mapping for the Common Name
        /// </summary>
        public static String MAP_KEY_COMMONNAME = "cn";
        /// <summary>
        ///  Mapping for the Hostname of a device certificate
        /// </summary>
        public static String MAP_KEY_HOSTNAME = "hostname";
        /// <summary>
        /// Mapping for the FQDN of a device certificate
        /// </summary>
        public static String MAP_KEY_FQDN = "fqdn";
        /// <summary>
        ///  Mapping for the DNS name of device certificate
        /// </summary>
        public static String MAP_KEY_DNS = "dns";
        /// <summary>
        ///  Mapping for the Domain of a device certificate
        /// </summary>
        public static String MAP_KEY_DOMAIN = "domain";
        /// <summary>
        /// Mapping for the application or fqdn part of an SPID certificate.
        /// </summary>
        public static String MAP_KEY_SPID_APPLICATION = "application";
        /// <summary>
        /// Returns the host id mappings extension of a certificate.
        /// </summary>
        public static String MAP_KEY_HOST_ID_MAPPINGS = "host_id_mappings";
        /// <summary>
        /// Content of the extension with OID 1.3.6.1.4.1.5095.300.3.2.1
        /// </summary>
        public static String MAP_KEY_CS_TYPE = "cs_type";
        /// <summary>
        ///  Content of the extension with OID 1.3.6.1.4.1.5095.300.3.2.2
        /// </summary>
        public static String MAP_KEY_CS_AF_PROPAGATION_ID = "cs_af_propagation_id";
        /// <summary>
        /// Content of the extension with OID 1.3.6.1.4.1.5095.300.3.3.1
        /// </summary>
        public static String MAP_KEY_CS_BU = "cs_bu";
        /// <summary>
        ///  Content of the extension with OID 1.3.6.1.4.1.5095.300.3.3.2
        /// </summary>
        public static String MAP_KEY_CS_COUNTRY = "cs_country";
        /// <summary>
        /// Content of the extension with OID 1.3.6.1.4.1.5095.300.3.3.3
        /// </summary>
        public static String MAP_KEY_CS_TRUSTLEVEL = "cs_trustlevel";
        /// <summary>
        /// Constant type value that is assigned to a certificate.
        /// </summary>
        public static String MAP_KEY_TYPE = "type";
        /// <summary>
        /// Returns the credential alias for the default certificate of a certificate type.
        /// </summary>
        public static String MAP_KEY_DEFAULT_ALIAS = "defaultAlias";
        /// <summary>
        /// Returns the SHA1 fingerprint of the certificate in the following form: 34:FE:...:8C:59
        /// </summary>
        public static String MAP_KEY_FINGERPRINT = "fingerprint";
        /// <summary>
        /// Returns the default propagation policy alias.
        /// </summary>
        public static String PROPAGATION_CS_STANDARD = "cs_standard";
    }
}