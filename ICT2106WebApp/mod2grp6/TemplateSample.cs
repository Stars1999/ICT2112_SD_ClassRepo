using ICT2106WebApp.mod2grp6.Text;
using ICT2106WebApp.mod2grp6.Layout;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6.TestCase
{
    /// <summary>
    /// TemplateSample class providing specific mock data for template testing
    /// </summary>
    public class TemplateSample
    {
        // Research paper headings
        public List<AbstractNode> HeadingsAndParagraphs = new List<AbstractNode> { 
            new SimpleNode(1, "h1", "Artificial Intelligence in Modern Education", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "center" }, { "FontSize", 16 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.5x)" }, { "LineSpacingValue", 18.0 } } }),
            new SimpleNode(2, "h2", "Abstract", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 14 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 16.1 } } }),
            new SimpleNode(3, "h2", "Introduction", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 14 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 16.1 } } }),
            new SimpleNode(4, "h2", "Literature Review", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 14 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 16.1 } } }),
            new SimpleNode(5, "h3", "Historical Development", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(6, "h3", "Current Applications", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(7, "h2", "Methodology", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 14 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 16.1 } } }),
            new SimpleNode(8, "h2", "Results and Discussion", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 14 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 16.1 } } }),
            new SimpleNode(9, "h2", "Conclusion", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 14 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 16.1 } } }),
            new SimpleNode(10, "h2", "References", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 14 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 16.1 } } })
        };

        // Document layout settings
        public List<AbstractNode> LayoutContent = new List<AbstractNode> {
            new SimpleNode(1, "layout", "", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Orientation", "Portrait" }, { "PageWidth", 21 }, { "PageHeight", 29.7 }, { "ColumnNum", 1 }, { "ColumnSpacing", 1.25 }, { "Margins", new Dictionary<string, object> { { "Top", 2.54 }, { "Bottom", 2.54 }, { "Left", 2.54 }, { "Right", 2.54 }, { "Header", 1.25 }, { "Footer", 1.25 } } } } }),
            new SimpleNode(2, "page_break", "[PAGE BREAK]", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } })
        };

        // Paragraph content
        public List<AbstractNode> ParagraphContent = new List<AbstractNode>{
            // Abstract paragraph
            new SimpleNode(1, "", "This paper examines the role of artificial intelligence in modern educational systems. We explore how AI-driven tools can enhance learning experiences, personalize education, and support both educators and students. Key findings indicate that while AI offers significant benefits, challenges remain regarding implementation, ethics, and accessibility.", 
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", true }, { "Alignment", "justified" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            
            // Introduction paragraphs
            new SimpleNode(2, "paragraph", "Education systems worldwide are experiencing a significant transformation with the integration of artificial intelligence technologies. AI offers the potential to revolutionize how knowledge is delivered, assessed, and personalized for each learner. This research examines the current landscape of AI in education and explores both opportunities and challenges associated with its implementation.", 
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "justified" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            
            new SimpleNode(3, "paragraph", "The educational technology market has grown substantially in recent years, with AI-powered solutions leading much of this growth. Intelligent tutoring systems, automated assessment tools, and administrative AI applications have become increasingly common in educational institutions from primary schools to universities. Understanding the efficacy and implications of these technologies is crucial for educators, policymakers, and technology developers.", 
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "justified" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            
            // Literature Review paragraphs
            new SimpleNode(4, "paragraph", "AI in education has evolved from simple computer-assisted instruction programs in the 1960s to today's sophisticated systems that can adapt to individual learning patterns. Early applications were limited by technological constraints but established important theoretical foundations that continue to guide development today.", 
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "justified" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            
            new SimpleNode(5, "paragraph", "Current applications of AI in education span a wide range of functions. Adaptive learning platforms like Knewton and Carnegie Learning's MATHia use algorithms to tailor content to individual students' needs and abilities. Natural language processing powers tools like Grammarly for writing assistance and automated essay scoring. Computer vision enables proctoring solutions that monitor student activity during remote examinations.", 
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "justified" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            
            // Methodology paragraph
            new SimpleNode(6, "paragraph", "This study employed a mixed-methods approach combining quantitative analysis of performance data from 12 educational institutions using AI-enhanced learning systems and qualitative interviews with 45 educators and administrators. The research was conducted over an 18-month period to capture longitudinal effects of AI implementation.", 
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "justified" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            
            // Results paragraphs
            new SimpleNode(7, "paragraph", "Our findings indicate that AI-enhanced educational tools produced a measurable improvement in student performance across multiple subjects. Institutions implementing comprehensive AI solutions saw an average 18% increase in assessment scores and a 23% reduction in completion time for equivalent learning objectives.", 
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "justified" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            
            new SimpleNode(8, "paragraph", "Qualitative data revealed that educator attitudes toward AI technologies were generally positive, with 78% reporting increased job satisfaction when administrative tasks were automated. However, concerns were raised regarding over-reliance on technology, potential biases in AI systems, and the digital divide affecting equitable access.", 
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "justified" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            
            // Conclusion paragraph
            new SimpleNode(9, "paragraph", "AI technologies demonstrate significant potential to enhance educational outcomes when thoughtfully implemented. However, successful integration requires addressing ethical considerations, ensuring equitable access, and providing adequate training for educators. Future research should focus on long-term impacts and developing guidelines for responsible AI use in diverse educational settings.", 
                new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "justified" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } })
        };

        // Text content
        public List<AbstractNode> TextContent = new List<AbstractNode>{
            new SimpleNode(1, "text", "Key findings", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(2, "text", "Limitations of the study", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(3, "text", "Future implications", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(4, "text_run", "The digital divide", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", true }, { "Italic", true }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "yellow" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
        };

        // Document metadata
        public List<AbstractNode> MetadataContent = new List<AbstractNode>
        {
            new SimpleNode(1, "metadata", "templatesample", new List<Dictionary<string, object>> { new Dictionary<string, object> { 
                { "CreatedDate_Internal", "2025-03-18 09:30:00Z" }, 
                { "LastModified_Internal", "2025-03-22 14:15:00Z" }, 
                { "filename", "AI_Education_Research.docx" }, 
                { "size", "438250" },
                { "author", "J. Smith, K. Johnson, L. Chen" },
                { "keywords", "artificial intelligence, education, adaptive learning, educational technology" }
            } })
        };

// Mathematical content
        public List<AbstractNode> MathContent = new List<AbstractNode>
        {
            // Original formulas correctly formatted
            new SimpleNode(1, "math", "Performance Improvement = \\frac{Post-Test\\_Score - Pre-Test\\_Score}{Pre-Test\\_Score} \\times 100\\%", 
                new List<Dictionary<string, object>> { new Dictionary<string, object> { 
                    { "Bold", false }, { "Italic", true }, { "Alignment", "center" }, 
                    { "FontSize", 12 }, { "FontType", "Times New Roman" }, 
                    { "FontColor", "000000" }, { "Highlight", "none" }, 
                    { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } 
                } }),
                
            new SimpleNode(2, "math", "Student Engagement Index = \\sum(Time\\_Spent \\times Interaction\\_Rate \\times Completion\\_Rate)", 
                new List<Dictionary<string, object>> { new Dictionary<string, object> { 
                    { "Bold", false }, { "Italic", true }, { "Alignment", "center" }, 
                    { "FontSize", 12 }, { "FontType", "Times New Roman" }, 
                    { "FontColor", "000000" }, { "Highlight", "none" }, 
                    { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } 
                } }),
                
            new SimpleNode(3, "math", "ROI = \\frac{Educational\\_Benefit - Implementation\\_Cost}{Implementation\\_Cost}", 
                new List<Dictionary<string, object>> { new Dictionary<string, object> { 
                    { "Bold", false }, { "Italic", true }, { "Alignment", "center" }, 
                    { "FontSize", 12 }, { "FontType", "Times New Roman" }, 
                    { "FontColor", "000000" }, { "Highlight", "none" }, 
                    { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } 
                } }),

            // Additional complex formulas
            new SimpleNode(4, "math", "\\frac{1}{\\sqrt{2\\pi}\\sigma} e^{-\\frac{(x-\\mu)^2}{2\\sigma^2}}", 
                new List<Dictionary<string, object>> { new Dictionary<string, object> { 
                    { "Bold", false }, { "Italic", true }, { "Alignment", "center" }, 
                    { "FontSize", 12 }, { "FontType", "Times New Roman" }, 
                    { "FontColor", "000000" }, { "Highlight", "none" }, 
                    { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } 
                } }),
                
            new SimpleNode(5, "math", "\\int_{a}^{b} f(x) dx = F(b) - F(a)", 
                new List<Dictionary<string, object>> { new Dictionary<string, object> { 
                    { "Bold", false }, { "Italic", true }, { "Alignment", "center" }, 
                    { "FontSize", 12 }, { "FontType", "Times New Roman" }, 
                    { "FontColor", "000000" }, { "Highlight", "none" }, 
                    { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } 
                } })
        };

        // Lists
        public List<AbstractNode> Lists = new List<AbstractNode>
        {
            new SimpleNode(1, "bulleted_list", "Intelligent Tutoring Systems", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(2, "bulleted_list", "Automated Assessment Tools", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(3, "bulleted_list", "Natural Language Processing Applications", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(4, "bulleted_list", "Predictive Analytics for Student Success", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(5, "numbered_list", "Improved learning outcomes", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(6, "numbered_list", "Reduction in administrative workload", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(7, "numbered_list", "Enhanced personalization of education", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(8, "numbered_list", "Timely intervention for struggling students", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } })
        };

        // Figures/Images
        public List<AbstractNode> Images = new List<AbstractNode>
        {
            new SimpleNode(1, "image", "Figure_1_AI_Learning_Model.png", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Alignment", "Center" }, { "Format", "PNG" }, { "Position", "Inline" }, { "WidthEMU", 4500000 }, { "HeightEMU", 3000000 }, { "Caption", "Figure 1: Conceptual Model of AI-Enhanced Learning" } } }),
            new SimpleNode(2, "image", "Figure_2_Performance_Comparison.png", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Alignment", "Center" }, { "Format", "PNG" }, { "Position", "Inline" }, { "WidthEMU", 5000000 }, { "HeightEMU", 3500000 }, { "Caption", "Figure 2: Performance Comparison Between Traditional and AI-Enhanced Learning Groups" } } })
        };

        // Bibliography content
        public List<AbstractNode> BibliographyContent = new List<AbstractNode>
        {
            new SimpleNode(1, "bibliography", "Baker, R. S. (2024). Educational data mining: An advancing field. Journal of Educational Technology, 52(3), 425-438.", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(2, "bibliography", "Chen, L., & Kim, J. (2024). Artificial intelligence in personalized learning: A systematic review. Educational Psychology Review, 36(2), 189-215.", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(3, "bibliography", "Johnson, K., & Smith, J. (2023). Ethical considerations in AI-based educational systems. AI & Society, 38(4), 742-758.", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(4, "bibliography", "Liu, M., & Anderson, T. (2024). The impact of intelligent tutoring systems on mathematics education. Computers & Education, 175, 104356.", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } }),
            new SimpleNode(5, "bibliography", "Williams, P., & Garcia, R. (2024). Addressing equity in AI-powered education tools. International Journal of Educational Technology, 15(2), 210-227.", new List<Dictionary<string, object>> { new Dictionary<string, object> { { "Bold", false }, { "Italic", false }, { "Alignment", "left" }, { "FontSize", 12 }, { "FontType", "Times New Roman" }, { "FontColor", "000000" }, { "Highlight", "none" }, { "LineSpacingType", "Multiple (1.15x)" }, { "LineSpacingValue", 13.8 } } })
        };
    }
}