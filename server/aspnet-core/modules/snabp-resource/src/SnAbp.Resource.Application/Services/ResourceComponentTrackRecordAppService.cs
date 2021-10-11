using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Enums;
using SnAbp.Resource.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

/************************************************************************************
*命名空间：SnAbp.Resource.Services
*文件名：ResourceComponentTrackRecordAppService
*创建人： liushengtao
*创建时间：2021/6/24 10:39:30
*描述：构件跟踪记录接口
*
***********************************************************************/
namespace SnAbp.Resource.Services
{
    public class ResourceComponentTrackRecordAppService : ResourceAppService, IResourceComponentTrackRecordAppService
    {
        private readonly IRepository<ComponentRltQRCode, Guid> _ComponentRltQRCodeRepository;
        private readonly IRepository<Equipment, Guid> _Equipment;
        private readonly IRepository<ComponentTrackRecord, Guid> _ComponentTrackRecordRepository;
        private readonly IGuidGenerator _guidGenerator;
        public ResourceComponentTrackRecordAppService(
              IRepository<ComponentRltQRCode, Guid> ComponentRltQRCodeRepository,
              IRepository<Equipment, Guid> Equipment,
              IRepository<ComponentTrackRecord, Guid> ComponentTrackRecordRepository,
              IGuidGenerator guidGenerator
            )
        {
            _ComponentRltQRCodeRepository = ComponentRltQRCodeRepository;
            _Equipment = Equipment;
            _ComponentTrackRecordRepository = ComponentTrackRecordRepository;
            _guidGenerator = guidGenerator;
        }

        /// <summary>
        /// 创建跟踪记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ComponentTrackRecordDto> Create(ComponentTrackRecordCreateDto input)
        {
            input.TrackingId = _guidGenerator.Create();
            if (input.NodeType == NodeType.Install)
            {
                var componentRltQRCode = _ComponentRltQRCodeRepository.FirstOrDefault(x => x.Id == input.ComponentRltQRCodeId);
                if (componentRltQRCode != null && input.InstallationEquipmentId != null)
                {
                        var quipment = _Equipment.FirstOrDefault(x => x.Id == input.InstallationEquipmentId);
                        quipment.State = EquipmentState.Installed;
                        componentRltQRCode.InstallationEquipmentId = input.InstallationEquipmentId;
                        await _ComponentRltQRCodeRepository.UpdateAsync(componentRltQRCode);
                        await _Equipment.UpdateAsync(quipment);
                }
            }
            var oldEntity = _ComponentTrackRecordRepository.FirstOrDefault(x => x.ComponentRltQRCodeId == input.ComponentRltQRCodeId && x.NodeType == NodeType.Install);
            if (oldEntity != null)
            {
                await _ComponentTrackRecordRepository.DeleteAsync(oldEntity.Id);
            }
            var componentTrackRecord = ObjectMapper.Map<ComponentTrackRecordCreateDto, ComponentTrackRecord>(input);
            componentTrackRecord.SetId(_guidGenerator.Create());

            await _ComponentTrackRecordRepository.InsertAsync(componentTrackRecord);

            return ObjectMapper.Map<ComponentTrackRecord, ComponentTrackRecordDto>(componentTrackRecord);
        }

        public Task<List<ComponentTrackRecordDto>> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的数据");
            /* 获取当前设备的二维码id*/
            var componentRltQRCode = _ComponentRltQRCodeRepository.WithDetails().Where(x => x.GenerateEquipmentId == id && x.State == ActivatedState.Activated).FirstOrDefault();
            if (componentRltQRCode == null) throw new UserFriendlyException("当前设备未生成二维码，不存在构件跟踪记录");
            var componentRltQRCodeId = componentRltQRCode.Id;
            /*根据二维码id获取跟踪记录*/
            var componentTrackRecords = _ComponentTrackRecordRepository.WithDetails().Where(x => x.ComponentRltQRCodeId == componentRltQRCodeId).ToList();
            return Task.FromResult(ObjectMapper.Map<List<ComponentTrackRecord>, List<ComponentTrackRecordDto>>(componentTrackRecords.OrderBy(x => x.Time).ToList()));
        }

        public async Task<List<ComponentTrackRecordDto>> GetByInstallationEquipmentId(Guid installationEquipmentId)
        {
            if (installationEquipmentId == null || installationEquipmentId == Guid.Empty) throw new UserFriendlyException("请确定要查询的数据");

            var componentRltQRCode = _ComponentRltQRCodeRepository.WithDetails().Where(x => x.InstallationEquipmentId == installationEquipmentId && x.State == ActivatedState.Activated).FirstOrDefault();
            if (componentRltQRCode == null)
            {
                return new List<ComponentTrackRecordDto>();
            }
            var componentRltQRCodeId = componentRltQRCode.Id;
            /*根据二维码id获取跟踪记录*/
            var componentTrackRecords = _ComponentTrackRecordRepository.WithDetails().Where(x => x.ComponentRltQRCodeId == componentRltQRCodeId).ToList();
            return ObjectMapper.Map<List<ComponentTrackRecord>, List<ComponentTrackRecordDto>>(componentTrackRecords.OrderBy(x => x.Time).ToList());
        }

        /// <summary>
        /// 通过二维码id 查询安装设备id
        /// </summary>
        /// <param name="QRCode"></param>
        /// <returns></returns>
        public Task<Guid?> GetInstallationEquipmentId(string QRCode)
        {
            if (string.IsNullOrWhiteSpace(QRCode)) throw new UserFriendlyException("二维码无效");

            var installationId = _ComponentRltQRCodeRepository.FirstOrDefault(x => x.QRCode == QRCode).InstallationEquipmentId;
            if (installationId == null) throw new UserFriendlyException("暂无关联安装设备");

            return Task.FromResult(installationId);
        }
    }
}
