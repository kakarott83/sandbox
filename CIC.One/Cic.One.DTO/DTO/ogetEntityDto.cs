﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetEntityDto<T>:oBaseDto 
    {
        public T entity;
    }
}
