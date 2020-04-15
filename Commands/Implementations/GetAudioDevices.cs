using obs_cli.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace obs_cli.Commands.Implementations
{
    public class GetAudioDevices : BaseCommand
    {
        public override string Name => AvailableCommand.GetAudioDevices.GetDescription();

        public GetAudioDevices(IDictionary<string, string> arguments)
        {

        }

        public override void Execute()
        {
            Console.WriteLine("get-audio-devices audio devices blah blah blah");
        }
    }
}
