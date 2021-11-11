using System;
using System.Collections.Generic;
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

namespace _1uzduotis
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

        //event handleris paspaudus skaiciuoti mygtuka
        private void Skaiciuoti(object sender, RoutedEventArgs e)
        {
            if (ilgisBox.Text != "" && plotisBox.Text != "" && kainaBox.Text != "")
            {
                double suma, ilgis, plotis, kaina;
                ilgis = Convert.ToDouble(ilgisBox.Text);
                plotis = Convert.ToDouble(plotisBox.Text);
                kaina = Convert.ToDouble(kainaBox.Text);
                suma = ilgis * plotis * kaina;
                MessageBox.Show(suma.ToString());
            }
            else
            {
                MessageBox.Show("Neuzpildyti visi laukeliai!");
            }
        }

        //event handleris paspaudus isvalyti laukus mygtuka
        private void ValytiLaukus(object sender, RoutedEventArgs e)
        {
            ilgisBox.Clear();
            plotisBox.Clear();
            kainaBox.Clear();
        }


        //tikrina ar duotas string gali buto convertinamas i skaiciu
        private bool ArSkaicius(string eilute)
        {
            bool arSkaicius = double.TryParse(eilute, out double sk);

            return arSkaicius;
        }

        //--------------event handleris kuris neleidzia rasyti raidziu i textbox--------
        private void ilgisBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!ArSkaicius(ilgisBox.Text + e.Text))
            {
                e.Handled = true;
            }

        }
        private void plotisBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!ArSkaicius(plotisBox.Text + e.Text))
            {
                e.Handled = true;
            }
        }
        private void kainaBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!ArSkaicius(kainaBox.Text + e.Text))
            {
                e.Handled = true;
            }
        }
        //----------------------------------------------------------------------------

        //event handleris priziurintis textboxu pasikeitimus
        private void SkaiciuotiPasikeitus(object sender, TextChangedEventArgs e)
        {
            if (ilgisBox.Text != "" && plotisBox.Text != "" && kainaBox.Text != "") //jei visuose textboxuose yra kasnors ivesta, suskaiciuojama ir isvedama suma
            {
                double suma, ilgis, plotis, kaina;
                ilgis = Convert.ToDouble(ilgisBox.Text);
                plotis = Convert.ToDouble(plotisBox.Text);
                kaina = Convert.ToDouble(kainaBox.Text);
                suma = ilgis * plotis * kaina;
                label3.Content = $"Is viso kainuos: {suma}EU";
            }
            else
            {
                label3.Content = "";
            }
        }

        private void Skaiciuoti19m(object sender, RoutedEventArgs e)
        {
            if (ilgisBox.Text != "" && plotisBox.Text != "" && kainaBox.Text != "")
            {
                double suma, ilgis, plotis, kaina;
                double kilimoIlgis = 0;
                double kilimoPlotis = 19.0;
                ilgis = Convert.ToDouble(ilgisBox.Text);
                plotis = Convert.ToDouble(plotisBox.Text);
                kaina = Convert.ToDouble(kainaBox.Text);

                //patikriname kuriuo atveju maziau kilimo reik palikti
                switch (PasirinktiKaipKlotiKilima(ilgis, plotis, kilimoPlotis))
                {
                    case 0: //kai apsimoka kilima tiesti isilgai kambario
                        kilimoIlgis = IsmatuotiKilima(ilgis, plotis, kilimoPlotis);
                        break;
                    case 1: //kai apsimoka kilima tiesti skersai kambario
                        kilimoIlgis = IsmatuotiKilima(plotis, ilgis, kilimoPlotis);
                        break;
                    default:
                        MessageBox.Show("Ivyko klaida");
                        break;
                }

                //apskaiciuojame kiek kainuos kilimas
                suma = kilimoIlgis * kilimoPlotis * kaina;
                double nepanaudotas = (kilimoIlgis * kilimoPlotis) - ilgis * plotis;

                label4.Content = $"Norint uzkloti kambari, jums reikes {kilimoIlgis}m. kilimo\nKilimas kainuos {suma} Eu\nNepanaudota liko {nepanaudotas}m2";
            }
            else
            {
                MessageBox.Show("Neuzpildyti visi laukeliai!");
            }
        }

        private int PasirinktiKaipKlotiKilima(double ilgis, double plotis, double kilimoPlotis)
        {
            if (ilgis % kilimoPlotis > plotis % kilimoPlotis)   //kai reikia maziau nukirpti naudojant ploti
            {
                return 0;
            }
            else if (ilgis % kilimoPlotis < plotis % kilimoPlotis) //kai reikia maziau nukirpti naudojant ilgi
            {
                return 1;
            }
            else if (ilgis % kilimoPlotis == plotis % kilimoPlotis && ilgis > plotis)              //kai skirtumas lygus nuliui, bet ilgis > plotis
            {
                return 0;
            }
            else if (ilgis % kilimoPlotis == plotis % kilimoPlotis && ilgis < plotis)           //kai skirtumas lygus nuliui, bet plotis>ilgi
            {
                return 1;
            }
            else        // kai kambarys kvadrato formos (ilgis = plotis)
            {
                return 0;
            }
        }

        private double IsmatuotiKilima(double ilgis, double plotis, double kilimoPlotis)
        {
            double kilimoIlgis, likucioIlgis;
            double sveikosLinijos = 0;

            //suskaiciuoja kiek sveiku kilimo liniju reikes, ir atima jau uzklota plota is bendro ploto
            sveikosLinijos = Math.Floor(plotis / kilimoPlotis);
            plotis = plotis - (sveikosLinijos * kilimoPlotis);
            kilimoIlgis = sveikosLinijos * ilgis;

            likucioIlgis = IsmatuotiKilimaUzklotiLikuciui(ilgis, plotis, kilimoPlotis);

            return kilimoIlgis + likucioIlgis;


        }

        private double IsmatuotiKilimaUzklotiLikuciui(double ilgis, double plotis, double kilimoPlotis)
        {
            if (ilgis > plotis)
            {
                if (ilgis % kilimoPlotis == 0)
                {
                    return plotis * (ilgis / kilimoPlotis);
                }
                else
                {
                    return plotis * (Math.Floor(ilgis / kilimoPlotis) + 1);
                }
            }
            else
            {
                return ilgis;
            }
        }
    }
}
