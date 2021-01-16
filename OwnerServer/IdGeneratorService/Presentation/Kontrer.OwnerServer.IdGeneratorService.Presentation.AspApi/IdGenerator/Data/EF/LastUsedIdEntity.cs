using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator.Data.EF
{
    public class LastUsedIdEntity
    {
        [Key]
        public string GroupName { get; set; }
        public int LastUsedId { get; set; }
    }
}
