using System;
using System.Collections.Generic;
using System.Text;

namespace OOAGame
{
    public static class AsciiSpritesheetCutter
    {
        // The height of each frame
        const int FrameHeight = 28;
        // The amount of frames in the sprite sheet
        const int FrameCount = 23;

        // Reads the whole sheet and writes each Ascii frame to its own .txt file
        public static void FileCut(string sheetPath, string outputFolder, ConsoleColor textColour = ConsoleColor.White)
        {

            string[] lines = File.ReadAllLines(sheetPath);

            Directory.CreateDirectory(outputFolder);

            for (int f = 0; f < FrameCount; f++)
            {
                int startY = f * FrameHeight;

                string[] frameRows = new string[FrameHeight];

                for (int y = 0; y < FrameHeight; y++)
                {
                    int sheetRow = startY + y;
                    string line = sheetRow < lines.Length ? lines[sheetRow] : ""; 

                    frameRows[y] = line;
                }

                string outPath = Path.Combine(outputFolder, $"frame_{f:D2}.txt");

                Console.ForegroundColor = textColour;
                File.WriteAllLines(outPath, frameRows);
            }
        }
    }
}
