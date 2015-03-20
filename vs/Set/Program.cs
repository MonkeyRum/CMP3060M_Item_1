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
            // Probably the most fragile code ever...

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
            testingCsv = testingCsv.Substring(0, testingCsv.Length - 1);

            WriteCsv(outputFolder + @"\training.csv", ref trainingCsv);
            WriteCsv(outputFolder + @"\testing.csv", ref testingCsv);

            BuildLabelTestCsv(testingCsv);
            BuildLabelTrainCsv(trainingCsv);
            BuildDiscrimTestCsv(testingCsv);
            BuildDiscrimTrainCsv(trainingCsv);
        }

        private static void SplitIntoSets(List<string> paths, ref string trainingCsv, ref string testingCsv, string suffix, string imageType)
        {
            int mid = (paths.Count / 3) * 2;

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

        private static void BuildLabelTestCsv(string testing_csv)
        {
            string label_path = @"C:\Users\Alex\Dropbox\CMP3060M\ImageDataSet\ManualClassification\Alex2";

            string[] files = testing_csv.Split(',');
            RemoveExtensions(ref files);

            string[] filePaths = Directory.GetFiles(label_path);
            List<string> acceptedFilesPaths = new List<string>();

            foreach (var str in filePaths)
            {
                foreach(var str2 in files)
                {
                    if(str.Contains(str2))
                    {
                        acceptedFilesPaths.Add(Path.GetFileName(str));
                    }
                }
            }

            acceptedFilesPaths = acceptedFilesPaths.Distinct().ToList();
            string csvOut = string.Empty;

            foreach(var str in acceptedFilesPaths)
            {
                csvOut += str + ",";
            }

            csvOut = csvOut.Substring(0, csvOut.Length - 1);

            WriteCsv(@"C:\Users\Alex\Dropbox\CMP3060M\ImageDataSet\WORKING_SET\testing_labels.csv", ref csvOut);
        }

        private static void BuildLabelTrainCsv(string training_csv)
        {
            string label_path = @"C:\Users\Alex\Dropbox\CMP3060M\ImageDataSet\ManualClassification\Alex2";

            string[] files = training_csv.Split(',');
            RemoveExtensions(ref files);

            string[] filePaths = Directory.GetFiles(label_path);
            List<string> acceptedFilesPaths = new List<string>();

            foreach (var str in filePaths)
            {
                foreach (var str2 in files)
                {
                    if (str.Contains(str2))
                    {
                        acceptedFilesPaths.Add(Path.GetFileName(str));
                    }
                }
            }

            acceptedFilesPaths = acceptedFilesPaths.Distinct().ToList();
            string csvOut = string.Empty;

            foreach (var str in acceptedFilesPaths)
            {
                csvOut += str + ",";
            }

            csvOut = csvOut.Substring(0, csvOut.Length - 1);

            WriteCsv(@"C:\Users\Alex\Dropbox\CMP3060M\ImageDataSet\WORKING_SET\training_labels.csv", ref csvOut);
        }

        private static void BuildDiscrimTestCsv(string testing_csv)
        {
            string label_path = @"C:\Users\Alex\Dropbox\CMP3060M\ImageDataSet\FeatureData\256";

            string[] files = testing_csv.Split(',');
            RemoveExtensions(ref files);

            string[] filePaths = Directory.GetFiles(label_path);
            List<string> acceptedFilesPaths = new List<string>();

            foreach (var str in filePaths)
            {
                foreach (var str2 in files)
                {
                    if (str.Contains(str2))
                    {
                        acceptedFilesPaths.Add(Path.GetFileName(str));
                    }
                }
            }

            acceptedFilesPaths = acceptedFilesPaths.Distinct().ToList();
            string csvOut = string.Empty;

            foreach (var str in acceptedFilesPaths)
            {
                csvOut += str + ",";
            }

            csvOut = csvOut.Substring(0, csvOut.Length - 1);

            WriteCsv(@"C:\Users\Alex\Dropbox\CMP3060M\ImageDataSet\WORKING_SET\testing_data.csv", ref csvOut);
        }

        private static void BuildDiscrimTrainCsv(string training_csv)
        {
            string label_path = @"C:\Users\Alex\Dropbox\CMP3060M\ImageDataSet\FeatureData\256";

            string[] files = training_csv.Split(',');
            RemoveExtensions(ref files);

            string[] filePaths = Directory.GetFiles(label_path);
            List<string> acceptedFilesPaths = new List<string>();

            foreach (var str in filePaths)
            {
                foreach (var str2 in files)
                {
                    if (str.Contains(str2))
                    {
                        acceptedFilesPaths.Add(Path.GetFileName(str));
                    }
                }
            }

            acceptedFilesPaths = acceptedFilesPaths.Distinct().ToList();
            string csvOut = string.Empty;

            foreach (var str in acceptedFilesPaths)
            {
                csvOut += str + ",";
            }

            csvOut = csvOut.Substring(0, csvOut.Length - 1);

            WriteCsv(@"C:\Users\Alex\Dropbox\CMP3060M\ImageDataSet\WORKING_SET\training_data.csv", ref csvOut);
        }

        private static void RemoveExtensions(ref string[] path)
        {
            for(int i = 0; i < path.Length; ++i)
            {
                path[i] = Path.GetFileNameWithoutExtension(path[i]);
            }
        }

        private static void WriteCsv(string outputPath, ref string contents)
        {
            File.WriteAllText(outputPath, contents);
        }
    }
}
