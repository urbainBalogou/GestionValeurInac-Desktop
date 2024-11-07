using gvi.Data;
using gvi.Models;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Paragraph = iTextSharp.text.Paragraph;
using ClosedXML.Excel;

namespace gvi.PrintForms
{
    /// <summary>
    /// Logique d'interaction pour SortiePrint.xaml
    /// </summary>
    public partial class SortiePrint : Window
    {
        DataContext _context;
        public SortiePrint()
        {
            InitializeComponent();
            _context = new DataContext();
            LoadCommune();
        }

        private void ExportToPDF_Click(object sender, RoutedEventArgs e)
        {
            if (comboCommune.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez selectionnez une commune dans la liste et spécifier les dates", "ERREUR IMPRESSION", MessageBoxButton.OK);
                return;
            }
            int communeId = (comboCommune.SelectedItem as Commune)?.Id ?? 0;
            DateTime startDate = datePickerStart.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = datePickerEnd.SelectedDate ?? DateTime.MaxValue;

            // Filtrer les données
            var sorties = GetFilteredData(communeId, startDate, endDate);

            // Exporter en PDF
            ExportToPDF(sorties, comboCommune.Text, startDate, endDate);
            comboCommune.SelectedIndex = -1;
            datePickerEnd = null;
            datePickerStart = null;
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            if(comboCommune.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez selectionnez une commune dans la liste et spécifier les dates","ERREUR IMPRESSION",MessageBoxButton.OK);
                return;
            }
            int communeId = (comboCommune.SelectedItem as Commune)?.Id ?? 0;
            DateTime startDate = datePickerStart.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = datePickerEnd.SelectedDate ?? DateTime.MaxValue;

            // Filtrer les données
            var sorties = GetFilteredData(communeId, startDate, endDate);

            // Exporter en Excel
            ExportToExcel(sorties, comboCommune.Text, startDate, endDate);
        }
        void LoadCommune()
        {
            var communes = _context.Communes.ToList();
            comboCommune.ItemsSource = communes;
        }
        public List<Sortie> GetFilteredData(int communeId, DateTime startDate, DateTime endDate)
        {
            // Filtre les sorties par commune et date
            return _context.Sorties
                   .Where(s => s.CommuneId == communeId &&
                               s.DateSortie >= startDate && s.DateSortie <= endDate)
                   .Include(s => s.Valeurs) // Inclure la collection SortieValeur
                       .ThenInclude(sv => sv.Valeur) // Inclure la relation vers Valeur
                           .ThenInclude(v => v.TypeValeur)// Inclure la relation vers TypeValeur
                            .AsEnumerable()
                            
                   .ToList();
        }


            public void ExportToPDF(List<Sortie> sorties, string communeNom, DateTime startDate, DateTime endDate)
    {
        string filePath = ShowSaveFileDialog("PDF Files|*.pdf");

        if (string.IsNullOrEmpty(filePath))
            return;

        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        using (Document document = new Document())
        {
            PdfWriter.GetInstance(document, fs);
            document.Open();

            // Titre principal
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            document.Add(new Paragraph($"Liste des sorties pour {communeNom} : Période du {startDate:dd/MM/yyyy} au {endDate:dd/MM/yyyy}", titleFont));

            // Tableau de sorties
            PdfPTable table = new PdfPTable(3) { WidthPercentage = 100 };
                table.AddCell("Valeur");
                table.AddCell("Date Sortie");
            
                table.AddCell("Quantité");

            foreach (var sortie in sorties)
            {
                foreach (var valeur in sortie.Valeurs)
                {
                    table.AddCell(valeur.Valeur?.Nom?.ToString() ?? "N/A");
                    table.AddCell(sortie.DateSortie.ToString("dd/MM/yyyy"));
                    
                    table.AddCell(valeur.Quantite.ToString());
                }
            }

            document.Add(table);
            document.Close();
        }
    }
        public string ShowSaveFileDialog(string filter)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                Filter = filter
            };

            bool? result = dlg.ShowDialog();
            return result == true ? dlg.FileName : string.Empty;
        }
        public void ExportToExcel(List<Sortie> sorties, string communeNom, DateTime startDate, DateTime endDate)
        {
            string filePath = ShowSaveFileDialog("Excel Files|*.xlsx");

            if (string.IsNullOrEmpty(filePath))
                return;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sorties");

                // En-tête
                worksheet.Cell(1, 1).Value = $"Liste des sorties pour {communeNom} : Période du {startDate:dd/MM/yyyy} au {endDate:dd/MM/yyyy}";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Range("A1:D1").Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Titres des colonnes
                
                worksheet.Cell(2, 2).Value = "Date Sortie";
                worksheet.Cell(2, 3).Value = "Valeur";
                worksheet.Cell(2, 4).Value = "Quantité";

                int row = 3;

                foreach (var sortie in sorties)
                {
                    foreach (var valeur in sortie.Valeurs)
                    {
                      
                        worksheet.Cell(row, 2).Value = sortie.DateSortie.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 3).Value = valeur.Valeur?.Nom?.ToString() ?? "N/A";
                        worksheet.Cell(row, 4).Value = valeur.Quantite;
                        row++;
                    }
                }

                // Enregistre le fichier
                workbook.SaveAs(filePath);
            }
        }
    }
}
