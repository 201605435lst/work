import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import SmSystemOrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import ApiOrganizationRltLayer from '../../sm-api/sm-resource/OrganizationRltLayer';

let apiOrganizationRltLayer = new ApiOrganizationRltLayer();

export default {
  name: 'SmD3SetLayerModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      loading: false, // 数据保存加载状态
      form: {}, // 表单
      layers: [],
    };
  },
  computed: {
    title() {
      // 计算模态框的标题变量
      return utils.getModalTitle(this.status);
    },
    visible() {
      // 计算模态框的显示变量
      return this.status !== ModalStatus.Hide;
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiOrganizationRltLayer = new ApiOrganizationRltLayer(this.axios);
    },

    // 添加
    async relate(data) {
      this.layers = data;
      this.status = ModalStatus.Add;
      let layerIds = [];
      let organizationId = localStorage.getItem('OrganizationId');
      if (organizationId) {
        layerIds = await this.getLayerIds(organizationId);
      }
      this.$nextTick(() => {
        this.form.setFieldsValue({ organizationId, layerIds });
      });
    },

    async getLayerIds(organizationId) {
      let layerIds = [];
      let response = await apiOrganizationRltLayer.getLayerIdsByOrganizationId(organizationId);
      if (utils.requestIsSuccess(response)) {
        layerIds = response.data;
      }
      return layerIds;
    },

    // 关闭模态框
    close() {
      this.form.resetFields();
      this.loading = false;
      this.status = ModalStatus.Hide;
      this.layers = [];
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = await apiOrganizationRltLayer.create(values);
          if (utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.$emit('success');
          }
        }
      });
      this.loading = false;
    },
  },
  render() {
    return (
      <a-modal
        title='关联图层'
        visible={this.visible}
        confirmLoading={this.loading}
        destroyOnClose={true}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label="组织机构"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmSystemOrganizationTreeSelect
              axios={this.axios}
              placeholder='请选择组织机构'
              treeCheckable={false}
              onChange={async value => {
                console.log(value);
                let layerIds = await this.getLayerIds(value);
                this.form.setFieldsValue({ layerIds });
              }}
              v-decorator={[
                'organizationId',
                {
                  initialValue: null,
                  rules: [{ required: true, message: '请选择组织机构' }],
                },
              ]}
            />
          </a-form-item>


          <a-form-item
            label='图层'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-select
              disabled={this.disabled}
              placeholder='请关联图层'
              allowClear
              mode="multiple"
              options={this.layers}
              style="width: 100%"
              v-decorator={[
                'layerIds',
                {
                  initialValue: [],
                  rules: [{ required: true, message: '请关联图层' }],
                },
              ]}
            />
          </a-form-item>

        </a-form>
      </a-modal>
    );
  },
};
