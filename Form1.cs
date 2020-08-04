using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace ñeñePlayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        List<Song> songsAdded = new List<Song>();

        private NAudio.Wave.BlockAlignReductionStream stream = null;
        private NAudio.Wave.DirectSoundOut output = null;

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                string folder = folderDialog.SelectedPath;
                List<string> files = new List<string>(Directory.GetFiles(folder));
                createSongs(Purify(files));
            }
        }
        private void setSongs(List<Song> songsToSet)
        {
            Button b1 = songsToSet[0].setSong();
            this.Controls.Add(b1);
            b1.Click += new EventHandler(this.setButtons);
        }
        private void setButtons(object sender, EventArgs e)
        {
            NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(songsAdded[0].path));
            stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
            output = new NAudio.Wave.DirectSoundOut();
            output.Init(stream);
            output.Play();
        }
        private void createSongs(List<string> songsToCreate)
        {
            foreach (string arg in songsToCreate)
            {
                Song songToAdd = new Song(Path.GetFileNameWithoutExtension(arg), "a", arg);
                songsAdded.Add(songToAdd);
            }
            setSongs(songsAdded);
        }

        private List<string> Purify(List<string> songsToCleanse)
        {
            foreach (string arg in songsToCleanse.ToList())
            {
                if (!arg.EndsWith(".mp3"))
                {
                    songsToCleanse.Remove(arg);
                }
            }
            return (songsToCleanse);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                output.Stop();
            }
            catch
            {
                this.Close();
            }
        }
    }
}
