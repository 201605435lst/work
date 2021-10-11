using SnAbp.Material.Dtos;
using SnAbp.Material.Entities;
using SnAbp.Material.Enums;
using SnAbp.Material.IServices;
using SnAbp.Utils.ExcelHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Material.Services
{
    public class MaterialSupplierAppService : MaterialAppService, IMaterialSupplierAppService
    {
        private readonly IRepository<Supplier, Guid> _repository;
        private readonly IGuidGenerator _generator;
        private readonly IRepository<SupplierRltAccessory, Guid> _repositorySupplierRltAccessory;
        private readonly IRepository<SupplierRltContacts, Guid> _repositorySupplierRltContacts;
        public MaterialSupplierAppService(
            IRepository<Supplier, Guid> repository,
            IGuidGenerator generator,
            IRepository<SupplierRltAccessory, Guid> repositorySupplierRltAccessory,
            IRepository<SupplierRltContacts, Guid> repositorySupplierRltContacts)
        {
            _repository = repository;
            _generator = generator;
            _repositorySupplierRltAccessory = repositorySupplierRltAccessory;
            _repositorySupplierRltContacts = repositorySupplierRltContacts;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<SupplierDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不能为空");
            }

            var supplier = _repository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (supplier == null)
            {
                throw new UserFriendlyException("该供应商不存在");
            }

            return Task.FromResult(ObjectMapper.Map<Supplier, SupplierDto>(supplier));
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<SupplierSimpleDto>> GetList(SupplierSearchDto input)
        {
            var suppliers = _repository.WithDetails()
                   .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Name.Contains(input.Keyword) || x.Code.Contains(input.Keyword));

            var result = new PagedResultDto<SupplierSimpleDto>();
            result.TotalCount = suppliers.Count();
            if (input.IsAll)
            {
                result.Items = ObjectMapper.Map<List<Supplier>, List<SupplierSimpleDto>>
                    (suppliers.OrderBy(x => x.Code).ToList());
            }
            else
            {
                result.Items = ObjectMapper.Map<List<Supplier>, List<SupplierSimpleDto>>
                    (suppliers.OrderBy(x => x.Code).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            return Task.FromResult(result);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SupplierSimpleDto> Create(SupplierCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name.Trim())) throw new Volo.Abp.UserFriendlyException("名称不能为空");
            if (_repository.FirstOrDefault(x => x.Name == input.Name) != null) throw new Volo.Abp.UserFriendlyException("该供应商名称已存在");

            var maxCode = "GYS-0001";
            var maxCodeSupplier = _repository.Where(x => x.Id != Guid.Empty).OrderByDescending(s => s.Code).FirstOrDefault();
            if (maxCodeSupplier != null && !string.IsNullOrEmpty(maxCodeSupplier.Code))
            {
                var lastCode = "0000" + (int.Parse(maxCodeSupplier.Code.Substring(maxCodeSupplier.Code.Length - 4)) + 1).ToString();
                maxCode = "GYS-" + lastCode.Substring(lastCode.Length - 4);
            }

            var supplier = new Supplier(_generator.Create())
            {
                Name = input.Name,
                Type = input.Type,
                Level = input.Level,
                Property = input.Property,
                Code = maxCode,
                Principal = input.Principal,
                Telephone = input.Telephone,
                LegalPerson = input.LegalPerson,
                TIN = input.TIN,
                BusinessScope = input.BusinessScope,
                OpeningBank = input.OpeningBank,
                BankAccount = input.BankAccount,
                AccountOpeningUnit = input.AccountOpeningUnit,
                RegisteredAssets = input.RegisteredAssets,
                Address = input.Address,
                Remark = input.Remark,
            };

            supplier.SupplierRltAccessories = new List<SupplierRltAccessory>();

            // 重新保存关联文件信息
            foreach (var accessory in input.SupplierRltAccessories)
            {
                supplier.SupplierRltAccessories.Add(new SupplierRltAccessory(_generator.Create())
                {
                    FileId = accessory.FileId,
                    SupplierId = supplier.Id,
                });
            }

            supplier.SupplierRltContacts = new List<SupplierRltContacts>();
            // 重新保存联系人信息
            foreach (var user in input.SupplierRltContacts)
            {
                supplier.SupplierRltContacts.Add(new SupplierRltContacts(_generator.Create())
                {
                    Name = user.Name,
                    Telephone = user.Telephone,
                    LandlinePhone = user.LandlinePhone,
                    QQ = user.QQ,
                    Email = user.Email,
                    SupplierId = supplier.Id,
                });
            }

            await _repository.InsertAsync(supplier);

            return ObjectMapper.Map<Supplier, SupplierSimpleDto>(supplier);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SupplierSimpleDto> Update(SupplierUpdateDto input)
        {
            if (string.IsNullOrEmpty(input.Name.Trim())) throw new Volo.Abp.UserFriendlyException("名称不能为空");
            if (_repository.FirstOrDefault(x => x.Name == input.Name && x.Id != input.Id) != null) throw new Volo.Abp.UserFriendlyException("该供应商名称已存在");

            var supplier = _repository.FirstOrDefault(x => x.Id == input.Id);
            if (supplier == null) throw new Volo.Abp.UserFriendlyException("该供应商不存在");

            supplier.Name = input.Name;
            supplier.Type = input.Type;
            supplier.Level = input.Level;
            supplier.Property = input.Property;
            supplier.Code = supplier.Code;
            supplier.Principal = input.Principal;
            supplier.Telephone = input.Telephone;
            supplier.LegalPerson = input.LegalPerson;
            supplier.TIN = input.TIN;
            supplier.BusinessScope = input.BusinessScope;
            supplier.OpeningBank = input.OpeningBank;
            supplier.BankAccount = input.BankAccount;
            supplier.AccountOpeningUnit = input.AccountOpeningUnit;
            supplier.RegisteredAssets = input.RegisteredAssets;
            supplier.Address = input.Address;
            supplier.Remark = input.Remark;

            // 清楚之前关联文件信息
            await _repositorySupplierRltAccessory.DeleteAsync(x => x.SupplierId == supplier.Id);
            supplier.SupplierRltAccessories = new List<SupplierRltAccessory>();

            // 重新保存关联文件信息
            foreach (var accessory in input.SupplierRltAccessories)
            {
                supplier.SupplierRltAccessories.Add(new SupplierRltAccessory(_generator.Create())
                {
                    FileId = accessory.FileId,
                    SupplierId = supplier.Id,
                });
            }

            // 清楚之前关联文件信息
            await _repositorySupplierRltContacts.DeleteAsync(x => x.SupplierId == supplier.Id);
            supplier.SupplierRltContacts = new List<SupplierRltContacts>();
            // 重新保存联系人信息
            foreach (var user in input.SupplierRltContacts)
            {
                supplier.SupplierRltContacts.Add(new SupplierRltContacts(_generator.Create())
                {
                    Name = user.Name,
                    Telephone = user.Telephone,
                    LandlinePhone = user.LandlinePhone,
                    QQ = user.QQ,
                    Email = user.Email,
                    SupplierId = supplier.Id,
                });
            }

            await _repository.UpdateAsync(supplier);

            return ObjectMapper.Map<Supplier, SupplierSimpleDto>(supplier);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            await _repository.DeleteAsync(id);

            return true;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<Stream> Export(SupplierSearchDto input)
        {
            var suppliers = _repository.WithDetails().Where(x =>
                                                            x.Name.Contains(input.Keyword) ||
                                                            x.Code.Contains(input.Keyword))
                                                     .OrderBy(x => x.Code)
                                                     .ToList();
            Stream stream = null;
            byte[] sbuf;
            var dt = (DataTable)null;
            var dataColumn = (DataColumn)null;
            var dataRow = (DataRow)null;
            dt = new DataTable();
            //添加表头
            var enumValues = Enum.GetValues(typeof(ExportSuppliers));
            if (enumValues.Length > 0)
            {
                foreach (int item in enumValues)
                {
                    dataColumn = new DataColumn(Enum.GetName(typeof(ExportSuppliers), item));
                    dt.Columns.Add(dataColumn);
                }
            }
            //添加内容
            foreach (var row in suppliers)
            {
                dataRow = dt.NewRow();
                dataRow[ExportSuppliers.供应商名称.ToString()] = row.Name;
                dataRow[ExportSuppliers.供应商类型.ToString()] = GetSupplierTypeName(row.Type);
                dataRow[ExportSuppliers.供应商级别.ToString()] = GetSupplierLevelName(row.Level);
                dataRow[ExportSuppliers.供应商性质.ToString()] = GetSupplierPropertyName(row.Property);
                dataRow[ExportSuppliers.供应商编码.ToString()] = row.Code;
                dataRow[ExportSuppliers.负责人姓名.ToString()] = row.Principal;
                dataRow[ExportSuppliers.负责人电话.ToString()] = row.Telephone;
                dataRow[ExportSuppliers.法定代表人.ToString()] = row.LegalPerson;
                dataRow[ExportSuppliers.开户单位.ToString()] = row.AccountOpeningUnit;
                dataRow[ExportSuppliers.开户行名称.ToString()] = row.OpeningBank;
                dataRow[ExportSuppliers.开户行账户.ToString()] = row.BankAccount;
                dataRow[ExportSuppliers.注册资本.ToString()] = row.RegisteredAssets;
                dataRow[ExportSuppliers.经营范围.ToString()] = row.BusinessScope;
                dataRow[ExportSuppliers.纳税人识别号.ToString()] = row.TIN;
                dataRow[ExportSuppliers.地址.ToString()] = row.Address;
                dataRow[ExportSuppliers.备注.ToString()] = row.Remark;
                dt.Rows.Add(dataRow);
            }
            sbuf = ExcelHelper.DataTableToExcel(dt, "供应商数据表.xlsx",null,"供应商数据表");
            stream = new MemoryStream(sbuf);
            return System.Threading.Tasks.Task.FromResult(stream);
        }


        #region 私有放法

        /// <summary>
        /// 获取供应商类型title
        /// </summary>
        /// <returns></returns>
        private string GetSupplierTypeName(SupplierType Type)
        {
            string typeName = null;

            if (Type == SupplierType.Supplier)
            {
                typeName = "供应商";
            }
            if (Type == SupplierType.Proprietor)
            {
                typeName = "业主";
            }
            if (Type == SupplierType.ConstructionTeam)
            {
                typeName = "施工队";
            }
            return typeName;
        }

        /// <summary>
        /// 获取供应商级别title
        /// </summary>
        /// <returns></returns>
        private string GetSupplierLevelName(SupplierLevel Level)
        {
            string levelName = null;

            if (Level == SupplierLevel.LevelI)
            {
                levelName = "一级供应商";
            }
            if (Level == SupplierLevel.LevelII)
            {
                levelName = "二级供应商";
            }
            if (Level == SupplierLevel.LevelIII)
            {
                levelName = "三级供应商";
            }
            return levelName;
        }

        /// <summary>
        /// 获取供应商性质title
        /// </summary>
        /// <returns></returns>
        private string GetSupplierPropertyName(SupplierProperty Property)
        {
            string propertyName = null;

            if (Property == SupplierProperty.Unit)
            {
                propertyName = "单位";
            }
            if (Property == SupplierProperty.Personal)
            {
                propertyName = "个人";
            }
            return propertyName;
        }

        #endregion
    }
}
