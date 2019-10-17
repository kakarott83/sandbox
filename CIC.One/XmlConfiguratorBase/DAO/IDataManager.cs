using Cic.One.DTO;
using System;

namespace XmlConfiguratorBase.DAO
{
    public interface IDataManager : IDisposable
    {
        /// <summary>
        /// Read the configuration objects
        /// </summary>
        /// <returns>Mask configurations</returns>
        WfvConfig ReadData();
        
        /// <summary>
        /// Save configuration data
        /// </summary>
        /// <param name="data">Configuration data</param>
        void SaveData(WfvConfig data);
    }
}
