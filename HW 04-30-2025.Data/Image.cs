﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_04_30_2025.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public DateTime UploadedDate { get; set; }
        public int Likes { get; set; }
    }
}
