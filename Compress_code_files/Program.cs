using System.CommandLine;
using System.CommandLine.Parsing;
 
public class Program
{
    public static void Main(string[] args)
    {
        {
            var bundleCommand = new Command("bundle", "Create a bundle of code files");
            var createRspCommand = new Command("create-rsp", "Create a response file");
            /*{
                Handler = CommandHandler.Create(CreateRspFile)
            };
            var outputFileOption = new Option<string>("--outputFile", "Output file name") { IsRequired = true };
            outputFileOption.AddAlias("-o");
            createRspCommand.AddOption(outputFileOption);*/

            var languageOption = new Option<string>("--language", "Programming language") { IsRequired = true };
            languageOption.AddAlias("-l");
            bundleCommand.AddOption(languageOption);

            var outputOption = new Option<string>("--output", "Output bundle file name") { IsRequired = true };
            outputOption.AddAlias("-o");
            bundleCommand.AddOption(outputOption);

            var noteOption = new Option<string>("--note", "Add a note to the bundle file");
            noteOption.AddAlias("-n");
            bundleCommand.AddOption(noteOption);

            var authorOption = new Option<string>("--author", "Add an author to the bundle file");
            authorOption.AddAlias("-a");
            bundleCommand.AddOption(authorOption);

            var sortOption = new Option<string>("--sort", "Sort the code files by name or type");
            sortOption.AddAlias("-s");
            bundleCommand.AddOption(sortOption);

            var removeEmptyLinesOption = new Option<bool>("--remove_empty_lines", "Remove empty lines from source code");
            removeEmptyLinesOption.AddAlias("-r");
            bundleCommand.AddOption(removeEmptyLinesOption);
 
            bundleCommand.SetHandler<string, string, string, string, string,bool >((language, output, note, author, sort, removeEmptyLines)=>
            {
                 BundleCodeFiles(language, output, note, author, sort, removeEmptyLines);
            }, languageOption, outputOption, noteOption, authorOption, sortOption, removeEmptyLinesOption);

            createRspCommand.SetHandler(() =>
            {
                var options = new List<Option>()
            {
             new Option<string>("--language", "Programming language") { IsRequired = true },
             new Option<string>("--output", "Output bundle file name") { IsRequired = true },
             new Option<string>("--note", "Add a note to the bundle file"),
             new Option<string>("--author", "Add an author to the bundle file"),
             new Option<string>("--sort", "Sort the code files by name or type"),
             new Option<string>("--remove_empty_lines", "Remove empty lines from source code")
             };
                Console.WriteLine("Enter Name for file ");
                string outputFile;
                outputFile= Console.ReadLine();
                outputFile = outputFile + ".rsp";
                string responseFile = Path.Combine(Directory.GetCurrentDirectory(), outputFile);

                using (var writer = new StreamWriter(responseFile))
                {
                    string responseFileContent = "";
                    foreach (var option in options)
                    {
                        Console.Write($"Enter the value for {option.Name}: ");
                        var value = Console.ReadLine();

                        if (option.IsRequired && string.IsNullOrEmpty(value))
                        {
                        while(string.IsNullOrEmpty(value))
                        {
                            Console.WriteLine("Please provide a value for the required option.");
                            Console.Write($"Enter the value for {option.Name}: ");
                            value = Console.ReadLine();
                        } 
                        }
                        if(!string.IsNullOrEmpty(value))
                        responseFileContent += $"--{option.Name} {value}\n";
                    }

                    writer.Write(responseFileContent);
                }

                Console.WriteLine($"Response file created: {responseFile}");
            });

            var rootCommand = new RootCommand();
            rootCommand.AddCommand(bundleCommand);
            rootCommand.AddCommand(createRspCommand); 

            rootCommand.Invoke(args);
        }
    }
    
    static void BundleCodeFiles(string language, string output, string? note = null, string? author = null, string? sort = null, bool remove_empty_lines = false)
    {


        string[] arr = new string[] { "java", "c#", "python" ,"all"};
        if(!arr.Contains(language))
        {
            while(!arr.Contains(language))
            {
                Console.WriteLine("The language you entered is not a programming language, please enter a programming language");
                language = Console.ReadLine();
            }
        }
        string[] codeFiles = new string[0]; // אתחול מערך ריק כדי לאחסן את קבצי הקוד
         switch (language.ToLower())//קבלת קובצי הקוד על סמך השפה שנבחרה
        {
            case "java":
                {
                    if (!string.IsNullOrEmpty(sort))
                        codeFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.java").OrderBy(file => file).ToArray();//מיון על פי שם הקובץ
                    else
                        codeFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.java");
                }
                break;

            case "c#":
                {
                     if (!string.IsNullOrEmpty(sort))
                        codeFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.cs").OrderBy(file => file).ToArray();
                    else
                        codeFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.cs");
                }
                break;

            case "python":
                {
                     if (!string.IsNullOrEmpty(sort))
                        codeFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.py").OrderBy(file => file).ToArray();
                    else
                        codeFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.py");
                    break;
                }

            default:
                {
                     if (!string.IsNullOrEmpty(sort))
                        codeFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.*").OrderBy(file => Path.GetExtension(file)).ToArray();// מיון על פי סוג הקובץ
                    else
                        codeFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.*");
                }
                break;
        }

        string bundleFilePath;
        if (Path.IsPathRooted(output))//אם נשלח ניתוב, אז שמור את הקובץ במיקום שהתקבל 
            bundleFilePath = output;
        else
            bundleFilePath = Path.Combine(Directory.GetCurrentDirectory(), output);//אחרת, קבל את ניתוב התיקיה הקיימת ושם ישמר הקובץ 
        bundleFilePath += ".txt";

        try
        {
            using (var bundleFile = File.CreateText(bundleFilePath))//בקובץ זה ישמר לי כל קבצי הקוד העונים על השפה שנבחרה 
            {
                if (!string.IsNullOrWhiteSpace(author))//תנאי הזה מוודא שהמחרוזת אינה ריקה ואין בה רק רווחים
                {
                    bundleFile.WriteLine($"author: {author}");
                }

                foreach (var codeFile in codeFiles)
                {
                    if (!string.IsNullOrWhiteSpace(note))
                    {
                        if (language.ToLower() == "python")
                            bundleFile.WriteLine($"#Code Source: {codeFile}");
                        else
                            bundleFile.WriteLine($"//Code Source: {codeFile}");

                    }
                    string code = File.ReadAllText(codeFile);

                    if (remove_empty_lines)
                    {
                        code = RemoveEmptyLines(code);
                    }
                    bundleFile.WriteLine(code);
                    
                }
            }


            Console.WriteLine($"Bundle file created successfully: {bundleFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create bundle file: {ex.Message}");
        }

    }
    static string RemoveEmptyLines(string code)
    {
        string[] lines = code.Split(Environment.NewLine);//חילוק הקובץ לפי שורות 
        lines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();//מקבל רק שורות שאינם ריקות!!!
        return string.Join("\n", lines);
    }
    


}



