using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.Common.Model
{
    /// <summary>
    /// Interface for a centralized altering of Session Parameters in an Object context.
    /// The caller to  ((System.Data.Objects.ObjectContext)this).RegisterObjectContext(this); (ExtensionMethod of EFContextExtension) must
    /// implement this interface and implement it the following way:
    ///  public void EntityConnection_StateChange(object sender, StateChangeEventArgs e)
    /// {
    ///         this.PerformStateChange(e);
    /// }
    /// </summary>
    public interface IAlteredSession
    {
        void EntityConnection_StateChange(object sender, StateChangeEventArgs e);
    }
}
