using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.SimpleWorkflowEngine.Models
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public static Person FromCsv(string row)
        {
            string[] values = row.Split(',');
            Person person = new Person();
            person.Name = values[0];
            person.Age = int.Parse(values[1]);
            person.Gender = values[2];

            return person;
        }
    }
}
