using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticKernelDemo
{

    
    public class ArchivePlugin
    {
        [KernelFunction("save_data")]
        [Description("save data in your computer.")]
        public async Task writeData(Kernel kernel,string fileName,string data) { 
            await File.WriteAllTextAsync($@"C:\app\OpenAIChatApp\SemanticKernelDemo\{fileName}.txt", data);
        
        }
    }
}
