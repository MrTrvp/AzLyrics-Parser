using System;      
using System.Windows.Forms;
using AzLyricParser.Core.Models;
using AzLyricParser.Core.Providers;

namespace AzLyricParser.UI.Forms
{
    public partial class MainForm : Form
    {
        private readonly AzLyricProvider _provider;

        public MainForm()
        {
            _provider = new AzLyricProvider();
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbQuery.Text))
            {
                MessageBox.Show("Query is empty. Type something in.");
                return; 
            }            

            lbSongs.Items.Clear();

            const int pagesToGet = 2;
            var currentPage = 0;
                  
            SearchResult result;
            do
            {
                result = await _provider.Search(tbQuery.Text, currentPage);
                lbSongs.Items.AddRange(result.Entries);
                currentPage++;
            } while (result.EntriesFound && currentPage != pagesToGet);   
        }

        private async void lbSongs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var index = lbSongs.IndexFromPoint(e.Location);
            if (index == ListBox.NoMatches)
                return;

            var selectedSong = (lbSongs.Items[index] as Song);
            rtbLyrics.Text = await selectedSong.GetLyrics();
        }
    }
}