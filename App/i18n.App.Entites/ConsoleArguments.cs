using CommandLine;

namespace i18n.App.Entites
{
    public class ConsoleArguments
    {
        [Option('p', "path", Required = true, HelpText = "Set path to the HTML file")]
        public string HTMLPath { get; set; }

        [Option('o', "output", Required = false, HelpText = "Set the output path for the utility. If not configured, will default to creating an output folder in the same folder as the utility")]
        public string OutputName { get; set; }

        [Option('s', "source", Required = false, HelpText = "Set the source language of the HTML file. If not configured, will default to en-us")]
        public string SourceLanguage { get; set; }

        [Option('t', "target", Required = true, HelpText = "Set the target language for translating the HTML file")]
        public string TargetLanguage { get; set; }
    }
}
