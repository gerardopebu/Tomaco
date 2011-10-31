using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TomacoServer.Infrastructure.UI
{
    public class TextBoxStreamWriter:TextWriter
    {
        TextBox _textBox;

        public TextBoxStreamWriter(TextBox textBox)
        {
            _textBox=textBox;
        }

        public override void Write(char value)
        {
            base.Write(value);
            _textBox.AppendText(value.ToString());
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
