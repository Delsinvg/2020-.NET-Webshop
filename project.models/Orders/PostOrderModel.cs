﻿using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.Orders
{
    public class PostOrderModel : BaseOrderModel
    {
        public Guid UserId { get; set; }
        public ICollection<Guid> Products { get; set; }
    }
}
