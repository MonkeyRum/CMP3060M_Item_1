using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Set
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> goodFilepaths = new List<string>();
            List<string> badFilepaths = new List<string>();
            string manualClassificationFolder = args[0];
            string outputFolder = args[1];
            string suffix = args[2];
            string imageType = args[3];

            string[] filePaths = Directory.GetFiles(manualClassificationFolder);

            foreach (var file in filePaths)
            {
                var reader = new StreamReader(File.OpenRead(file));
                bool isBad = false;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line.Contains("1"))
                    {
                        isBad = true;
                        break;
                    }
                }

                var list = isBad ? badFilepaths : goodFilepaths;
                list.Add(file);
            }

            string trainingCsv = string.Empty;
            string testingCsv = string.Empty;

            SplitIntoSets(badFilepaths, ref trainingCsv, ref testingCsv, suffix, imageType);
            SplitIntoSets(goodFilepaths, ref trainingCsv, ref testingCsv, suffix, imageType);

            // remove trailing comma
            trainingCsv = trainingCsv.Substring(0, trainingCsv.Length - 1);
            testingCsv = testingCsv.Substring(0, trainingCsv.Length - 1);

            WriteCsv(outputFolder + @"\training.csv", ref trainingCsv);
            WriteCsv(outputFolder + @"\testing.csv", ref testingCsv);
        }

        private static void SplitIntoSets(List<string> paths, ref string trainingCsv, ref string testingCsv, string suffix, string imageType)
        {
            int mid = paths.Count / 2;

            for (int i = 0; i < paths.Count; ++i)
            {
                string csvEntry = Path.GetFileName(paths[i]);
                csvEntry = csvEntry.Substring(0, csvEntry.Length - suffix.Length);
                csvEntry += "." + imageType + ",";

                if (i < mid)
                {
                    trainingCsv += csvEntry;
                }
                else
                {
                    testingCsv += csvEntry;
                }
            }
        }

        private static void WriteCsv(string outputPath, ref string contents)
        {
            File.WriteAllText(outputPath, contents);
        }
    }
}
