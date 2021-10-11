using System.Collections.Generic;

namespace SnAbp.FeatureManagement
{
    public class UpdateFeaturesDto
    {
        public List<UpdateFeatureDto> Features { get; set; }
    }
}