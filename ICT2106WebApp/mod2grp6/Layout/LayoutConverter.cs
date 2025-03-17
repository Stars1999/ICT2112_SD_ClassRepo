using System.Collections.Generic;
using ICT2106WebApp.Utilities;

namespace ICT2106WebApp.mod2grp6.Layout
{
    public class LayoutConverter
    {
        private LayoutManager layoutManager;

        public LayoutConverter()
        {
            layoutManager = new LayoutManager();
        }

        // Update to use List<AbstractNode>
        public bool ConvertLayout(List<AbstractNode> content)
        {
            // Initialize layout processing
            if (layoutManager.StartLayoutFormatting(content))
            {
                // Process all layout components
                bool headersFormatted = layoutManager.FormatHeaders();
                bool footersFormatted = layoutManager.FormatFooters();
                bool marginsFormatted = layoutManager.FormatMargins();
                bool orientationFormatted = layoutManager.FormatOrientation();
                bool pageSizeFormatted = layoutManager.FormatPageSize();
                bool columnNumFormatted = layoutManager.FormatColumnNum();
                bool columnSpacingFormatted = layoutManager.FormatColumnSpacing();

                // Return success if at least one component was formatted
                return headersFormatted || footersFormatted || marginsFormatted ||
                       orientationFormatted || pageSizeFormatted ||
                       columnNumFormatted || columnSpacingFormatted;
            }

            return false;
        }

        public List<AbstractNode> GetConvertedContent()
        {
            // Return the formatted LaTeX nodes
            return layoutManager.ApplyLayoutFormatting();
        }
    }
}