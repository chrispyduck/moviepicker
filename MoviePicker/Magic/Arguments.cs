using Ookii.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePicker.Magic
{
    public class Arguments
    {
        [CommandLineArgument(Position = 0)]
        public string Directory { get; set; }

        [CommandLineArgument]
        public bool DisplayExceptions { get; set; }

        [CommandLineArgument]
        public int Rows { get; set; } = 2;

        [CommandLineArgument]
        public int Columns { get; set; } = 4;

        [CommandLineArgument("?")]
        public bool DisplayUsage { get; set; }
    }
}
