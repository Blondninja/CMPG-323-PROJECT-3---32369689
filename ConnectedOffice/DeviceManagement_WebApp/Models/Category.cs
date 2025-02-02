﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DeviceManagement_WebApp.Models
{
    public partial class Category
    {
        public Category()
        {
            Device = new HashSet<Device>();
        }
        [Required]

        [DisplayName("Category ID")]
        public Guid CategoryId { get; set; }
        [Required]
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }
        [Required]
        [DisplayName("Category Description")]
        public string CategoryDescription { get; set; }
        [DisplayName("Category Date Created")]
        public DateTime DateCreated { get; set; }

        [DisplayName("Device")]
        public virtual ICollection<Device> Device { get; set; }
    }
}
