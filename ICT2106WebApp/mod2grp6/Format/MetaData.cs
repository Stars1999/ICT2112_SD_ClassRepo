namespace ICT2106WebApp.mod2grp6
{
    public class MetaData
    {
        private string id;
        private string fileName;
        private string lastModified;
        private string createdDate;
        private string size;

        public string GetId()
        {
            return id;
        }

        public string GetFileName()
        {
            return fileName;
        }

        public string GetLastModified()
        {
            return lastModified;
        }

        public string GetCreatedDate()
        {
            return createdDate;
        }

        public string GetSize()
        {
            return size;
        }

        public void SetId(string id)
        {
            this.id = id;
        }

        public void SetFileName(string fileName)
        {
            this.fileName = fileName;
        }

        public void SetLastModified(string lastModified)
        {
            this.lastModified = lastModified;
        }

        public void SetCreatedDate(string createdDate)
        {
            this.createdDate = createdDate;
        }

        public void SetSize(string size)
        {
            this.size = size;
        }
    }
}
