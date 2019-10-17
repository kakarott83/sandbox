using System;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Base object of all service response objects
    /// </summary>
    public class oBaseDto
    {
        [NonSerialized]
        private double start;

        /// <summary>
        /// oBaseDto-Konstruktor
        /// </summary>
        public oBaseDto()
        {
            start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            message = new Message();
        }

        /// <summary>
        /// success
        /// </summary>
        public void success()
        {
            message.code = "0";
            message.detail = "";
            message.message = "";
            message.type = MessageType.None;
            message.duration = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds - start);
        }

        /// <summary>
        /// common Messageobject for returning service status information
        /// </summary>
        public Message message
        {
            get;
            set;
        }
    }
}