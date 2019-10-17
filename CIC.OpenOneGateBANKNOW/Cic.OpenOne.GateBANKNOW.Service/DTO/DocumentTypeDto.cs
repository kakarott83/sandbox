using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Describes a DocumentType
    /// </summary>
    public class DocumentTypeDto
    {
        public long syswftx { get; set; }
        public String wftxcode { get; set; }
        public String name { get; set; }
        public String extension { get; set; }

        public long sysbdefgrp { get; set; }
        public String groupname { get; set; }


    }
}