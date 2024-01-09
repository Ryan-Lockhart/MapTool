using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapTool
{
    public enum Border
    {
        None = 0,
        Northern = 1,
        Southern = 2,
        Western = 4,
        Eastern = 8,
        Northwestern = Northern | Western,
        Northeastern = Northern | Eastern,
        Southwestern = Southern | Western,
        Southeastern = Southern | Eastern,
    }

    public enum MapType
    {
        Areas,
        Climates,
        Continents,
        Cultures,
        CultureGroups,
        Provinces,
        Regions,
        Religions,
        Rivers,
        Superregions,
        Terrain,
        TradeGoods
    }
}
