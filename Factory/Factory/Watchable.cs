﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    public interface Watchable
    {
        string Name { get; set; }
        string Description { get; set; }
        //For the image
        string ImageUrl { get; set; }
        void watchable(string name, string description, string imageUrl);
    }
}
