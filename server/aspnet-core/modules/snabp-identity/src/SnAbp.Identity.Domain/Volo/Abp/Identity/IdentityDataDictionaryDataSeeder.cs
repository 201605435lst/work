/**********************************************************************
*******命名空间： Volo.Abp.Identity
*******类 名 称： IdentityDataDictionaryDataSeeder
*******类 说 明： 数字字典种子数据
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/19 8:33:00
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using NPOI.SS.Formula.Functions;
using SnAbp.Identity;

namespace SnAbp.Identity
{
    public class IdentityDataDictionaryDataSeeder
    {
        public static List<DataDictionary> GetDataDictionaryDataSeed()
        {

            var dataDictionaryId = Guid.NewGuid();
            var installationSiteTypeId = Guid.NewGuid();
            var installationSiteType3Id = Guid.NewGuid();
            var installationSiteCategoryId = Guid.NewGuid();
            var emergLevelId = Guid.NewGuid();
            var userPositionId = Guid.NewGuid();
            var repairTagId = Guid.NewGuid();
            var projectTypeId = Guid.NewGuid();
            var executiveUnitId = Guid.NewGuid();
            var professionId = Guid.NewGuid();
            var contractorId = Guid.NewGuid();
            return new List<DataDictionary>()
            {
                 new DataDictionary(userPositionId){
                    Name = "用户职位",
                    Key = "UserPosition",
                    IsStatic = true,
                    Children= new List<DataDictionary>(){
                          new DataDictionary(Guid.NewGuid()){Name = "董事长",Key = "UserPosition.Chairman",IsStatic = true,ParentId=userPositionId},
                          new DataDictionary(Guid.NewGuid()){Name = "总经理",Key = "UserPosition.GeneralManager",IsStatic = true,ParentId=userPositionId},
                    }
                },
                new DataDictionary(dataDictionaryId){
                    Name = "组织机构类型",
                    Key = "OrganizationType",
                    IsStatic = true,
                    Children = new List<DataDictionary>(){
                        new DataDictionary(Guid.NewGuid()){Name = "公司",Key = "OrganizationType.Company",IsStatic = true,ParentId = dataDictionaryId},
                        new DataDictionary(Guid.NewGuid()){Name = "部门",Key = "OrganizationType.Department",IsStatic = true,ParentId = dataDictionaryId},
                        new DataDictionary(Guid.NewGuid()){Name = "局",Key = "OrganizationType.Bureau",IsStatic = true,ParentId = dataDictionaryId},
                        new DataDictionary(Guid.NewGuid()){Name = "所",Key = "OrganizationType.Institute",IsStatic = true,ParentId = dataDictionaryId},
                        new DataDictionary(Guid.NewGuid()){Name = "科",Key = "OrganizationType.Section",IsStatic = true,ParentId = dataDictionaryId},
                        new DataDictionary(Guid.NewGuid()){Name = "室",Key = "OrganizationType.Office",IsStatic = true,ParentId = dataDictionaryId},
                        new DataDictionary(Guid.NewGuid()){Name = "工段",Key = "OrganizationType.WorkSection",IsStatic = true,ParentId = dataDictionaryId},
                        new DataDictionary(Guid.NewGuid()){Name = "车间",Key = "OrganizationType.WorkShop",IsStatic = true,ParentId = dataDictionaryId},
                        new DataDictionary(Guid.NewGuid()){Name = "工区",Key = "OrganizationType.WorkArea",IsStatic = true,ParentId = dataDictionaryId},
                        new DataDictionary(Guid.NewGuid()){Name = "网管",Key = "OrganizationType.Management",IsStatic = true,ParentId = dataDictionaryId},
                        new DataDictionary(Guid.NewGuid()){Name = "段",Key = "OrganizationType.Duan",IsStatic = true,ParentId = dataDictionaryId},
                    }
                },
                new DataDictionary(installationSiteTypeId){
                    Name = "安装位置类型",
                    Key = "InstallationSiteType",
                    IsStatic = true,
                    Children = new List<DataDictionary>(){
                        new DataDictionary(Guid.NewGuid()){Name = "一类",Key = "InstallationSiteType.1",IsStatic = true,ParentId = installationSiteTypeId},
                        new DataDictionary(Guid.NewGuid()){Name = "二类",Key = "InstallationSiteType.2",IsStatic = true,ParentId = installationSiteTypeId},
                        new DataDictionary(installationSiteType3Id){
                            Name = "三类",
                            Key = "InstallationSiteType.3",
                            IsStatic = true,
                            ParentId = installationSiteTypeId,
                            Children = new List<DataDictionary>(){
                                new DataDictionary(Guid.NewGuid()){Name = "充气机房",Key = "InstallationSiteType.3.InflatorRoom",IsStatic = true,ParentId = installationSiteType3Id},
                                new DataDictionary(Guid.NewGuid()){Name = "车站通信机械室",Key = "InstallationSiteType.3.MachineRoom",IsStatic = true,ParentId = installationSiteType3Id},
                                new DataDictionary(Guid.NewGuid()){Name = "电缆转接机房",Key = "InstallationSiteType.3.CableSwitchingRoom",IsStatic = true,ParentId = installationSiteType3Id},
                                new DataDictionary(Guid.NewGuid()){Name = "区间设备",Key = "InstallationSiteType.3.SectionEquipment",IsStatic = true,ParentId = installationSiteType3Id},
                            }
                        },
                        new DataDictionary(Guid.NewGuid()){Name = "其他",Key = "InstallationSiteType.Other",IsStatic = true},
                    }
                },
                new DataDictionary(installationSiteCategoryId)
                {
                    Name="安装位置分类",
                    Key="InstallationSiteCategory",
                    IsStatic=true,
                    Children = new List<DataDictionary>
                    {
                        new DataDictionary(Guid.NewGuid()){Name="基站机房",Key="InstallationSiteCategory.JiZhan",IsStatic=true,ParentId=installationSiteCategoryId},
                        new DataDictionary(Guid.NewGuid()){Name="直放站机房",Key="InstallationSiteCategory.ZhiFangZhan",IsStatic=true,ParentId=installationSiteCategoryId},
                        new DataDictionary(Guid.NewGuid()){Name="防灾机房",Key="InstallationSiteCategory.FangZai",IsStatic=true,ParentId=installationSiteCategoryId},
                        new DataDictionary(Guid.NewGuid()){Name="车站通信机房",Key="InstallationSiteCategory.CheZhanTongXin",IsStatic=true,ParentId=installationSiteCategoryId},
                        new DataDictionary(Guid.NewGuid()){Name="信息机房",Key="InstallationSiteCategory.XinXi",IsStatic=true,ParentId=installationSiteCategoryId},
                        new DataDictionary(Guid.NewGuid()){Name="合建站",Key="InstallationSiteCategory.HeJianZhan",IsStatic=true,ParentId=installationSiteCategoryId},
                        new DataDictionary(Guid.NewGuid()){Name="AT所通信机房",Key="InstallationSiteCategory.ATTongXin",IsStatic=true,ParentId=installationSiteCategoryId},
                        new DataDictionary(Guid.NewGuid()){Name="AT所分区所通信机房",Key="InstallationSiteCategory.ATFenQuTongXin",IsStatic=true,ParentId=installationSiteCategoryId},
                        new DataDictionary(Guid.NewGuid()){Name="信号中继站通信机房",Key="InstallationSiteCategory.XinHaoZhongJiZhanTongXin",IsStatic=true,ParentId=installationSiteCategoryId},
                        new DataDictionary(Guid.NewGuid()){Name="警务区通信机房",Key="InstallationSiteCategory.JingWuQuTongXin",IsStatic=true,ParentId=installationSiteCategoryId},
                        new DataDictionary(Guid.NewGuid()){Name="牵引变电所通信机房",Key="InstallationSiteCategory.QianYinBianDianSuoTongXin",IsStatic=true,ParentId=installationSiteCategoryId},
                        new DataDictionary(Guid.NewGuid()){Name="其他通信机房",Key="InstallationSiteCategory.QiTaTongXin",IsStatic=true,ParentId=installationSiteCategoryId},
                        new DataDictionary(Guid.NewGuid()){Name="其他",Key="InstallationSiteCategory.QiTa",IsStatic=true,ParentId=installationSiteCategoryId},
                    }
                },
                new DataDictionary(emergLevelId){
                    Name = "应急预案等级",
                    Key = "EmergPlanLevel",
                    IsStatic = true,
                    Children= new List<DataDictionary>(){
                          new DataDictionary(Guid.NewGuid()){Name = "一级",Key = "EmergPlanLevel.1",IsStatic = true,ParentId=emergLevelId},
                          new DataDictionary(Guid.NewGuid()){Name = "二级",Key = "EmergPlanLevel.2",IsStatic = true,ParentId=emergLevelId},
                          new DataDictionary(Guid.NewGuid()){Name = "三级",Key = "EmergPlanLevel.3",IsStatic = true,ParentId=emergLevelId},
                    }
                },
                new DataDictionary(Guid.NewGuid()){
                    Name = "应急故障原因",
                    Key = "EmergFaultReasonType",
                    IsStatic = true
                },
                new DataDictionary(Guid.NewGuid()){
                    Name = "检测机构",
                    Key = "TestingOrganizationType",
                    IsStatic = true
                },

                new DataDictionary(repairTagId)
                {
                    Name = "维修项标签",
                    Key = "RepairTag",
                    IsStatic = true,
                    Children = new List<DataDictionary>
                    {
                        new DataDictionary(Guid.NewGuid()){Name = "铁路有线版",Key = "RepairTag.RailwayWired",IsStatic = true,ParentId = repairTagId},
                        new DataDictionary(Guid.NewGuid()){Name = "铁路高铁版",Key = "RepairTag.RailwayHighSpeed",IsStatic = true,ParentId = repairTagId}
                }
                },
                new DataDictionary(Guid.NewGuid())
                {
                    Name = "项目合同类型",
                    Key = "ProjectContractType",
                    IsStatic = true
                },
               //new DataDictionary(Guid.NewGuid())
               // {
               //     Name = "资金类别",
               //     Key = "CostmanagementCapitalCategory",
               //     IsStatic = true
               // },
              //new DataDictionary(Guid.NewGuid())
              //  {
              //      Name = "土建单位",
              //      Key = "ConstructionUnit",
              //      IsStatic = true
              //  },
               //new DataDictionary(Guid.NewGuid())
               // {
               //     Name = "成本合同类别",
               //     Key = "CostmanagementContract",
               //     IsStatic = true
               // }, 
               // new DataDictionary(Guid.NewGuid())
               // {
               //     Name = "收款单位",
               //     Key = "CostmanagementCostPayee",
               //     IsStatic = true
               // },
                // new DataDictionary(Guid.NewGuid())
                //{
                //    Name = "费用类型",
                //    Key = "CostmanagementCostType",
                //    IsStatic = true
                //},
                new DataDictionary(projectTypeId)
                {
                    Name = "工程类型",
                    IsStatic = true,
                    Key = "ProjectType",
                    Children = new List<DataDictionary>
                    {
                        new DataDictionary(Guid.NewGuid()){Name = "建筑工程",Key = "ProjectType.BuildProject",IsStatic = true,ParentId = projectTypeId},
                        new DataDictionary(Guid.NewGuid()){Name = "安装工程",Key = "ProjectType.InstallProject",IsStatic = true,ParentId = projectTypeId},
                        new DataDictionary(Guid.NewGuid()){Name = "机电工程",Key = "ProjectType.ElectromechanicalProject",IsStatic = true,ParentId = projectTypeId},
                        new DataDictionary(Guid.NewGuid()){Name = "其他工程",Key = "ProjectType.OtherProject",IsStatic = true,ParentId = projectTypeId},
                    }
                },
                new DataDictionary(executiveUnitId)
                {
                    Name="执行单位",
                    IsStatic=true,
                    Key="ExecutiveUnit"
                },
                new DataDictionary(professionId)
                {
                    Name = "专业",
                    IsStatic = true,
                    Key = "Profession",
                    Children = new List<DataDictionary>
                    {
                        //new DataDictionary(Guid.NewGuid()){Name = "全部",Key = "Profession.All",IsStatic = true,ParentId = professionId},
                        new DataDictionary(Guid.NewGuid()){Name = "通信专业",Key = "Profession.StructuralMember",IsStatic = true,ParentId = professionId},
                        new DataDictionary(Guid.NewGuid()){Name = "信号专业",Key = "Profession.Tool",IsStatic = true,ParentId = professionId},
                        new DataDictionary(Guid.NewGuid()){Name = "信息专业",Key = "Profession.SafetyEquipment",IsStatic = true,ParentId = professionId},
                        new DataDictionary(Guid.NewGuid()){Name = "路基专业",Key = "Profession.LowvalueConsumables",IsStatic = true,ParentId = professionId},
                        new DataDictionary(Guid.NewGuid()){Name = "轨道专业",Key = "Profession.RevolvingMaterials",IsStatic = true,ParentId = professionId},
                        new DataDictionary(Guid.NewGuid()){Name = "桥涵专业",Key = "Profession.FuelOil",IsStatic = true,ParentId = professionId},
                        new DataDictionary(Guid.NewGuid()){Name = "隧道专业",Key = "Profession.OtherMaterials",IsStatic = true,ParentId = professionId},
                        new DataDictionary(Guid.NewGuid()){Name = "电力专业",Key = "Profession.MainMaterials",IsStatic = true,ParentId = professionId},
                        new DataDictionary(Guid.NewGuid()){Name = "牵引变电专业",Key = "Profession.Equipment",IsStatic = true,ParentId = professionId},
                        new DataDictionary(Guid.NewGuid()){Name = "接触网专业",Key = "Profession.EquipmentAsterisk",IsStatic = true,ParentId = professionId},
                        new DataDictionary(Guid.NewGuid()){Name = "房建专业",Key = "Profession.EquipmentAsterisk",IsStatic = true,ParentId = professionId},
                        new DataDictionary(Guid.NewGuid()){Name = "防灾专业",Key = "Profession.EquipmentAsterisk",IsStatic = true,ParentId = professionId},
                        new DataDictionary(Guid.NewGuid()){Name = "环保专业",Key = "Profession.EquipmentAsterisk",IsStatic = true,ParentId = professionId},
                        new DataDictionary(Guid.NewGuid()){Name = "站场专业",Key = "Profession.EquipmentAsterisk",IsStatic = true,ParentId = professionId},
                    }
                },
                new DataDictionary(contractorId)
                {
                    Name = "承包商",
                    IsStatic = true,
                    Key = "Contractor"
                }
            };
        }

    }
}
