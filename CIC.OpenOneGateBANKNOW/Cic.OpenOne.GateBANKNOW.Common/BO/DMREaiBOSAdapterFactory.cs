namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    using System;

    using Auskunft.CrifHelper;
    using DAO;

    using OpenOne.Common.DTO;

    public class DMREaiBOSAdapterFactory : IEaiBOSAdapterFactory
    {
        public IEaiBOSAdapter getEaiBOSAdapter(string method)
        {
            //make it case-insensitive
            String imethod = method.ToUpper();
            switch (imethod)
            {
                case ("CREATEORUPDATEDMR"):
                    return new CreateOrUpdateDMRAdapter();
            }
            return null;
        }

        private class CreateOrUpdateDMRAdapter : IEaiBOSAdapter
        {
            public void processEaiHot(IEaihotDao dao, EaihotDto eai)
            {
                var bo = BOFactory.getInstance().createDMRBo();
                bo.createOrUpdateDMR(eai.SYSOLTABLE.Value);
            }
        }
    }
}