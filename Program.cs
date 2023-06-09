﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using IWshRuntimeLibrary;

namespace SFACS {
    internal class Program {
        static void Main(string[] args) {
            try {
                // переменные
                string searchPath, finalPath, filesType;

                // лого
                Console.WriteLine(); Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("   ######   #######    ###         #####      ######  ");
                Console.WriteLine("  ######   #######    #####      #########   ######   ");
                Console.WriteLine("  ##       ##        ##   ##    ###     ###  ##       ");
                Console.WriteLine("  ##       ##        ##   ##    ##           ##       ");
                Console.WriteLine("  ######   ######   ##     ##   ##           ######   ");
                Console.WriteLine("   ######  ######   ##     ##   ##            ######  ");
                Console.WriteLine("       ##  ##      ###########  ##                ##  ");
                Console.WriteLine("       ##  ##      ###     ###  ###     ###       ##  ");
                Console.WriteLine("   ######  ##      ##       ##   #########    ######  ");
                Console.WriteLine("  ######   ##      ##       ##     #####     ######   ");
                Console.WriteLine(); Console.ForegroundColor = ConsoleColor.Gray;

                // получаем данные от пользователя
                Console.Write("Путь для поиска: "); searchPath = Console.ReadLine();
                Console.Write("Путь для ярлыков: "); finalPath = Console.ReadLine();
                Console.Write("Расширение файлов: "); filesType = Console.ReadLine();

                // убирает кавычки по краям ссылки
                if (finalPath.StartsWith("\"")) finalPath = finalPath.Substring(1);
                if (finalPath.EndsWith("\"")) finalPath = finalPath.Substring(0, finalPath.Length - 1);
                if (searchPath.StartsWith("\"")) searchPath = searchPath.Substring(1);
                if (searchPath.EndsWith("\"")) searchPath = searchPath.Substring(0, searchPath.Length - 1);

                // добавляет слэш в конец строки
                if (!finalPath.EndsWith("\\")) finalPath += "\\";
                if (!searchPath.EndsWith("\\")) searchPath += "\\";

                // исправляет расширение файла
                if (!filesType.StartsWith(".")) filesType = "." + filesType;

                void Search(string path) {
                    if (path.EndsWith(filesType)) {
                        List<string> folderOrFile = new List<string>();
                        foreach (string element in path.Replace(searchPath, "").Split(new char[] { '\\' })) {
                            folderOrFile.Add(element);
                        };
                        if (folderOrFile.Count <= 1) {
                            folderOrFile.Add(folderOrFile.First());
                            folderOrFile.Insert(0, folderOrFile.Last().Substring(0, folderOrFile.Last().Length - filesType.Length));
                        };
                        Directory.CreateDirectory(finalPath + folderOrFile.First());
                        WshShell wshShell = new WshShell();
                        IWshShortcut Shortcut = (IWshShortcut)wshShell.CreateShortcut(finalPath + folderOrFile.First() + "\\" + folderOrFile.Last().Substring(0, folderOrFile.Last().Length - filesType.Length) + ".lnk");
                        Shortcut.TargetPath = path;
                        Shortcut.WorkingDirectory = searchPath + folderOrFile.First();
                        Shortcut.Save();
                        Console.WriteLine(path);
                    };
                    try { Parallel.ForEach(Directory.GetFileSystemEntries(path), element => { Search(element); }); } catch { };
                };
                try { Parallel.ForEach(Directory.GetFileSystemEntries(searchPath), element => { Search(element); }); } catch { };

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Готово! Для закрытия нажмите enter...");
                Console.ForegroundColor = ConsoleColor.Gray;
            } catch (Exception error) { Console.WriteLine(error); };
            Console.ReadLine();
        }
    }
}
