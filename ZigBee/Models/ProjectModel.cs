using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigBee.Models
{
    public class ProjectModel
    {
        public string ProjectName { get; set; } = Resources.Resources.ProjectModelPojectNameDefault;

        public List<ZigBeeModel> AvailableZigBees { get; set; } = new List<ZigBeeModel>();

        public ZigBeeModel ZigBeeTemplate { get; set; } = new ZigBeeModel();
    }
}
