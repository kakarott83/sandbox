using System;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace Cic.OpenOne.Common.Util.Behaviour
{
    /// <summary>
    /// ValidateMustUnderstandElement
    /// </summary>
    public class ValidateMustUnderstandElement : BehaviorExtensionElement
    {
        /// <summary>
        /// BehaviorType
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(MustUnderstandBehavior); }
        }

        /// <summary>
        /// CreateBehavior
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return new MustUnderstandBehavior(false);
        }
    }
}