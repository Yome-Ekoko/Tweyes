using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweyesBackend.Domain.Settings
{
    public class AzureStorageOptions
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string StorageAccountName { get; set; }
    }
}
