using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Undefinierter Platzhalte für Messageobjekt
    /// </summary>
    public class Message
    {

        /// <summary>
        /// Compare to equal Operator
        /// </summary>
        /// <param name="left">left bject</param>
        /// <param name="right">right object</param>
        /// <returns>true = Equal values</returns>
        public static bool operator ==(Message left, Message right)
        {
            if ((object)left == null && null == (object)right)
            {
                return true;
            }

            if ((object)left != null && (object)right != null)
            {
                // Can't be different at the moment
                return true;
            }

            return false;
        }

        /// <summary>
        /// Compare to not equal Operator
        /// </summary>
        /// <param name="left">left object</param>
        /// <param name="right">right object</param>
        /// <returns>true = not equal values</returns>
        public static bool operator !=(Message left, Message right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Equals operation
        /// </summary>
        /// <param name="obj">Object to compare current obejct to</param>
        /// <returns>true = equal objects</returns>
        public override bool Equals(object obj)
        {
            if (obj != null && (this.GetType() == obj.GetType()))
            {
                Message message = (Message)obj;
                return (this == message);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get Hashcode
        /// </summary>
        /// <returns>Hashcode</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
