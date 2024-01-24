using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chat4_Prototype
{
    internal class Message
    {
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Text { get; set; }

        public DateTime Stime { get; set; }


        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        //
        public static Message? FromJson(string somemessage)
        {
            return JsonSerializer.Deserialize<Message>(somemessage);
        }

        //Prototype
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
        //

        public Message(string nikname, string text)
        {
            this.FromName = nikname;
            this.Text = text;
            this.Stime = DateTime.Now;
        }
        public Message() { }

        public override string ToString()
        {
            return $"Получено сообщение от {FromName} ({Stime.ToShortTimeString()}): \n {Text}";
        }
    }
}
