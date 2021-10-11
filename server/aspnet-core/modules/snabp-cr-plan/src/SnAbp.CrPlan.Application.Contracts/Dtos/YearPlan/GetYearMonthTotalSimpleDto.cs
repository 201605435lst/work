using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class GetYearMonthTotalSimpleDto
    {
        /// <summary>
        ///总数量统计
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 已通过的年表数量
        /// </summary>
        public int ApprovedYearTotal { get; set; }

        /// <summary>
        /// 已通过的年表车间ids
        /// </summary>
        public List<Guid> ArrovedYearOrganizationIds { get; set; }

        /// <summary>
        /// 已拒绝的年表数量
        /// </summary>
        public int RefausedYearTotal { get; set; }

        /// <summary>
        /// 已拒绝的年表车间ids
        /// </summary>
        public List<Guid> RefausedYearOrganizationIds { get; set; }

        /// <summary>
        /// 待审核的年表数量
        /// </summary>
        public int WaittingYearTotal { get; set; }

        /// <summary>
        /// 待审核的年表车间ids
        /// </summary>
        public List<Guid> WaittingYearOrganizationIds { get; set; }

        /// <summary>
        ///未提报年表车间数量
        /// </summary
        public int UnSubmitedYearTotal { get; set; }
        /// <summary>
        ///未提报年表车间ids
        /// </summary>
        public List<Guid> UnSubmitedYearOrganizationIds { get; set; }


        /// <summary>
        /// 已通过的月表数量
        /// </summary>
        public int ApprovedMonthTotal { get; set; }

        /// <summary>
        /// 已通过的月表车间ids
        /// </summary>
        public List<Guid> ArrovedMonthOrganizationIds { get; set; }

        /// <summary>
        /// 已拒绝的月表数量
        /// </summary>
        public int RefausedMonthTotal { get; set; }

        /// <summary>
        /// 已拒绝的月表车间ids
        /// </summary>
        public List<Guid> RefausedMonthOrganizationIds { get; set; }

        /// <summary>
        /// 待审核的月表数量
        /// </summary>
        public int WaittingMonthTotal { get; set; }

        /// <summary>
        /// 待审核的月表车间ids
        /// </summary>
        public List<Guid> WaittingMonthOrganizationIds { get; set; }

        /// <summary>
        ///未提报月表车间数量
        /// </summary
        public int UnSubmitedMonthTotal { get; set; }
        /// <summary>
        ///未提报月表车间ids
        /// </summary>
        public List<Guid> UnSubmitedMonthOrganizationIds { get; set; }



        // <summary>
        /// 已通过的年表月度数量
        /// </summary>
        public int ApprovedMonthOfYearTotal { get; set; }

        /// <summary>
        /// 已通过的年表月度车间ids
        /// </summary>
        public List<Guid> ArrovedMonthOfYearOrganizationIds { get; set; }

        /// <summary>
        /// 已拒绝的年表月度数量
        /// </summary>
        public int RefausedMonthOfYearTotal { get; set; }

        /// <summary>
        /// 已拒绝的年表月度车间ids
        /// </summary>
        public List<Guid> RefausedMonthOfYearOrganizationIds { get; set; }

        /// <summary>
        /// 待审核的年表月度数量
        /// </summary>
        public int WaittingMonthOfYearTotal { get; set; }

        /// <summary>
        /// 待审核的年表月度车间ids
        /// </summary>
        public List<Guid> WaittingMonthOfYearOrganizationIds { get; set; }

        /// <summary>
        ///未提报年表月度车间数量
        /// </summary
        public int UnSubmitedMonthOfYearTotal { get; set; }
        /// <summary>
        ///未提报年表月度车间ids
        /// </summary>
        public List<Guid> UnSubmitedMonthOfYearOrganizationIds { get; set; }
    }
}
