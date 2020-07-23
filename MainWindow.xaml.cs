using System;
using System.Collections.Generic;
using ProjektHR.Models;

namespace ProjektHR
{
    /// <summary>
    /// Klasa okna głównego MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ApplicationDbContext dbcontext = new ApplicationDbContext();  //utworzenie obiektu bazy danych
        CollectionViewSource wyplatasViewSource; //utworzenie kolekcji źródłowej dla wypłat
        CollectionViewSource pracowniksViewSource; //utworzenie kolekcji źródłowej dla pracowników
        public MainWindow()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Kod wykonywany podczas ładowania okna
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dbcontext.Pracownicy.Load();  //załadowanie listy pracowników do obiektu bazy
            dbcontext.Umowy.Load();  //załadowanie listy umów do obiektu bazy
            dbcontext.Wyplaty.Load();  //załadowanie listy wypłat do obiektu bazy

            //stworzenie tabeli i podpięcie źródła do tabeli pracowników
            ProjektHR.DefConnPracDataSet defConnPracDataSet = ((ProjektHR.DefConnPracDataSet)(this.FindResource("defConnPracDataSet")));
            ProjektHR.DefConnPracDataSetTableAdapters.PracowniksTableAdapter defConnPracDataSetPracowniksTableAdapter = new ProjektHR.DefConnPracDataSetTableAdapters.PracowniksTableAdapter();
            defConnPracDataSetPracowniksTableAdapter.Fill(defConnPracDataSet.Pracowniks);
            pracowniksViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pracowniksViewSource")));
            pracowniksViewSource.Source = dbcontext.Pracownicy.Local; 
            //stworzenie tabeli i podpięcie źródła do tabeli pracowników

