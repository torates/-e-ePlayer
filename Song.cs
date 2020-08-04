using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ñeñePlayer
{
    class Song
    {
        public string title;
        public string author;
        public string path;
        private static int count = 0;
        public Button thisSongBtn;
        public string btnName;
        public Song(string title, string author, string path)
        {
            this.title = title;
            this.author = author;
            this.path = path;
        }
        public Button setSong()
        {
            Button song = new Button();
            song.Text = this.title;
            song.Location = new Point(50, 50);
            song.Name = "song " + count.ToString();
            btnName = song.Name;
            thisSongBtn = song;
            count++;
            return(song);
        }
    }
}
