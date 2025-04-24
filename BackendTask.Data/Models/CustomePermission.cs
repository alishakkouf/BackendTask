using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTask.Data.Models
{
    internal class CustomePermission
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string Permission { get; set; }

        [StringLength(500)]
        public string Action { get; set; }

        [StringLength(500)]
        public string Subject { get; set; }

        [StringLength(500)]
        public string PermissionLabel { get; set; }

    }
}
