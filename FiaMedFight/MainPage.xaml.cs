using FiaMedFight.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FiaMedFight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }


        private void SimpleDice_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var button = sender as Button;
            int result = Dice.RollAnyDice(6);
            ResultText.Text = "You rolled: " + result;
            Dice.ChangeDiceFace(button, result);
        }

        private void pointerout(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            //ResultText.Text = "previous result: " + testDice.FaceValue;
        }

        private void pointerin(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ResultText.Text = "click to roll";

        }
    }
}
