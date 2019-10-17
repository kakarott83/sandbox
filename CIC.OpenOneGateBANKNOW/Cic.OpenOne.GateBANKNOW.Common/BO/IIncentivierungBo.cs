using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public interface IIncentivierungBo
    {
        /// <summary>
        /// creates the incentive provisions for the given provision context 
        /// </summary>
        /// <param name="ctx"></param>
        void createProvisions(provKontextDto ctx);

         

        /// <summary>
        /// Get data displayed in the My Pocket panel for the seller
        /// </summary>
        /// <param name="sysSeller">seller for whom the MyPocket shall be displayed</param>
        /// <returns>MyPocket data for seller</returns>
        MyPocketDto GetPocket(long sysSeller);
    }
}
