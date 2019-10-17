using System.DirectoryServices;

namespace Cic.One.Web.DAO.Mail
{
    public class ActiveDirectoryConnection
    {
        public DirectoryEntry GetLdapDirectoryEntry(string path)
        {
            return GetDirectoryEntry(path, "LDAP");
        }

        public DirectoryEntry GetGCDirectoryEntry(string path)
        {
            return GetDirectoryEntry(path, "GC");
        }

        private DirectoryEntry GetDirectoryEntry(string path, string protocol)
        {
            var ldapPath = string.IsNullOrEmpty(path) ? string.Format("{0}:", protocol) : string.Format("{0}://{1}", protocol, path);
            return new DirectoryEntry(ldapPath);
        }
    }
}