            //stworzenie tabeli i podpięcie źródła do tabeli umów
            ProjektHR.DefConnPracDataSetTableAdapters.UmowasTableAdapter defConnPracDataSetUmowasTableAdapter = new ProjektHR.DefConnPracDataSetTableAdapters.UmowasTableAdapter();
            defConnPracDataSetUmowasTableAdapter.Fill(defConnPracDataSet.Umowas);
            System.Windows.Data.CollectionViewSource umowasViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("umowasViewSource")));
            umowasViewSource.Source = dbcontext.Umowy.Local; 
            //stworzenie tabeli i podpięcie źródła do tabeli umów


            //stworzenie tabeli wypłat
            ProjektHR.DefConnPracDataSetTableAdapters.WyplatasTableAdapter defConnPracDataSetWyplatasTableAdapter = new ProjektHR.DefConnPracDataSetTableAdapters.WyplatasTableAdapter();
            defConnPracDataSetWyplatasTableAdapter.Fill(defConnPracDataSet.Wyplatas);
            wyplatasViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("wyplatasViewSource")));
            wyplatasViewSource.Source = null; 
            //stworzenie tabeli wypłat

            CbPracownik.ItemsSource = dbcontext.Pracownicy.Local;//załadowanie pracowników do comboboxa
            CbUmowa.ItemsSource = dbcontext.Umowy.Local; //załadowanie umów do comboboxa

            List<string> miesiace = new List<string>() { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };
            CbMiesiac.ItemsSource = miesiace; //załadowanie miesięcy do comboboxa

            List<string> lata = new List<string>() { "2020", "2021", "2022", "2023", "2024", "2025", "2026", "2027", "2028", "2029", "2030" };
            CbRok.ItemsSource = lata; //załadowanie lat do comboboxa


        }

        /// <summary>
        /// Kod wykonywany przy dodawaniu nowego pracownika
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void BtDodajPracownika_Click(object sender, RoutedEventArgs e)
        {
            if (TbPracownikImie.Text != "" && TbPracownikNazwisko.Text != "")
            {
                if (Models.Pracownik.WalidacjaPesel(TbPracownikPesel.Text)) //walidacja numeru PESEL
                {
                    MessageBoxResult result = MessageBox.Show("Czy zapisać pracownika " + TbPracownikImie.Text + " " + TbPracownikNazwisko.Text + "?", "Zapis", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            Pracownik.DodajPracownika(TbPracownikImie.Text, TbPracownikNazwisko.Text, TbPracownikPesel.Text, TbPracownikAdres.Text, TbPracownikTelefon.Text, decimal.Parse(TbPracownikStawka.Text), CbUmowa.SelectedItem as Umowa, dbcontext); // dodanie pracownika
                            TbPracownikImie.Clear(); TbPracownikAdres.Clear(); TbPracownikPesel.Clear(); TbPracownikTelefon.Clear(); TbPracownikNazwisko.Clear(); TbPracownikStawka.Clear(); CbUmowa.SelectedIndex = -1; //czyszczenie pól po zapisie
                        }
                        catch (FormatException ex)
                        {
                            MessageBox.Show("Popraw format danych!"); //komunikat jeśli wprowadzono niepoprawny format danych
                        }                        
                    }
                }
                else
                {
                    MessageBox.Show("Podaj poprawny numer PESEL!");
                }
            }
            else
            {
                MessageBox.Show("Podaj imię i nazwisko!");
            }
        }
        /// <summary>
        /// Kod wykonywany przy dodawaniu nowej umowy
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void BtDodajUmowe_Click(object sender, RoutedEventArgs e)
        {
            if (TbUmowaNazwa.Text != "" && TbUmowaUbEmryt.Text != "" && TbUmowaUbRent.Text != "" && TbUmowaUbChor.Text != "" && TbUmowaUbWypadk.Text != "" && TbUmowaFP.Text != "" && TbUmowaFGSP.Text != "") //sprawdzanie czy wszystkie pola są wypełnione
            {
                
                    MessageBoxResult result = MessageBox.Show("Czy zapisać umowę " + TbUmowaNazwa.Text + "?", "Zapis", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        Umowa.DodajUmowe(TbUmowaNazwa.Text, decimal.Parse(TbUmowaUbEmryt.Text), decimal.Parse(TbUmowaUbRent.Text), decimal.Parse(TbUmowaUbChor.Text), decimal.Parse(TbUmowaUbWypadk.Text), decimal.Parse(TbUmowaFP.Text), decimal.Parse(TbUmowaFGSP.Text), dbcontext); //dodanie umowy
                        TbUmowaNazwa.Clear(); TbUmowaUbEmryt.Clear(); TbUmowaUbRent.Clear(); TbUmowaUbChor.Clear(); TbUmowaUbWypadk.Clear(); TbUmowaFP.Clear(); TbUmowaFGSP.Clear(); //czyszczenie pól
                    }
                    catch (FormatException ex)
                    {
                        MessageBox.Show("Popraw format danych!"); //komunikat jeśli wprowadzono niepoprawny format danych
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("Podaj wszystkie potrzebne dane!");
            }
        }
        /// <summary>
        /// Kod wykonywany przy dodawaniu nowej wypłaty
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void BtDodajWyplate_Click(object sender, RoutedEventArgs e)
        {
            if (CbPracownik.SelectedIndex < 0) //sprawdzenie czy wybrano pracownika z listy
                MessageBox.Show("Wybierz pracownika!");
            else
            {
                if (TbPrLbGodz.Text == "")
                    MessageBox.Show("Podaj liczbę godzin!");
                else
                {
                    if (CbMiesiac.SelectedIndex < 0)
                        MessageBox.Show("Wybierz miesiąc!");
                    else
                    {
                        if (CbRok.SelectedIndex < 0)
                        {
                            MessageBox.Show("Wybierz rok!");
                        }
                        else
                        {
                            try
                            {
                                Wyplata.DodajWyplate(CbMiesiac.Text + " " + CbRok.Text, decimal.Parse(TbPrLbGodz.Text), CbPracownik.SelectedItem as Pracownik, dbcontext); //dodanie wypłaty
                                wyplatasDataGrid.ItemsSource = dbcontext.Wyplaty.Local.Where(w => w.Pracownik.Equals(CbPracownik.SelectedItem)); //przeładowanie danych
                                wyplatasDataGrid.Items.Refresh();
                                CbRok.SelectedItem = null; CbMiesiac.SelectedItem = null; TbPrLbGodz.Clear(); //czyszczenie pól
                            }
                            catch (FormatException ex)
                            {
                                MessageBox.Show("Popraw format danych!");  //komunikat jeśli wprowadzono niepoprawny format danych
                            }
                            
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Kod wykonywany po zmianie pracownika w comboboxie
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void CbPracownik_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            wyplatasDataGrid.ItemsSource = dbcontext.Wyplaty.Local.Where(w => w.Pracownik.Equals(CbPracownik.SelectedItem)).ToList(); //przeładowanie danych
            wyplatasDataGrid.Items.Refresh();
        }
        /// <summary>
        /// Kod wykonywany przy usuwaniu wypłaty
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void BtUsunWyplate_Click(object sender, RoutedEventArgs e)
        {
            Wyplata w1 = (Wyplata)wyplatasDataGrid.SelectedItem; //pobranie obiekty wypłaty z tabeli
            if (w1 != null)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Czy usunąć wypłatę dla pracownika " + w1.Pracownik.Imie + " " + w1.Pracownik.Nazwisko + "?", "Usuwanie wypłaty", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Wyplata.UsunWyplate(w1, dbcontext); //usunięcie wypłaty
                    wyplatasDataGrid.ItemsSource = dbcontext.Wyplaty.Local.Where(w => w.Pracownik.Equals(CbPracownik.SelectedItem)).ToList();//przeładowanie danych
                    wyplatasDataGrid.Items.Refresh();
                }
            }           
        }
        /// <summary>
        /// Kod wykonywany przy usuwaniuo pracownika
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void BtUsunPracownika_Click(object sender, RoutedEventArgs e)
        {
            Pracownik p1 = (Pracownik)pracowniksDataGrid.SelectedItem;//pobranie obiekty pracownika z tabeli
            if (p1 != null)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Czy usunąć pracownika " + p1.Imie + " " + p1.Nazwisko + "?", "Usuwanie pracownika", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if (dbcontext.Wyplaty.Local.Where(w => w.IdPracownik.Equals(p1.Id)).Any())//sprawdzenie czy pracownik nie ma żadnej zaksięgowanej wypłaty
                        MessageBox.Show("Nie można usunąć pracownika który posiada wypłatę!");
                    else
                    {
                        Pracownik.UsunPracownika(p1, dbcontext);//usuniecie pracownika
                    }
                    
                }
            }
            pracowniksDataGrid.Items.Refresh();
        }
        /// <summary>
        /// Kod wykonywany przy usuwaniu umowy
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void BtUsunUmowe_Click(object sender, RoutedEventArgs e)
        {
            Umowa u1 = (Umowa)umowasDataGrid.SelectedItem; //pobranie obiektu umowy z tabeli
            if (u1 != null)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Czy usunąć umowę " + u1.NazwaUmowy + "?", "Usuwanie umowy", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if (dbcontext.Pracownicy.Local.Where(u => u.IdUmowa.Equals(u1.Id)).Any())//sprawdzenie czy umowa nie jest przypisana do jakiegokolwiek pracownikay
                        MessageBox.Show("Nie można usunąć umowy przypisanej do pracownika!");
                    else
                    {
                        Umowa.UsunUmowe(u1, dbcontext);//usunięcie umowy
                    }
                    
                }
            }
            umowasDataGrid.Items.Refresh();
        }
        /// <summary>
        /// Kod wykonywany przy zamykaniu aplikacji
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        /// <summary>
        /// Kod wykonywany przy minimalizowaniu aplikacji
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void BtMinimalizuj_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        /// <summary>
        /// Kod wykonywany przy maksymalizowaniu aplikacji
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void BtMaksymalizuj_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }
        /// <summary>
        /// Kod wykonywany przy wyszukiwaniu nowego pracownika
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void BtWyszukajPracownika_Click(object sender, RoutedEventArgs e)
        {
            pracowniksViewSource.Source = Pracownik.SzukajPracownika(TbPracownikImie.Text, TbPracownikNazwisko.Text, TbPracownikPesel.Text, TbPracownikAdres.Text, TbPracownikTelefon.Text, CbUmowa.SelectedItem as Umowa, dbcontext).ToList();//przeładowanie danych
        }
        /// <summary>
        /// Kod wykonywany pp kliknięciu 'Pokaż wszystkich'
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void BtPokazWszystkichPracownikow_Click(object sender, RoutedEventArgs e)
        {
            pracowniksViewSource.Source = dbcontext.Pracownicy.Local;//przeładowanie danych
        }
        /// <summary>
        /// Kod wykonywany po edycji danych w tabeli Pracownicy
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void pracowniksDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            dbcontext.SaveChanges();//zapis zmian
        }
        /// <summary>
        /// Kod wykonywany po edycji danych w tabeli Wypłaty
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void wyplatasDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            dbcontext.SaveChanges();//zapis zmian
        }
        /// <summary>
        /// Kod wykonywany po edycji danych w tabeli Umowy
        /// </summary>
        /// <param name="sender">zawiera referencję do controlki/obiektu zdarzenia</param>
        /// <param name="e">zawiera informacje zdarzenia</param>
        private void umowasDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            dbcontext.SaveChanges();//zapis zmian
        }
    }
    
    
}
