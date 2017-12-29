using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WyswietlanieTextu.Model;

namespace WyswietlanieTextu.MainViewModel
{
    public class MainViewModels : INotifyPropertyChanged
    {
        public ICommand Przekonwertuj { get; set; }
        public ICommand Wyczyść { get; set; }

        private Parametry param;
        public Parametry Param
        {
            get { return param; }
            set
            {
                param = value;
                OnPropertyChanged("Text");
            }
        }
     
        //wlasciwosc StringBuilder
        private StringBuilder result = new StringBuilder();
        //odebrane jako string
        public string Result
        {
            get { return result.ToString(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        public MainViewModels()
        {
            Przekonwertuj = new RelayCommand(PrzekonwertujButton);
            Wyczyść = new RelayCommand(WyczyśćButton);
            param = new Parametry("Wprowadz Text :");
        }

        private void PrzekonwertujButton()
        {
            string content = param.Text;
            var lines = GetLinesFromContent(content);
            var declarations = new List<Zmienna>();

            GetDeclarations(lines, declarations);

            int licznik = 0; int licznik_bajtow = 0;

            try
            {
                SetNewValue(declarations, ref licznik, ref licznik_bajtow);
                GetLastLine();
            }
            catch (Exception)
            {
                MessageBox.Show("Wprowadzono niepoprawna strukture !");
            }
            OnPropertyChanged("Result");
        }

        private void GetDeclarations(List<string> lines, List<Zmienna> declarations)
        {
            foreach (var line in lines)
            {
                if (line.Contains("ST_POMIARY"))
                {

                    result.AppendLine("[StructLayout(LayoutKind.Explicit, Pack = 1)]");
                    result.AppendLine("public struct PomiaryStruct");
                    result.AppendLine("{");

                    var startp = line.IndexOf("TYP");
                    var endp = line.IndexOf("UCT");
                    var ed = endp + 3;

                    var deklarujStruct = line.Remove(startp, ed - startp);
                    declarations.Add(GetVariable(deklarujStruct));
                    continue;
                }

                //jesli w lini znajduje sie ""attribute""
                if (line.Contains("attribute"))
                {
                    //wyciecie wewnetrzne 
                    var start = line.IndexOf(@"\r\n");
                    var end = line.IndexOf("}");

                    var nowaDeklaracja = line.Remove(start + 1, end - start);

                    declarations.Add(GetVariable(nowaDeklaracja));
                    continue;
                }

                var poczatek = line.IndexOf(@"//");
                var koniec = line.IndexOf(Environment.NewLine);
                if (poczatek == -1 || koniec == -1)
                {
                    declarations.Add(GetVariable(line));
                }
                else
                {
                    //pierwsze ciecie 
                    var deklaracja = line.Remove(poczatek, koniec - poczatek);
                    declarations.Add(GetVariable(deklaracja));
                }
            }
        }

        private void SetNewValue(List<Zmienna> declarations, ref int licznik, ref int licznik_bajtow)
        {
            foreach (var item in declarations)
            {
                if (declarations[licznik].Type == "DINT")
                {
                    var int_type32 = declarations[licznik].Type = "Int32";

                    result.AppendLine("[FieldOffset(" + licznik_bajtow + ")]");
                    result.AppendLine("public " + int_type32 + " _" + declarations[licznik].Name +";");
                    result.AppendLine("public " + int_type32 + " " + declarations[licznik].Name);
                    result.AppendLine("{");
                    result.AppendLine("   get");
                    result.AppendLine("       {");
                    result.AppendLine("           return "+" _" +declarations[licznik].Name);
                    result.AppendLine("       }");
                    result.AppendLine("   set");
                    result.AppendLine("       {");
                    result.AppendLine("           _" +declarations[licznik].Name + " = value;");
                    result.AppendLine("       }");
                    result.AppendLine("}");
                    result.AppendLine("");
                    //dodanie nowego bajta
                    //licznik_bajtow = licznik_bajtow + 4;
                    licznik_bajtow += 4;
                }
                else if (declarations[licznik].Type == "REAL")
                {

                    var float_type = declarations[licznik].Type = "float";

                    result.AppendLine("[FieldOffset(" + licznik_bajtow + ")]");
                    result.AppendLine("public " + float_type + " _" + declarations[licznik].Name + ";");
                    result.AppendLine("public " + float_type + " " + declarations[licznik].Name);
                    result.AppendLine("{");
                    result.AppendLine("   get");
                    result.AppendLine("       {");
                    result.AppendLine("           return " + " _" + declarations[licznik].Name);
                    result.AppendLine("       }");
                    result.AppendLine("   set");
                    result.AppendLine("       {");
                    result.AppendLine("           _" + declarations[licznik].Name + " = value;");
                    result.AppendLine("       }");
                    result.AppendLine("}");
                    result.AppendLine("");
                    //dodanie nowego
                    licznik_bajtow += 4;


                }
                else if (declarations[licznik].Type == "INT")
                {
                    var int_type = declarations[licznik].Type = "Int16";

                    result.AppendLine("[FieldOffset(" + licznik_bajtow + ")]");
                    result.AppendLine("public " + int_type + " _" + declarations[licznik].Name + ";");
                    result.AppendLine("public " + int_type + " " + declarations[licznik].Name);
                    result.AppendLine("{");
                    result.AppendLine("   get");
                    result.AppendLine("       {");
                    result.AppendLine("           return " + " _" + declarations[licznik].Name);
                    result.AppendLine("       }");
                    result.AppendLine("   set");
                    result.AppendLine("       {");
                    result.AppendLine("            _" + declarations[licznik].Name + " = value;");
                    result.AppendLine("       }");
                    result.AppendLine("}");
                    result.AppendLine("");
                    ////dodanie nowego bajta
                    licznik_bajtow += 2;
                }
                else if (declarations[licznik].Type == "UINT")
                {
                    var int_type32 = declarations[licznik].Type = "UInt16";

                    result.AppendLine("[FieldOffset(" + licznik_bajtow + ")]");
                    result.AppendLine("public " + int_type32 + " _" + declarations[licznik].Name + ";");
                    result.AppendLine("public " + int_type32 + " " + declarations[licznik].Name);
                    result.AppendLine("{");
                    result.AppendLine("   get");
                    result.AppendLine("       {");
                    result.AppendLine("           return " + " _" + declarations[licznik].Name);
                    result.AppendLine("       }");
                    result.AppendLine("   set");
                    result.AppendLine("       {");
                    result.AppendLine("            _" + declarations[licznik].Name + " = value;");
                    result.AppendLine("       }");
                    result.AppendLine("}");
                    result.AppendLine("");
                    //dodanie nowego bajta
                    licznik_bajtow += 2;
                }

                licznik++;
                if (licznik == declarations.Count - 1)
                {
                    break;
                }
            }
        }

        private void GetLastLine()
        {
            result.AppendLine("}");
        }
        
        private Zmienna GetVariable(string variable)
        {
            //tutaj wychwyce 
            variable = variable.Trim().Replace(" ", string.Empty);
            var singlevariable = variable.Split(':');

            if (singlevariable[0] == @"END_STRUCT\r\nEND_TYPE")
            {
                result.AppendLine("Inny sposob");
            }

            if (singlevariable.Count() == 2)
            {
              
                return new Zmienna(singlevariable[0], singlevariable[1]);
            }

            return null;
        }

        private List<string> GetLinesFromContent(string content)
        {
            //calosc - jako ciag znakow
            content = content.Trim().Replace(" ", string.Empty);
            var lines = content.Split(';');
            return lines.ToList();
        }

        private void WyczyśćButton()
        {
            result = new StringBuilder();
            //wklejenie pustego stringu
            result.AppendLine(string.Empty);
            OnPropertyChanged("Result");
        }
    }
}
