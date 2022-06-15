using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Digi.Models
{
    public class CSharpDigiTestPayload
    {
        public int Id {get;private set;}
        public int Length { get; private set; }
        public CSharpDigiTestPayload(int id, int length)
        {
            this.Id = id;
            this.Length = length;
        }

        public string GetOutput()
        {
            var data = $"C#TD:{this.Id}";
            var charsToAdd = this.Length - data.Length;
            if (charsToAdd > 0)
            {
                for (int j = 0; j < charsToAdd; j++)
                    data += 'x';
            }
            return data;
        }

        public static CSharpDigiTestPayload FromString(string s)
        {
            if(s.Contains("C#TD:") && s.StartsWith("C#TD:"))
            {
                var x = s.TrimEnd('x');
                return new CSharpDigiTestPayload(int.Parse(x.Split(':')[1]), x.Length);
            }
            return null;
        }
    }
}
