using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTask.Shared.ResultDtos
{
    public class LiteModelDto<U>
    {
        public U Id { get; set; }

        public string Name { get; set; }
    }
}
