using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

public class Docx 
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		// // Private Getters and Setters
		// protected string DocId { get; set; }
		// protected string Title { get; set; }
		// protected string FileName { get; set; }
		// protected byte[] FileData { get; set; }
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

		private void setDocId(string docId ) => Id = docId;
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
				"docxId"   => getDocId(), // string
				"title"    => getTitle(), // string
				"fileName" => getFileName(), // string
				"fileData" => getFileData(), // byte[]
				_ => throw new NotImplementedException($"Docx: Attribute '{docxAttribute}' not found")
			};
		}
	
		public Docx UpdateDocxID(Docx docx,string id)
		{
			docx.setDocId(id);
			
			return docx;
		}
	// }
	 // Required empty constructor for MongoDB
    public Docx() { }

    // Call this at application startup
// Modified registration method for Docx class
public static void RegisterMongoSerializer()
{
    // Optional: If you want to force re-registration, you could use:
    BsonSerializer.TryRegisterSerializer(new DocxSerializer());
}
}

// Simplified serializer
public class DocxSerializer : SerializerBase<Docx>
{
    public override Docx Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        Docx docx = new Docx();
        var reader = context.Reader;
        
        reader.ReadStartDocument();
        
        while (reader.ReadBsonType() != BsonType.EndOfDocument)
        {
            string name = reader.ReadName();
            switch (name)
            {
                case "_id":
                    CallPrivateSetter(docx, "setDocId", reader.ReadString());
                    break;
                case "Title":
                    CallPrivateSetter(docx, "setTitle", reader.ReadString());
                    break;
                case "FileName":
                    CallPrivateSetter(docx, "setFileName", reader.ReadString());
                    break;
                case "FileData":
                    CallPrivateSetter(docx, "setFileData", reader.ReadBytes().ToArray());
                    break;
                default:
                    reader.SkipValue();
                    break;
            }
        }
        
        reader.ReadEndDocument();
        return docx;
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Docx value)
    {
        var writer = context.Writer;
        
        writer.WriteStartDocument();
        
        string docId = CallPrivateGetter<string>(value, "getDocId");
        if (!string.IsNullOrEmpty(docId))
        {
            writer.WriteName("_id");
            writer.WriteString(docId);
        }
        
        string title = CallPrivateGetter<string>(value, "getTitle");
        if (!string.IsNullOrEmpty(title))
        {
            writer.WriteName("Title");
            writer.WriteString(title);
        }
        
        string fileName = CallPrivateGetter<string>(value, "getFileName");
        if (!string.IsNullOrEmpty(fileName))
        {
            writer.WriteName("FileName");
            writer.WriteString(fileName);
        }
        
        byte[] fileData = CallPrivateGetter<byte[]>(value, "getFileData");
        if (fileData != null)
        {
            writer.WriteName("FileData");
            writer.WriteBytes(fileData);
        }
        
        writer.WriteEndDocument();
    }
    
    // Helper methods for reflection
    private void CallPrivateSetter(Docx obj, string methodName, object value)
    {
        typeof(Docx).GetMethod(methodName, 
            BindingFlags.NonPublic | BindingFlags.Instance)
            ?.Invoke(obj, new[] { value });
    }
    
    private T CallPrivateGetter<T>(Docx obj, string methodName)
    {
        return (T)(typeof(Docx).GetMethod(methodName, 
            BindingFlags.NonPublic | BindingFlags.Instance)
            ?.Invoke(obj, null) ?? default(T));
    }
}
