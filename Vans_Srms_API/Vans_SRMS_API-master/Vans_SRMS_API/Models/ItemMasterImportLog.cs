using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vans_SRMS_API.Models
{
    public class ItemMasterImportLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemMasterImportLogId { get; set; }
        [Required]
        public DateTime FileLastModifiedDate { get; set; }
        [Required]
        public DateTime StartedAt { get; set; }

        public DateTime FinishedAt { get; set; }
        public int ProductsImported { get; set; }
        public int ProductsFailedToImport { get; set; }
        public int RecordsInFile { get; set; }
    }
}
