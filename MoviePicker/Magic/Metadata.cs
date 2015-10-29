using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MoviePicker.Magic
{
    [XmlType("movie")]
    public class Metadata
    {
        /*
        <movie>
          <title>13 Going On 30</title>
          <originaltitle>13 Going On 30</originaltitle>
          <id>tt0337563</id>
          <outline>After total humiliation at her thirteenth birthday party, Jenna Rink wants to just hide until she's thirty. Thanks to some wishing dust, Jenna's prayer has been answered. With a knockout body, a dream apartment, a fabulous wardrobe, an athlete boyfriend, a dream job, and superstar friends, this can't be a better life. Unfortunetly, Jenna realizes that this is not what she wanted. The only one that she needs is her childhood best friend, Matt, a boy that she thought destroyed her party. But when she finds him, he's a grown up, and not the same person that she knew.</outline>
          <year>2004</year>
          <runtime xmlns:i="http://www.w3.org/2001/XMLSchema-instance">98</runtime>
          <director>Gary Winick</director>
        </movie>
        */

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("originaltitle")]
        public string OriginalTitle { get; set; }

        [XmlElement("id")]
        public string ImdbId { get; set; }

        [XmlElement("outline")]
        public string Summary { get; set; }

        [XmlElement("year")]
        public int? Year { get; set; }

        [XmlElement("runtime")]
        public int RuntimeMinutes { get; set; }

        [XmlElement("director")]
        public string Director { get; set; }

        private static XmlSerializer serializer = new XmlSerializer(typeof(Metadata));
        public static Metadata Deserialize(FileInfo file)
        {
            try {
                using (var stream = file.OpenRead())
                {
                    return (Metadata)serializer.Deserialize(stream);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
