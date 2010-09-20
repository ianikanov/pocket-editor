using System;

using System.Collections.Generic;
using System.Text;

namespace JSEditor
{
    /// <summary>
    /// Singleton with languages
    /// </summary>
    public class LanguagePack
    {
        public Dictionary<DevelopLanguages, CLanguage> Languages { get; set; }

        private static LanguagePack _inst;
        public static LanguagePack Instance
        {
            get
            {
                if (_inst == null)
                {
                    _inst = new LanguagePack();
                }
                return _inst;
            }
        }

        private LanguagePack()
        {
            Languages = new Dictionary<DevelopLanguages, CLanguage>();
            Languages.Add(DevelopLanguages.Php,
                new CLanguage
                {
                    Separators = new List<string> { " ", "+", "-", "/", "*", "(", ")", ";", "<", ">", "=" },
                    Language = DevelopLanguages.Php,
                    KeyWords = new List<string> { "for", "foreach", "while", "do", "if", "int" },
                    IsCaseSensitive = true,
                    Classes = new List<string>()
                });
            Languages.Add(DevelopLanguages.ActionScript,
                new CLanguage
                {
                    Separators = new List<string> { " ", "+", "-", "/", "*", "(", ")", ";", "<", ">", "=" },
                    Language = DevelopLanguages.ActionScript,
                    KeyWords = new List<string> { "for", "foreach", "while", "do", "if", "int" },
                    IsCaseSensitive = true,
                    Classes = new List<string> { "Number", "String", "DisplayObject", "MovieClip", "Event", "Error" }
                });
            Languages.Add(DevelopLanguages.Sql,
                new CLanguage
                {
                    Separators = new List<string> { " ", "+", "-", "/", "*", "(", ")", ";", "<", ">", "=" },
                    Language = DevelopLanguages.Sql,
                    KeyWords = new List<string> { "while", "do", "case", "when", "then", "if", 
                        "exec", "execute", "select", "update", "insert", "delete", "call",
                        "int", "varchar", "nvarchar", "bit" },
                    IsCaseSensitive = false,
                    Classes = new List<string>()
                });
            Languages.Add(DevelopLanguages.Html,
                new CLanguage
                {
                    Separators = new List<string> { " ", "+", "-", "/", "*", "(", ")", ";", "<", ">", "=" },
                    Language = DevelopLanguages.Html,
                    KeyWords = new List<string>(),
                    IsCaseSensitive = false,
                    Classes = new List<string>()
                });
            Languages.Add(DevelopLanguages.Javascript,
                new CLanguage
                {
                    Separators = new List<string> { " ", "+", "-", "/", "*", "(", ")", ";", "<", ">", "=" },
                    Language = DevelopLanguages.Javascript,
                    KeyWords = new List<string> { "for", "foreach", "while", "do", "if" },
                    IsCaseSensitive = true,
                    Classes = new List<string>()
                });
            Languages.Add(DevelopLanguages.CSharp,
                new CLanguage
                {
                    Separators = new List<string> { " ", "+", "-", "/", "*", "(", ")", ";", "<", ">", "=" },
                    Language = DevelopLanguages.CSharp,
                    KeyWords = new List<string> { "for", "foreach", "while", "do", "if", "int", "char", "string", "bool" },
                    IsCaseSensitive = true,
                    Classes = new List<string> { "Console" }
                });
            Languages.Add(DevelopLanguages.Cpp,
                new CLanguage
                {
                    Separators = new List<string> { " ", "+", "-", "/", "*", "(", ")", ";", "<", ">", "=" },
                    Language = DevelopLanguages.Cpp,
                    KeyWords = new List<string> { "for", "foreach", "while", "do", "if", "int", "char", "string", "bool" },
                    IsCaseSensitive = true,
                    Classes = new List<string>()
                });
        }
    }

    public class CLanguage
    {
        public DevelopLanguages Language { get; set; }

        public bool IsCaseSensitive { get; set; }

        public List<string> Separators { get; set; }
        public List<string> KeyWords { get; set; }
        public List<string> Classes { get; set; }
    }

    public enum DevelopLanguages
    {
        Php,
        ActionScript,
        Html,
        Javascript,
        Sql,
        CSharp,
        Cpp
    }
}
