﻿using System;

namespace Wanted.FoodManager.Stock.Domain
{
    public class Entity
    {
        public string Id { get; private set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
