using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Név_generátor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lbCsaladnevek.Background = Brushes.Yellow;
            lbUtonevek.Background = Brushes.Yellow;
            lbGeneraltNevek.Background = Brushes.Red;
            tbGeneralandoNevekSzama.Background = Brushes.LightGreen;
        }

        private void btnCsaladnevBetoltes_Click(object sender, RoutedEventArgs e)
        {
            NevBetoltes(lbCsaladnevek, lblCsaladnevDb, "Családnevek ");
        }

        private void btnUtonevBetoltes_Click(object sender, RoutedEventArgs e)
        {
            NevBetoltes(lbUtonevek, lblUtonevDb, "Utónevek ");
        }

        private void NevBetoltes(ListBox lista, Label listaDb, string szoveg)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Válassz fájlt!";
            if (ofd.ShowDialog()==true)
            {
                lista.Items.Clear();
                string[] nevek = System.IO.File.ReadAllLines(ofd.FileName);
                foreach (string nev in nevek)
                {
                    lista.Items.Add(nev);
                }
                listaDb.Content = szoveg + NevekSzama(lista);
                Frissites();
            }
            
        }

        private int NevekSzama(ListBox lista)
        {
            return lista.Items.Count;
        }

        private void Frissites()
        {
            lblCsaladnevDb.Content = "Családnevek " + NevekSzama(lbCsaladnevek);
            lblUtonevDb.Content = "Utónevek " + NevekSzama(lbUtonevek);
            int csaladnevekSzama = Convert.ToInt16(lblCsaladnevDb.ToString().Split(" ")[2]);
            int utonevekSzama = Convert.ToInt16(lblUtonevDb.ToString().Split(" ")[2]);

            if (rdoUtonevekEgy.IsChecked == true)
            {
                if (csaladnevekSzama <= utonevekSzama)
                {
                    lblMaxNev.Content = sliGeneralandoNevekSzama.Maximum = csaladnevekSzama;
                }
                else
                {
                    lblMaxNev.Content = sliGeneralandoNevekSzama.Maximum = utonevekSzama;
                }
            }
            else
            {
                if (csaladnevekSzama <= utonevekSzama / 2)
                {
                    lblMaxNev.Content = sliGeneralandoNevekSzama.Maximum = csaladnevekSzama;
                }
                else
                {
                    lblMaxNev.Content = sliGeneralandoNevekSzama.Maximum = utonevekSzama / 2;
                }
            }

            tbGeneraltNevekSzama.Text = Convert.ToString(lbGeneraltNevek.Items.Count);

        }

        private void btnGeneral_Click(object sender, RoutedEventArgs e)
        {
            Frissites();
            Random rand = new Random();
            if (rdoUtonevekEgy.IsChecked==true)
            {
                for (int i = 0; i < sliGeneralandoNevekSzama.Value; i++)
                {
                    var csaladnev = lbCsaladnevek.Items.GetItemAt(rand.Next(lbCsaladnevek.Items.Count));
                    var utonev = lbUtonevek.Items.GetItemAt(rand.Next(lbUtonevek.Items.Count));
                    lbCsaladnevek.Items.Remove(csaladnev);
                    lbUtonevek.Items.Remove(utonev);
                    lbGeneraltNevek.Items.Add(Convert.ToString(csaladnev) + " " + Convert.ToString(utonev));
                }
            }
            else
            {
                for (int i = 0; i < sliGeneralandoNevekSzama.Value; i++)
                {
                    var csaladnev = lbCsaladnevek.Items.GetItemAt(rand.Next(lbCsaladnevek.Items.Count));
                    lbCsaladnevek.Items.Remove(csaladnev);
                    var utonev1 = lbUtonevek.Items.GetItemAt(rand.Next(lbUtonevek.Items.Count));
                    lbUtonevek.Items.Remove(utonev1);
                    var utonev2 = lbUtonevek.Items.GetItemAt(rand.Next(lbUtonevek.Items.Count));
                    lbUtonevek.Items.Remove(utonev2);
                    lbGeneraltNevek.Items.Add(Convert.ToString(csaladnev) + " " + Convert.ToString(utonev1) + " " + Convert.ToString(utonev2));
                }
            }
            stbRendezes.Content = "";
            Frissites();

            lbGeneraltNevek.Items.MoveCurrentToLast();
            lbGeneraltNevek.ScrollIntoView(lbGeneraltNevek.Items.CurrentItem);
        }

        private void btnTorol_Click(object sender, RoutedEventArgs e)
        {
            foreach (var nev in lbGeneraltNevek.Items)
            {
                string[] torlendoNev = nev.ToString().Split(" ");
                lbCsaladnevek.Items.Add(torlendoNev[0]);
                lbUtonevek.Items.Add(torlendoNev[1]);
                if (torlendoNev.Length == 3)
                {
                    lbUtonevek.Items.Add(torlendoNev[2]);
                }
                lbGeneraltNevek.Items.Remove(lbGeneraltNevek.SelectedItem);
            }
            lbGeneraltNevek.Items.Clear();

            

            Frissites();
            UgrasListaAljara();
        }


        private void UgrasListaAljara()
        {
            lbCsaladnevek.Items.MoveCurrentToLast();
            lbCsaladnevek.ScrollIntoView(lbCsaladnevek.Items.CurrentItem);
            lbUtonevek.Items.MoveCurrentToLast();
            lbUtonevek.ScrollIntoView(lbUtonevek.Items.CurrentItem);
        }

        private void btnRendez_Click(object sender, RoutedEventArgs e)
        {
            stbRendezes.Content = "Rendezett névsor!";
            lbGeneraltNevek.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Content",System.ComponentModel.ListSortDirection.Ascending)); ;
        }

        private void btnMent_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "txt";
            sfd.Filter = "Szöveges fájl (*.txt) | *.txt | CSV fájl (*.csv) | *.csv | Összes fájl (*.*) | *.*";
            sfd.Title = "Adja meg a névsor nevét!";
            if (sfd.ShowDialog() == true)
            {
                string extesion = System.IO.Path.GetExtension(sfd.FileName);
                string separator = "";

                switch (extesion)
                {
                    case ".txt ":
                        separator = "\n";
                        break;
                    case ".csv":
                        separator = ";";
                        break;
                    default:
                        separator = " ";
                        break;
                }
                File.WriteAllText(sfd.FileName, Tartalom(separator));
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbGeneralandoNevekSzama.Text = Convert.ToString(sliGeneralandoNevekSzama.Value);
        }

        private void rdoUtonevekEgy_Checked(object sender, RoutedEventArgs e)
        {
            Frissites();
        }

        private void rdoUtonevekEgy_Click(object sender, RoutedEventArgs e)
        {
            Frissites();
        }

        private void tbGeneralandoNevekSzama_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbGeneralandoNevekSzama.Text != "")
            {
                sliGeneralandoNevekSzama.Value = Convert.ToInt32(tbGeneralandoNevekSzama.Text);
            }
            else
            {
                sliGeneralandoNevekSzama.Value = 0;
            }
            
            
        }

        private void lbGeneraltNevek_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lbGeneraltNevek.SelectedIndex != -1)
            {
                string[] torlendoNev = lbGeneraltNevek.SelectedItem.ToString().Split(" ");
                lbCsaladnevek.Items.Add(torlendoNev[0]);
                lbUtonevek.Items.Add(torlendoNev[1]);
                if (torlendoNev[1].Length == 3)
                {
                    lbUtonevek.Items.Add(torlendoNev[2]);
                }
                lbGeneraltNevek.Items.Remove(lbGeneraltNevek.SelectedItem);
                Frissites();
                UgrasListaAljara();
            }
        }

        private string Tartalom(string separator)
        {
            string tartalom = "";
            foreach (var nev in lbGeneraltNevek.Items)
            {
                tartalom += nev + separator;
            }
            return tartalom;
        }
    }
}
