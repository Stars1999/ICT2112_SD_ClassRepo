namespace ICT2106WebApp.Services.mod2grp6
{
    public class Format
    {
        private string id;
        private string heading;
        private string paragraphs;

        /// <summary>
        /// Gets the heading identifier
        /// </summary>
        /// <returns>The heading string identifier</returns>
        public string GetHeadingId()
        {
            return id;
        }

        /// <summary>
        /// Gets the paragraphs
        /// </summary>
        /// <returns>The paragraphs string</returns>
        public string GetParagraphs()
        {
            return paragraphs;
        }

        /// <summary>
        /// Sets the heading
        /// </summary>
        /// <param name="heading">The heading string</param>
        public void SetHeading(string heading)
        {
            this.heading = heading;
        }

        /// <summary>
        /// Sets the paragraphs
        /// </summary>
        /// <param name="heading">The heading string</param>
        public void SetParagraphs(string heading)
        {
            this.paragraphs = heading;
        }
    }

}