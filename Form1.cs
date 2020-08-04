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
using NAudio.Wave;

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
        bool isPlaying;
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
            
            for (int i = 0; i < songsToSet.Count; i++)
            {
                Button btn = songsToSet[i].setSong();
                this.Controls.Add(btn);
                btn.Click += new EventHandler(this.setButtons);
                Song.size.X += 40;
            }

        }
        private void setButtons(object sender, EventArgs e)
        {
            if (isPlaying == true) 
            {
                output.Stop();
            }
            string s = (sender as Button).Text;
            WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(songsAdded[findClickedSong(s)].path));
            stream = new BlockAlignReductionStream(pcm);
            output = new DirectSoundOut();
            output.Init(stream);
            output.Play();
            if (!isPlaying)
            {
                isPlaying = true;
            }
        } 
        private int findClickedSong(string txt)
        {
            foreach (Song song in songsAdded)
            {
                if(song.title == txt)
                {
                    return (song.thisSongNumber);
                }
            }
            return (0);
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
            if (output != null) 
            {
                output.Stop();
            }
        }
    }
}
