using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace OS1 {
    class Person {
        public string Name {get; set;}
        public int Age {get; set;}
    }
    class Program {
        static async Task Main(string[] args) {
            string Check1, Check2, Check3, Check4, Check5;
            //Задание 1
            Console.Write("Task 1:");
            Console.ReadLine();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach(DriveInfo drive in drives) {
                Console.WriteLine($"Название: {drive.Name}");
                Console.WriteLine($"Тип: {drive.DriveType}");
                if (drive.IsReady) {
                    Console.WriteLine($"Объем диска: {drive.TotalSize}");
                    Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                    Console.WriteLine($"Метка: {drive.VolumeLabel}");
                }
                Console.WriteLine();
            }
            //Задание 2
            Console.Write("Task 2:");
            Console.Read();
            string path = @ "C:\OS1";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists) {
                dirInfo.Create();
                Console.WriteLine("Папка успешно создана");
            } else {
                Console.WriteLine("Папка уже существует");
            }
            Console.Read();
            Console.WriteLine("Введите строку для записи в файл:");
            string text = Console.ReadLine();

            using(FileStream fstream = new FileStream($ @ "{path}\test.txt", FileMode.Append)) {
                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                await fstream.WriteAsync(array, 0, array.Length);
            }
            using(FileStream fstream = File.OpenRead($ @ "{path}\test.txt")) {
                byte[] array = new byte[fstream.Length];
                await fstream.ReadAsync(array, 0, array.Length);

                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Console.WriteLine($"Текст из файла: {textFromFile}");
            }
            Console.WriteLine("Хотите удалить файл?(+)");
            Check1 = Console.ReadLine();
            if (Check1 == "+") {
                File.Delete($ @ "{path}\test.txt");
                Console.WriteLine("Файл test.txt удалён");
            }
            Console.WriteLine();
            //Задание 3
            Console.Write("Task 3:");
            Console.Read();
            using(FileStream fstream = new FileStream($ @ "{path}\user.json", FileMode.OpenOrCreate)) {
                Person Egor = new Person() {
                    Name = "Egor", Age = 19
                };
                await JsonSerializer.SerializeAsync < Person > (fstream, Egor);
                Console.WriteLine("Файл был создан и уже содержит данные");
            }
            using(FileStream fstream = File.OpenRead($ @ "{path}\user.json")) {
                Person restoredPerson = await JsonSerializer.DeserializeAsync < Person > (fstream);
                Console.WriteLine($"Name: {restoredPerson.Name}  Age: {restoredPerson.Age}");
            }
            Console.WriteLine("Хотите удалить файл?(+)");
            Console.Read();
            Check1 = Console.ReadLine();
            switch (Check1) {
            case "+":
                File.Delete($ @ "{path}\user.json");
                Console.WriteLine("Файл user.json удалён");
                break;
            }
            Console.WriteLine();
            //Задание 4
            Console.Write("Task 4:");
            Console.Read();
            Console.WriteLine();
            Console.Read();
            XDocument xdoc = new XDocument(new XElement("people",
                new XElement("person",
                    new XAttribute("name", "Egorik"),
                    new XElement("company", "Microsoft"),
                    new XElement("age", 19)),
                new XElement("person",
                    new XAttribute("name", "Vova"),
                    new XElement("company", "Google"),
                    new XElement("age", 21))));
            xdoc.Save($ @ "{path}\people.xml");
            Console.WriteLine("people.xml created");
            Console.WriteLine("Введите имя для добавления в файл:");
            string tempname = Console.ReadLine();
            Console.WriteLine("Введите компанию для добавления в файл:");
            string tempcompany = Console.ReadLine();
            Console.WriteLine("Введите возраст для добавления в файл:");
            string tempage = Console.ReadLine();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load($ @ "{path}\people.xml");
            XmlElement ? xRoot = xDoc.DocumentElement;
            XmlElement personElem = xDoc.CreateElement("people");
            XmlAttribute nameAttr = xDoc.CreateAttribute("name");
            XmlElement companyElem = xDoc.CreateElement("company");
            XmlElement ageElem = xDoc.CreateElement("age");
            XmlText nameText = xDoc.CreateTextNode(tempname);
            XmlText companyText = xDoc.CreateTextNode(tempcompany);
            XmlText ageText = xDoc.CreateTextNode(tempage);
            nameAttr.AppendChild(nameText);
            companyElem.AppendChild(companyText);
            ageElem.AppendChild(ageText);
            personElem.Attributes.Append(nameAttr);
            personElem.AppendChild(companyElem);
            personElem.AppendChild(ageElem);
            xRoot?.AppendChild(personElem);
            xDoc.Save($ @ "{path}\people.xml");
            Console.WriteLine("people.xml edited\n");
            XmlDocument xxDoc = new XmlDocument();
            xDoc.Load($ @ "{path}\people.xml");
            XmlElement xxRoot = xDoc.DocumentElement;
            foreach(XmlNode xnode in xRoot) {
                if (xnode.Attributes.Count > 0) {
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");
                    if (attr != null)
                        Console.WriteLine(attr.Value);
                }
                foreach(XmlNode childnode in xnode.ChildNodes) {
                    if (childnode.Name == "company") {
                        Console.WriteLine($"Компания: {childnode.InnerText}");
                    }
                    if (childnode.Name == "age") {
                        Console.WriteLine($"Возраст: {childnode.InnerText}");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("Хотите удалить файл?(+)");
            Check3 = Console.ReadLine();
            if (Check3 == "+") {
                File.Delete($ @ "{path}\people.xml");
                Console.WriteLine("Файл people.xml удалён");
                Console.WriteLine();
            }
            //Задание 5
            Console.Write("Task 5:");
            Console.Read();
            Console.WriteLine();
            string somepath = @ "C:\OS1\zip";
            DirectoryInfo dirInfoo = new DirectoryInfo(somepath);
            if (!dirInfoo.Exists) {
                dirInfoo.Create();
            }
            Console.Read();
            using(FileStream fstream = new FileStream($ @ "{path}\test2.txt", FileMode.CreateNew)) {}
            string sourceFolder = @ "C:\OS1\zip\";
            string zipFile = @ "C:\OS1\zip.zip";
            ZipFile.CreateFromDirectory(sourceFolder, zipFile);
            Console.WriteLine($"Папка {sourceFolder} создана и конвертирована в архив {zipFile}");
            Console.Read();
            using(ZipArchive zipArchive = ZipFile.Open(zipFile, ZipArchiveMode.Update)) {
                zipArchive.CreateEntryFromFile(@ "C:\OS1\test2.txt", "test2.txt");
            }
            Console.Read();
            Console.Write($"test2.txt добавлен в архив {zipFile}\n");
            Console.Read();
            ZipFile.ExtractToDirectory(zipFile, sourceFolder);
            Console.WriteLine($"Архив {zipFile} распакован в папку {sourceFolder}");
            Console.WriteLine();
            Console.Read();
            Console.WriteLine("Хотите удалить файлы?(+)");
            Check4 = Console.ReadLine();
            if (Check4 == "+") {
                foreach(FileInfo file in dirInfo.GetFiles()) {
                    file.Delete();
                }
                Directory.Delete(sourceFolder, true);
                Console.WriteLine("Файлы из Task 5 удалены");
                Console.Read();
            }
            Console.WriteLine("Удаляём всё нами созданное?(+)");
            Check5 = Console.ReadLine();
            if (Check5 == "+") {
                Directory.Delete(path, true);
                Console.WriteLine("OS1 успешно удалена с вашего компьютера ;)))");
            }
        }
    }
}
