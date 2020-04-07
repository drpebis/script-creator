using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptCreator
{
    public class ClusterListParser
    {
        Dictionary<string, string> allTheThings = new Dictionary<string, string>();

        public void ReadIt()
        {
            // Open the file into a streamreader
            using (System.IO.StreamReader sr = new System.IO.StreamReader("./script-creator/script-creator/DCs.txt"))
            {
                while (!sr.EndOfStream) // Keep reading until we get to the end
                {
                    string splitMe = sr.ReadLine();
                    string[] clusters = splitMe.Split(new char[] { ':' }); //Split at the colons

                    if (clusters.Length < 2) // If we get less than 2 results, discard them
                        continue;
                    else if (clusters.Length == 2) // Easy part. If there are 2 results, add them to the dictionary
                        allTheThings.Add(clusters[0].Trim(), clusters[1].Trim());
                    else if (clusters.Length > 2)
                        SplitItGood(splitMe, allTheThings); // Hard part. If there are more than 2 results, use the method below.
                }
            }
        }

        public void SplitItGood(string stringInput, Dictionary<string, string> dictInput)
        {
            StringBuilder sb = new StringBuilder();
            List<string> clusterList = new List<string>(); // This list will hold the keys and values as we find them
            bool hasFirstValue = false;

            foreach (char c in stringInput) // Iterate through each character in the input
            {
                if (c != ':') // Keep building the string until we reach a colon
                    sb.Append(c);
                else if (c == ':' && !hasFirstValue)
                {
                    clusterList.Add(sb.ToString().Trim());
                    sb.Clear();
                    hasFirstValue = true;
                }
                else if (c == ':' && hasFirstValue)
                {

                    // Below, the StringBuilder currently has something like this:
                    // "    235235         Some Text Here"
                    // We trim the leading whitespace, then split at the first sign of a double space
                    string[] stringSplit = sb.ToString()
                                             .Trim()
                                             .Split(new string[] { "  " },
                                                    StringSplitOptions.RemoveEmptyEntries);

                    // Add both results to the list
                    clusterList.Add(stringSplit[0].Trim());
                    clusterList.Add(stringSplit[1].Trim());
                    sb.Clear();
                }
            }

            clusterList.Add(sb.ToString().Trim()); // Add the last result to the list

            for (int i = 0; i < clusterList.Count; i += 2)
            {
                // This for loop assumes that the amount of keys and values added together
                // is an even number. If it comes out odd, then one of the lines on the input
                // text file wasn't parsed correctly or wasn't generated correctly.
                dictInput.Add(clusterList[i], clusterList[i + 1]);
            }
        }
    }
}
