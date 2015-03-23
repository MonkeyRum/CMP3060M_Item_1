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
        static string label_path = @"C:\DATA\class labels";
        static string data_path = @"C:\DATA\discriminants";
        static string output_folder = @"C:\DATA\working set\";

        static string testing_labels_csv = output_folder + @"testing_labels.csv";
        static string training_labels_csv = output_folder + @"training_labels.csv";
        static string testing_data_csv = output_folder + @"testing_data.csv";
        static string training_data_csv = output_folder + @"training_data.csv";
        
        static string image_type = "jpg";
        static string suffix = "_manualClassification.csv";

        static void Main(string[] args)
        {
            // Probably the most fragile code ever...

            List<string> goodFilepaths = new List<string>();
            List<string> badFilepaths = new List<string>();
            string manualClassificationFolder = label_path;
            string outputFolder = output_folder;
            string imageType = image_type;

            string[] filePaths = Directory.GetFiles(manualClassificationFolder);

            foreach (var file in filePaths)
            {
                var reader = new StreamReader(File.OpenRead(file));
                bool isBad = false;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line.Contains("1") || line.Contains("2") || line.Contains("3"))
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

            WriteCsv(testing_labels_csv, ref csvOut);
        }

        private static void BuildLabelTrainCsv(string training_csv)
        {
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

            WriteCsv(training_labels_csv, ref csvOut);
        }

        private static void BuildDiscrimTestCsv(string testing_csv)
        {
            string[] files = testing_csv.Split(',');
            RemoveExtensions(ref files);

            string[] filePaths = Directory.GetFiles(data_path);
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

            WriteCsv(testing_data_csv, ref csvOut);
        }

        private static void BuildDiscrimTrainCsv(string training_csv)
        {
            string[] files = training_csv.Split(',');
            RemoveExtensions(ref files);

            string[] filePaths = Directory.GetFiles(data_path);
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

            WriteCsv(training_data_csv, ref csvOut);
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
