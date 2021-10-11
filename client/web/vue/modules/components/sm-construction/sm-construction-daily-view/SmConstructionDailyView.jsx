import './style';
import { getFileUrl, getUnplannedTaskType, getSafeProblemState,getQualityProblemType } from '../../_utils/utils';
import moment from 'moment';
import SmFileImageView from '../../sm-file/sm-file-manage/src/component/SmFileImageView';
import print from 'print-js';
export default {
  name: 'SmConstructionDailyView',
  props: {
    axios: { type: Function, default: null },
    record: { type: Object, default: null },
    sumCount: { type: Number, default: 0 },
    permissions: { type: Array, default: () => [] },
    dailyRltFiles: { type: Array, default: () => [] },
    dailyRltSafe: { type: Array, default: () => [] },
    dailyRltQuality: { type: Array, default: () => [] },
    dispatchs: { type: Array, default: () => [] },
    unplannedTask: { type: Array, default: () => [] },
    dailyRltEquipments: { type: Array, default: () => [] },
    dailyRltPlan: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource: null,
    };
  },

  computed: {},
  watch: {
    record: {
      handler: function (value, oldValue) {
        if (value) {
          this.dataSource = value;
        }
      },
      immediate: true,
      deep: true,
    },
  },
  async created() { },
  methods: {
    play(file) {
      let imgtypes = ['.jpg', '.png', '.tif', 'gif', '.JPG', '.PNG', '.GIF', '.jpeg', '.JPEG'];
      if (imgtypes.includes(file.type)) {
        this.$refs.SmFileImageView.view(file);
      } else {
        this.$message.warning('当前文件不支持预览');
      }
    },
  },
  render() {
    let _dailyRltFiles = [];
    _dailyRltFiles = this.record && this.record.dailyRltFiles.map(item => {
      let _name = item.file ? item.file.name : null;
      return (
        <span
          class="file-picture"
          onClick={() => {
            this.play(item.file);
          }}
        >
          <img src={getFileUrl(item.file.url)} alt={`${_name}`} class="picture" />
        </span>
      );
    });
    let _dailyRltQuality = [
      <a-row class="rows">
        <a-col span="3" class="title-daily center">
          序号
        </a-col>
        <a-col span="6" class="title-daily center">
          问题类型
        </a-col>
        <a-col span="6" class="title-daily center">
          问题名称
        </a-col>
        <a-col span="5" class="title-daily center">
          问题发起人
        </a-col>
        <a-col span="4" class="title-daily center">
          当前状态
        </a-col>
      </a-row>,
    ];
    this.dailyRltQuality.map((item, index) => {
      _dailyRltQuality.push(
        <a-row class="rows">
          <a-col span="3" class="title-daily center">
            {index + 1}
          </a-col>
          <a-col span="6" class="title-daily center">
            {item && item.type ? getQualityProblemType(item.type) : ''}
          </a-col>
          <a-col span="6" class="title-daily center">
            {item.title ? item.title : `  `}
          </a-col>
          <a-col span="5" class="title-daily center">
            {item && item.checker ? item.checker.name : ''}
          </a-col>
          <a-col span="4" class="title-daily center">
            {item && item.state ? getSafeProblemState(item.state) : ''}
          </a-col>
        </a-row>,
      );
    });
    let _dailyRltSafe = [
      <a-row class="rows">
        <a-col span="3" class="title-daily center">
          序号
        </a-col>
        <a-col span="6" class="title-daily center">
          问题类型
        </a-col>
        <a-col span="6" class="title-daily center">
          问题名称
        </a-col>
        <a-col span="5" class="title-daily center">
          问题发起人
        </a-col>
        <a-col span="4" class="title-daily center">
          当前状态
        </a-col>
      </a-row>,
    ];
    this.dailyRltSafe.map((item, index) => {
      _dailyRltSafe.push(
        <a-row class="rows">
          <a-col span="3" class="title-daily center">
            {index + 1}
          </a-col>
          <a-col span="6" class="title-daily center">
            {item && item.type ? item.type.name : ''}
          </a-col>
          <a-col span="6" class="title-daily center">
            {item.title ? item.title : `  `}
          </a-col>
          <a-col span="5" class="title-daily center">
            {item && item.checker ? item.checker.name : ''}
          </a-col>
          <a-col span="4" class="title-daily center">
            {item.state ? getSafeProblemState(item.state) : ''}
          </a-col>
        </a-row>,
      );
    });
    let _unplannedTask = [
      <a-row class="rows">
        <a-col span="3" class="title-daily center">
          序号
        </a-col>
        <a-col span="8" class="title-daily center">
          任务类型
        </a-col>
        <a-col span="13" class="title-daily center">
          任务说明
        </a-col>
      </a-row>,
    ];
    this.unplannedTask.map((item, index) => {
      _unplannedTask.push(
        <a-row class="rows">
          <a-col span="3" class="title-daily center">
            {index + 1}
          </a-col>
          <a-col span="8" class="title-daily center">
            {getUnplannedTaskType(item.taskType)}
          </a-col>
          <a-col span="13" class="title-daily center">
            {item.content ? item.content : `  `}
          </a-col>
        </a-row>,
      );
    });
    let _dailyRltPlan = [
      <a-row class="rows">
        <a-col span="3" class="title-daily center">
          序号
        </a-col>
        <a-col span="6" class="title-daily center">
          任务名称
        </a-col>
        <a-col span="3" class="title-daily center">
          单位
        </a-col>
        <a-col span="3" class="title-daily center">
          工程量
        </a-col>
        <a-col span="3" class="title-daily center">
          当天完成
        </a-col>
        <a-col span="3" class="title-daily center">
          当天完成量
        </a-col>
        <a-col span="3" class="title-daily center">
          累计完成量
        </a-col>
      </a-row>,
    ];
    this.dailyRltPlan.map((item, index) => {
      let count1 = Number((item.count / item.quantity) * 100).toFixed(2);
      count1 += "%";
      let count2 = Number(((item.count + item.sumCount) / item.quantity) * 100).toFixed(2);
      count2 += "%";
      _dailyRltPlan.push(
        <a-row class="rows">
          <a-col span="3" class="title-daily center">
            {index + 1}
          </a-col>
          <a-col span="6" class="title-daily center">
            {item.name ? item.name : `  `}
          </a-col>
          <a-col span="3" class="title-daily center">
            {item.unit ? item.unit : `  `}
          </a-col>
          <a-col span="3" class="title-daily center">
            {item.quantity ? item.quantity : `  `}
          </a-col>
          <a-col span="3" class="title-daily center">
            {item.count ? item.count : `  `}
          </a-col>
          <a-col span="3" class="title-daily center">
            {count1}
          </a-col>
          <a-col span="3" class="title-daily center">
            {count2}
          </a-col>
        </a-row>,
      );
    });
    let _dailyRltEquipments = [
      <a-row class="rows">
        <a-col span="3" class="title-daily center">
          序号
        </a-col>
        <a-col span="6" class="title-daily center">
          设备名称
        </a-col>
        <a-col span="5" class="title-daily center">
          规格型号
        </a-col>
        <a-col span="5" class="title-daily center">
          计量单位
        </a-col>
        <a-col span="5" class="title-daily center">
          工程量
        </a-col>
      </a-row>,
    ];
    this.dailyRltEquipments.map((item, index) => {
      let spec = item.productCategory ? item.productCategory.name : '';
      let unit = item.componentCategory && item.componentCategory.unit ? item.componentCategory.unit : "未定义";
      _dailyRltEquipments.push(
        <a-row class="rows">
          <a-col span="3" class="title-daily center">
            {index + 1}
          </a-col>
          <a-col span="6" class="title-daily center">
            {item.name ? item.name : `  `}
          </a-col>
          <a-col span="5" class="title-daily center">
            {spec}
          </a-col>
          <a-col span="5" class="title-daily center">
            {unit}
          </a-col>
          <a-col span="5" class="title-daily center">
            {item.quantity}
          </a-col>
        </a-row>,
      );
    });
    return (
      <div class="sm-construction-daily-log" >
        <div id="construction-daily-log">
          <a-row>
            <a-col span="24" class="body-title">{this.record && this.record.dailyTemplate ? this.record.dailyTemplate.name : '日志'}</a-col>
          </a-row>
          <a-row class="code">
            <a-col span="24" class="body-code">
              <span >{`编号：${this.record ? this.record.code : ''}`}</span>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
              日志编号
            </a-col>
            <a-col span="9">
              <span class="text">
                {this.record ? this.record.code : ''}
              </span>
            </a-col>
            <a-col span="3" class="title">
              派工单
            </a-col>
            <a-col span="9">
              <span class="text">{this.record &&this.record.dispatch ? this.record.dispatch.name : ''}</span>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
              填报日期
            </a-col>
            <a-col span="9">
              <span class="text">
                {this.record && this.record.date ? moment(this.record.date).format('YYYY-MM-DD') : ''}
              </span>
            </a-col>
            <a-col span="3" class="title">
              填报人
            </a-col>
            <a-col span="9">
              <span class="text">{this.record && this.record.informant ? this.record.informant.name : ''}</span>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
              天气
            </a-col>
            <a-col span="9">
              <span class="text">
                {this.record && this.record.weathers ? this.record.weathers : ''}
              </span>
            </a-col>
            <a-col span="3" class="title">
              温度
            </a-col>
            <a-col span="9">
              <span class="text">{this.record && this.record.temperature ? this.record.temperature : ''}</span>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
              风力风向
            </a-col>
            <a-col span="9">
              <span class="text">
                {this.record && this.record.windDirection ? this.record.windDirection : ''}
              </span>
            </a-col>
            <a-col span="3" class="title">
              空气质量
            </a-col>
            <a-col span="9">
              <span class="text">  {this.record && this.record.airQuality ? this.record.airQuality : ''}</span>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
              施工班组
            </a-col>
            <a-col span="9">
              <span class="text">
                {this.record && this.record.team ? this.record.team : ''}
              </span>
            </a-col>
            <a-col span="3" class="title">
              施工人员
            </a-col>
            <a-col span="9">
              <span class="text">{this.record ? this.record.builderCount : ''}</span>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
              施工部位
            </a-col>
            <a-col span="21">
              <span class="text">{this.record ? this.record.location : ''}</span>
            </a-col>
          </a-row>
          <a-row class="rows">
            <a-col span="3" class="title">
              施工总结
            </a-col>
            <a-col span="21">
              <span class="text">{this.record ? this.record.summary : ''}</span>
            </a-col>
          </a-row>
          {this.dailyRltPlan.length > 0 ? <a-row class="rows title">
            <a-col span="24" class="title-daily">
              <span class="text">
                施工任务
              </span>
            </a-col>
          </a-row> : ''}
          {this.dailyRltPlan.length > 0 ? _dailyRltPlan : ''}
          {this.dailyRltEquipments.length > 0 ? <a-row class="rows title">
            <a-col span="24" class="title-daily">
              <span class="text">
                设备信息
              </span>
            </a-col>
          </a-row> : ''}
          {this.dailyRltEquipments.length > 0 ? _dailyRltEquipments : ''}
          {this.unplannedTask.length > 0 ? <a-row class="rows title">
            <a-col span="24" class="title-daily">
              <span class="text">
                临时任务
              </span>
            </a-col>
          </a-row> : ''}
          {this.unplannedTask.length > 0 ? _unplannedTask : ''}
          {this.dailyRltSafe.length > 0 ? <a-row class="rows title">
            <a-col span="24" class="title-daily">
              <span class="text">
                存在的安全问题
              </span>
            </a-col>
          </a-row> : ''}
          {this.dailyRltSafe.length > 0 ? _dailyRltSafe : ''}
          {this.dailyRltQuality.length > 0 ? <a-row class="rows title">
            <a-col span="24" class="title-daily">
              <span class="text">
                存在的质量问题
              </span>
            </a-col>
          </a-row> : ''}
          {this.dailyRltQuality.length > 0 ? _dailyRltQuality : ''}
          <a-row class="rows photos">
            <a-col span="3" class="title">
              施工现场照片
            </a-col>
            <a-col span="21">
              <a-col span="21">{<div class="descriptions-item-media"> {_dailyRltFiles}</div>}</a-col>
            </a-col>
          </a-row>
          <a-row class="rows row-bottom">
            <a-col span="3" class="title">
              其他内容
            </a-col>
            <a-col span="21">
              <span class="text">
                {this.record ? this.record.remark : ''}
              </span>
            </a-col>
          </a-row>
        </div>
        {/* 图片类预览组件 */}
        <SmFileImageView axios={this.axios} ref="SmFileImageView" />
      </div>
    );
  },
};
