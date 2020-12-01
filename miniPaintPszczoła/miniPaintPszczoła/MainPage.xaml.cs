using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace miniPaintPszczoła
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool czyRysuje = false;
        Point pktStartowy;
        SolidColorBrush pisak =new SolidColorBrush(Windows.UI.Colors.Black);
        Windows.UI.Xaml.Shapes.Line tmpKreska = null;
        Stack<Line> listaUndo = new Stack<Line>();
    
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void RysujLinie(PointerRoutedEventArgs e)
        {
            if (czyRysuje)
            {
               
                Point pktAkt = e.GetCurrentPoint(polerysowania).Position;

                Line kreska = new Line()
                {
                    Stroke = pisak,
                    StrokeThickness = sldGrubość.Value,
                    X2 = pktAkt.X,
                    Y2 = pktAkt.Y,
                    //teraz poczatek lini
                    X1 = pktStartowy.X,
                    Y1 = pktStartowy.Y,
                    StrokeEndLineCap = PenLineCap.Round,
                    StrokeStartLineCap=PenLineCap.Round
                
                };

                polerysowania.Children.Add(kreska);

                if (rdbProsta.IsChecked == true)
                {
                    if (tmpKreska != null)
                    {
                        polerysowania.Children.Remove(tmpKreska);
                    }
                    tmpKreska = kreska;

                }
                else if (rdbDowolna.IsChecked == true)
                {
                    pktStartowy = pktAkt;
                    tmpKreska = kreska;
                    
                }  
                
            }
        }
        private void polerysowania_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            RysujLinie(e);
            
        }

        private void polerysowania_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            czyRysuje = true;
            //punkt poczatkowy
            pktStartowy= e.GetCurrentPoint(polerysowania).Position;
            
        }

        private void polerysowania_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            czyRysuje = false;
            if (rdbProsta.IsChecked == true)
            {
                listaUndo.Push(tmpKreska);
            }
            else if(rdbDowolna.IsChecked == true)
            {
                listaUndo.Push(tmpKreska);
            }
            tmpKreska = null;
        }

        private void kolorCzerwony(object sender, PointerRoutedEventArgs e)
        {
            pisak = new SolidColorBrush(Windows.UI.Colors.Red);
        }

        private void kolorCzarny(object sender, PointerRoutedEventArgs e)
        {
            pisak = new SolidColorBrush(Windows.UI.Colors.Black);
        }

        private void kolorZielony(object sender, PointerRoutedEventArgs e)
        {
            pisak = new SolidColorBrush(Windows.UI.Colors.Green);
        }

        private void kolorNiebieski(object sender, PointerRoutedEventArgs e)
        {
            pisak = new SolidColorBrush(Windows.UI.Colors.Blue);
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            if(listaUndo.Count > 0)
            {
                Shape undo = listaUndo.Pop();
                polerysowania.Children.Remove(undo);
            }
        }
   
        private async void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog dlg = new MessageDialog("Czy chcesz zakończyć program", "KONIEC");
            dlg.Commands.Add(new UICommand { Label = "tak", Id = 1 });
            dlg.Commands.Add(new UICommand { Label = "nie", Id = 0 });
            dlg.DefaultCommandIndex = 0;
            var response = await dlg.ShowAsync();
            if ((int)response.Id == 1)
            {
                Application.Current.Exit();
            }
        }
    }
}
