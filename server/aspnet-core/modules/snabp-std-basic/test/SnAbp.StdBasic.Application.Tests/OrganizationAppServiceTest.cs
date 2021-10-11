using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using SnAbp.StdBasic.Dto;
using Xunit;


namespace SnAbp.StdBasic
{
    public class OrganizationAppServiceTest: StdBasicApplicationTestBase
    {
        private readonly IOrganizationAppService _orgAppService;

        public OrganizationAppServiceTest()
        {
            _orgAppService = GetRequiredService<IOrganizationAppService>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var result = await _orgAppService.GetListAsync(new OrganizationGetDto { Name = ""});
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateAsync()
        {
            OrganizationInputDto dto = new OrganizationInputDto();
            dto.Name = "测试机构";
            dto.ShortName = "测试机构";
            dto.Description = "单元测试机构添加";
            dto.Remark = "单元测试机构添加";
            dto.ParentId = null;
            dto.Order = 1;
            var result = await _orgAppService.CreateAsync(dto);
            result.ShouldNotBeNull();
        }
        [Fact]
        public async Task DeleteAsync()
        {
            var result = await _orgAppService.DeleteAsync(Guid.Parse("bca027f9-f3ec-465e-a29d-15e5ab4dfb18"));
            result.ShouldBeTrue();
        }

        [Fact]
        public async Task UpdateAsync()
        {
            OrganizationUpdateDto dto = new OrganizationUpdateDto();
            dto.Id = Guid.Parse("84b1ca80-d2a6-4d35-af58-82d35389b4b9");
            dto.Name = "测试机构";
            dto.ShortName = "测试机构";  
            dto.Description = "单元测试机构添加";
            dto.Remark = "单元测试机构添加";
            dto.ParentId = null;
            dto.Order = 1;
            var result = await _orgAppService.UpdateAsync(dto);
            result.ShouldBeTrue();
        }
    }
}
