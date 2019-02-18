using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XOVO.Models
{
    public class Message
    {
        public string Header { get; set; }
        public string AdditionalHeader { get; set; }
        public string MessageText { get; set; }
        public string Solution { get; set; }


        public Message() : this("Uppss :(", "Das sollte nicht passieren! Wie arbeiten daran", "Versuchen Sie es später erneut!", "") { }
        public Message(string header, string addHeader, string message, string solution)
        {
            this.Header = header;
            this.AdditionalHeader = addHeader;
            this.MessageText = message;
            this.Solution = solution;
        }
    }
}