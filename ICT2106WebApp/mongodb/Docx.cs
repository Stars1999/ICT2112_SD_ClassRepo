using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace ICT2106WebApp.mod1Grp3
{
    public class Docx
    {
        // MongoDB Deserialization does not allow for private attributes as they are unable to access the property to re-create the object.
        // only protected can
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Title")]
        protected string Title { get; set; }
        [BsonElement("FileName")]
        protected string FileName { get; set; }
        [BsonElement("FileData")]
        protected byte[] FileData { get; set; }

        private string getDocId() => Id;
        private string getTitle() => Title;
        private string getFileName() => FileName;
        private byte[] getFileData() => FileData;

        private void setDocId(string docId) => Id = docId;
        private void setTitle(string title) => Title = title;
        private void setFileName(string filename) => FileName = filename;
        private void setFileData(byte[] fileData) => FileData = fileData;

        // Public Methods to Create Document
        public Docx CreateDocx(string title, string filename, byte[] fileData)
        {
            Docx newDocx = new Docx();
            // newDocx.setDocId(docId);
            newDocx.setTitle(title);
            newDocx.setFileName(filename);
            // newDocx.settUploadDate(uploadDate);
            newDocx.setFileData(fileData);

            return newDocx;
        }
        // Get specific docx attributes through the public method
        public object GetDocxAttributeValue(string docxAttribute) // object return type will require a typecast when calling the method
        {
            return docxAttribute switch
            {
                "docxId" => getDocId(), // string
                "title" => getTitle(), // string
                "fileName" => getFileName(), // string
                "fileData" => getFileData(), // byte[]
                _ => throw new NotImplementedException($"Docx: Attribute '{docxAttribute}' not found")
            };
        }
    }
}
