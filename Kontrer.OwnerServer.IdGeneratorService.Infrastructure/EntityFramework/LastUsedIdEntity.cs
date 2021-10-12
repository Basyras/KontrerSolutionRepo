using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.IdGeneratorService.Infrastructure.EntityFramework
{
    public class LastUsedIdEntity
    {
        [Key]
        public string GroupName { get; set; }

        public int LastUsedId { get; set; }
    }
}