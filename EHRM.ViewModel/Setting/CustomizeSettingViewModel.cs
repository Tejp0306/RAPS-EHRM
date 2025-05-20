using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EHRM.ViewModel.Setting
{
    public class CustomizeSettingViewModel
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; }
        public string Bio { get; set; }

        public IFormFile LogoFile { get; set; }
        public string ExistingLogoPath { get; set; }

        public IFormFile FaviconFile { get; set; }
        public string ExistingFaviconPath { get; set; }
    }
}

