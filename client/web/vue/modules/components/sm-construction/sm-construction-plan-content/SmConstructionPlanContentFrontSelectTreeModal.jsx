
import './style';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';

import ApiPlanContent from '../../sm-api/sm-construction/ApiPlanContent';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';


let apiPlanContent = new ApiPlanContent();
let apiDataDictionary = new ApiDataDictionary();// 字典api 查 类型用


// 表单字段
export default {
  name: 'SmConstructionPlanContentFrontSelectTreeModal', // 计划详情 选择树 - 模态框
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      selectedContentIds: [], // 选中的 content id 列表
      list: [], // 数据源
      selfId:undefined, // 选择的contentId(自己)
    };
  },
  computed: {
    visible() {
      return this.status !== ModalStatus.Hide; // 计算模态框的显示变量
    },
  },
  async created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiPlanContent = new ApiPlanContent(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
    },
    // 添加前置任务
    addFrontTask(record,list) {
      this.list = list;
      console.log(list);
      // 将 record 的 前置任务 中的 contentId 列表 弄到 选择框里面1
      this.selectedContentIds = record.antecedents.map(x => x.frontPlanContentId);
      console.log(this.selectedContentIds );
      this.selfId = record.id;
      this.status = ModalStatus.Add;
    },
    // 关闭模态框
    close() {
      this.status = ModalStatus.Hide;
    },
    // 数据提交
    async ok() {
      const response = await apiPlanContent.changeFrontTask(this.selfId, this.selectedContentIds);
      if (utils.requestIsSuccess(response)) {
        this.$message.success('操作成功');
        this.close();
        this.$emit('success');
      }
    },
  },
  render () {
    return (
      <a-modal
        title='添加前置任务(可多选)'
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose={true}
        onOk={this.ok}
      >
        <a-tree-select
          value={this.selectedContentIds}
          style='width: 100%'
          replaceFields={{ title: 'name', key: 'id', value: 'id' }}
          treeData={this.list}
          allowClear={true}
          multiple={true}
          treeDefaultExpandAll={true}
          showCheckedStrategy='SHOW_CHILD'
          searchPlaceholder='请选择前置任务'
          onChange={(value, label) => {
            if (value.includes(this.selfId)) {
              this.$message.error("不能选自己作为前置任务!");
              return;
            }
            this.selectedContentIds = value;
          }}
        />
      </a-modal>
    );
  },
};
