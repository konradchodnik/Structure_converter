using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WyswietlanieTextu.Model;

namespace WyswietlanieTextu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //Tak to ma dzialac na behind
        /*
        StringCollection GetLinesCollectionFromTextBox(TextBox textBox)
        {
            StringCollection lines = new StringCollection();

            var ind = lines.IndexOf(textBox.ToString());
            var n = lines.Count;
            var t = lines.IsReadOnly;
        
            int lineCount = textBox.LineCount;


            for (int line = 0; line < lineCount; line++) { lines.Add(textBox.GetLineText(line)); }

            return lines;
            textBox.Text = lines[0].ToString();
        }

        private List<string> GetLinesFromContent(string content)
        {
            content = content.Trim().Replace(" ", string.Empty);

            var lines = content.Split(';');
            return lines.ToList();
        } 

      
        private Zmienna GetVariable(string variable)
        {
            variable = variable.Trim().Replace(" ", string.Empty);

            var singlevariable = variable.Split(':');
            if (singlevariable.Count() == 2)
            {
                return new Zmienna(singlevariable[0], singlevariable[1]);
            }
            else
            {
                return null;
            }
        }
    
        private void testy_Click(object sender, RoutedEventArgs e)
        {
            string content = textBox.Text;
            var lines = GetLinesFromContent(content);

            var declarations = new List<Zmienna>();

            foreach (var line in lines)
            {
                var poczatek = line.IndexOf(@"//");
                var koniec = line.IndexOf(Environment.NewLine);
                if (poczatek == -1 || koniec == -1)
                {
                    declarations.Add(GetVariable(line));
                }
                else
                {
                    var deklaracja = line.Remove(poczatek, koniec - poczatek);
                    declarations.Add(GetVariable(deklaracja));
                }
            }

            var single = GetVariable(content);
            int licznik = 0;
            int licznik_bajtow = 0;
            foreach (var item in declarations)
            {
                if (declarations[licznik].Type == "DINT")
                {
                    var int_type32 = declarations[licznik].Type = "Int32";
              
                    textBox2.AppendText("" + Environment.NewLine);
                    textBox2.AppendText("[FieldOffset("+ licznik_bajtow +")]" + Environment.NewLine);
                    textBox2.AppendText("public " + int_type32 + " _" + declarations[licznik].Name + Environment.NewLine);
                    textBox2.AppendText("public " + int_type32 + declarations[licznik].Name + Environment.NewLine);
                    textBox2.AppendText("   get" + Environment.NewLine);
                    textBox2.AppendText("       {" + Environment.NewLine);
                    textBox2.AppendText("           return _vaule;" + Environment.NewLine);
                    textBox2.AppendText("       }" + Environment.NewLine);
                    textBox2.AppendText("   set" + Environment.NewLine);
                    textBox2.AppendText("       {" + Environment.NewLine);
                    textBox2.AppendText("           _value = value;" + Environment.NewLine);
                    textBox2.AppendText("       }" + Environment.NewLine);
                    textBox2.AppendText("" + Environment.NewLine);
                    //dodanie nowego
                    licznik_bajtow = licznik_bajtow +4;
                }
                else if (declarations[licznik].Type == "REAL")
                {
                   var float_type = declarations[licznik].Type = "float";
                
                    textBox2.AppendText("" + Environment.NewLine);
                    textBox2.AppendText("[FieldOffset(" + licznik_bajtow + ")]" + Environment.NewLine);
                    textBox2.AppendText("public " + float_type + " _" + declarations[licznik].Name + Environment.NewLine);
                    textBox2.AppendText("public " + float_type + declarations[licznik].Name + Environment.NewLine);
                    textBox2.AppendText("   get" + Environment.NewLine);
                    textBox2.AppendText("       {" + Environment.NewLine);
                    textBox2.AppendText("           return _vaule;" + Environment.NewLine);
                    textBox2.AppendText("       }" + Environment.NewLine);
                    textBox2.AppendText("   set" + Environment.NewLine);
                    textBox2.AppendText("       {" + Environment.NewLine);
                    textBox2.AppendText("           _value = value;" + Environment.NewLine);
                    textBox2.AppendText("       }" + Environment.NewLine);
                    textBox2.AppendText("" + Environment.NewLine);
                    //dodanie nowego
                    licznik_bajtow = licznik_bajtow + 4;

                }
                else if (declarations[licznik].Type == "INT")
                {
                    var int_type = declarations[licznik].Type = "Int16";
                
                    textBox2.AppendText("" + Environment.NewLine);
                    textBox2.AppendText("[FieldOffset(" + licznik_bajtow + ")]" + Environment.NewLine);
                    textBox2.AppendText("public " + int_type + " _" + declarations[licznik].Name + Environment.NewLine);
                    textBox2.AppendText("public " + int_type + declarations[licznik].Name + Environment.NewLine);
                    textBox2.AppendText("   get" + Environment.NewLine);
                    textBox2.AppendText("       {" + Environment.NewLine);
                    textBox2.AppendText("           return _vaule;" + Environment.NewLine);
                    textBox2.AppendText("       }" + Environment.NewLine);
                    textBox2.AppendText("   set" + Environment.NewLine);
                    textBox2.AppendText("       {" + Environment.NewLine);
                    textBox2.AppendText("           _value = value;" + Environment.NewLine);
                    textBox2.AppendText("       }" + Environment.NewLine);
                    textBox2.AppendText("" + Environment.NewLine);
                    //dodanie nowego
                    licznik_bajtow = licznik_bajtow + 2;
                }
                else if (declarations[licznik].Type == "UINT")
                {
                    var int_type32 = declarations[licznik].Type = "UInt16";
               
                    textBox2.AppendText("" + Environment.NewLine);
                    textBox2.AppendText("[FieldOffset(" + licznik_bajtow + ")]" + Environment.NewLine);
                    textBox2.AppendText("public " + int_type32 + " _" + declarations[licznik].Name + Environment.NewLine);
                    textBox2.AppendText("public " + int_type32 + declarations[licznik].Name + Environment.NewLine);
                    textBox2.AppendText("   get" + Environment.NewLine);
                    textBox2.AppendText("       {" + Environment.NewLine);
                    textBox2.AppendText("           return _vaule;" + Environment.NewLine);
                    textBox2.AppendText("       }" + Environment.NewLine);
                    textBox2.AppendText("   set" + Environment.NewLine);
                    textBox2.AppendText("       {" + Environment.NewLine);
                    textBox2.AppendText("           _value = value;" + Environment.NewLine);
                    textBox2.AppendText("       }" + Environment.NewLine);
                    textBox2.AppendText("" + Environment.NewLine);
                    //dodanie nowego
                    licznik_bajtow = licznik_bajtow + 2;
                }
              
                licznik++;
                if (licznik == 4)
                {
                    break;
                }
            }

        }

       */
    }
}
