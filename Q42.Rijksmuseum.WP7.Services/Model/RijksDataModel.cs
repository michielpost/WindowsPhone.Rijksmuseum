using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

namespace Q42.Rijksmuseum.WP7.Services.Model
{
    [DataContract]
    public class RijksDataModel
    {
        [DataMember]
        public string Exension { get; set; }

        [DataMember]
        public string Path { get; set; }


        [DataMember]
        public string ObjectId { get; set; }


        [DataMember]
        public string ArtistId { get; set; }

        [DataMember]
        public string ArtistName { get; set; }



        [DataMember]
        public string Link { get; set; }

        [DataMember]
        public string CreationDate { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public DateTime ReadDate { get; set; }

    }
}
