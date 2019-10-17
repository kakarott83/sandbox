using System;
using Cic.One.DTO;
using System.Collections.Generic;
namespace Cic.One.Web.DAO
{
    public interface IRecurrDao
    {
        List<ApptmtDto> searchApptmts(iSearchApptmtsWithRecurrDto search, long sysPerole);
    }
}
