<template>
  <a-card>
    <sc-table-operator @search="refresh" @reset="reset">
      <a-form-item label="名称">
        <a-input
          placeholder="请输入名称"
          :value="queryParams.keywords"
          @input="
            event => {
              queryParams.keywords = event.target.value
              refresh()
            }
          "
        />
      </a-form-item>

      <template v-if="!select" slot="buttons">
        <a-button type="primary" icon="plus" @click="() => add(null)">
          添加
        </a-button>
      </template>
    </sc-table-operator>

    <a-table
      row-key="id"
      :columns="columns"
      :data-source="modules"
      default-expand-all-rows
      :row-selection="select ? { selectedRowKeys: iSelectedRowKeys, onChange: onSelectChange } : null"
    >
      <span slot="name" slot-scope="text, record, index">
        {{ `${index + 1}. ${text} ${record.children.length ? '(' + record.children.length + ')' : ''}` }}
      </span>

      <span slot="workDays" slot-scope="text, record">
        {{ getWorkDays(record) }}
      </span>

      <span slot="progress" slot-scope="text, record">
        {{ getProgress(record) }}
      </span>

      <span slot="operations" slot-scope="text, record">
        <a @click="() => add(record.id)">增加子项</a>
        <a-divider type="vertical" />
        <a @click="() => edit(record)">编辑</a>
        <a-divider type="vertical" />
        <a @click="() => remove(record.id)">删除</a>
      </span>
    </a-table>

    <a-modal :title="title" :visible="visible" @cancel="close" @ok="ok">
      <a-form :form="form">
        <a-form-item label="名称" :label-col="formConfig.labelCol" :wrapper-col="formConfig.wrapperCol">
          <a-input
            v-decorator="[
              'name',
              {
                initialValue: '',
                rules: [{ required: true, message: '请输入名称', whitespace: true }]
              }
            ]"
            :disabled="status == ModalStatus.View"
          />
        </a-form-item>

        <a-form-item label="工作量" :label-col="formConfig.labelCol" :wrapper-col="formConfig.wrapperCol">
          <a-input-number
            v-decorator="[
              'workDays',
              {
                initialValue: 0
              }
            ]"
            placeholder="（人/天）"
            :disabled="status == ModalStatus.View"
            style="width:100%"
            :min="1.0"
            :max="200000"
            :step="0.1"
          />
        </a-form-item>

        <a-form-item label="进度" :label-col="formConfig.labelCol" :wrapper-col="formConfig.wrapperCol">
          <a-slider
            v-decorator="[
              'progress',
              {
                initialValue: 0
              }
            ]"
            :min="0"
            :max="1"
            :step="0.01"
            :disabled="status == ModalStatus.View"
            :marks="{ 0: '0', 0.2: '20%', 0.4: '40%', 0.6: '60%', 0.8: '80%', 1.0: '100%' }"
          />
        </a-form-item>

        <a-form-item label="功能描述" :label-col="formConfig.labelCol" :wrapper-col="formConfig.wrapperCol">
          <a-textarea
            v-decorator="[
              'content',
              {
                initialValue: '',
                rules: [{ max: 1000, message: '备注最多输入 1000 字符', whitespace: true }]
              }
            ]"
            :disabled="status == ModalStatus.View"
            rows="3"
          />
        </a-form-item>

        <a-form-item label="备注" :label-col="formConfig.labelCol" :wrapper-col="formConfig.wrapperCol">
          <a-textarea
            v-decorator="[
              'remark',
              {
                initialValue: '',
                rules: [{ max: 1000, message: '备注最多输入 1000 字符', whitespace: true }]
              }
            ]"
            :disabled="status == ModalStatus.View"
            rows="3"
          />
        </a-form-item>
        <a-form-item label="排序" :label-col="formConfig.labelCol" :wrapper-col="formConfig.wrapperCol">
          <a-input-number
            v-decorator="[
              'order',
              {
                initialValue: null
              }
            ]"
            style="width:100%"
            :min="0"
            :max="200000"
            :precision="0"
            :disabled="status == ModalStatus.View"
          />
        </a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>
<script>
import * as apiModule from '@/api/module';
import { requestIsSuccess, getModalTitle, objFilterProps } from 'snweb-module/es/_utils/utils';
import { treeArrayToFlatArray } from 'snweb-module/es/_utils/tree_array_tools';
import { form as formConfig, tips as tipsConfig } from 'snweb-module/es/_utils/config';
import { ModalStatus } from 'snweb-module/es/_utils/enum';
import _ from 'lodash';

// 定义表单字段常量
const formFields = ['name', 'content', 'workDays', 'progress', 'remark', 'order'];

export default {
  name: 'Modules',
  props: {
    select: { type: Boolean, default: false },
    selectedRowKeys: { type: Array, default: () => [] },
  },
  data() {
    return {
      ModalStatus,
      form: this.$form.createForm(this, {}),
      formConfig,
      parentId: null,
      record: {},
      queryParams: {
        keywords: null,
      },

      modules: [],
      flatModules: [],
      status: ModalStatus.Hide,
      iSelectedRowKeys: [],
    };
  },
  computed: {
    title() {
      // 计算模态框的标题变量
      return getModalTitle(this.status);
    },
    visible() {
      // 计算模态框的显示变量k
      return this.status !== ModalStatus.Hide;
    },

    columns() {
      let columns = [
        {
          title: '名称',
          dataIndex: 'name',
          width: '200px',
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '内容',
          dataIndex: 'content',
          key: 'content',
        },
        {
          title: '工作量',
          dataIndex: 'workDays',
          width: '80px',
          key: 'workDays',
          scopedSlots: { customRender: 'workDays' },
        },
        {
          title: '进度',
          dataIndex: 'progress',
          width: '60px',
          key: 'progress',
          scopedSlots: { customRender: 'progress' },
        },
        {
          title: '备注',
          dataIndex: 'remark',
          key: 'remark',
          width: '100px',
        },
      ];

      if (!this.select) {
        columns = [
          ...columns,
          {
            title: '排序',
            dataIndex: 'order',
            key: 'order',
          },
          {
            title: '操作',
            dataIndex: 'operations',
            width: '180px',
            scopedSlots: { customRender: 'operations' },
          },
        ];
      }
      return columns;
    },
  },
  watch: {
    selectedRowKeys: {
      handler: function(value, oldValue) {
        this.iSelectedRowKeys = value;
      },
      immediate: true,
    },
  },
  async created() {
    this.refresh();
  },
  methods: {
    getWorkDays(item) {
      let sum = 0;
      let loop = array => {
        array.map(item => {
          if (item.children && item.children.length) {
            loop(item.children);
          } else {
            sum += item.workDays;
          }
        });
      };

      if (item.children && item.children.length) {
        loop(item.children);
      } else {
        sum = item.workDays;
      }

      return sum.toFixed(2);
    },
    getProgress(item) {
      let progress = [];
      let loop = array => {
        array.map(item => {
          if (item.children && item.children.length) {
            loop(item.children);
          } else {
            console.log(item.progress);
            progress.push(item.progress);
          }
        });
      };

      if (item.children && item.children.length) {
        loop(item.children);
      } else {
        progress = [item.progress];
      }

      console.log(progress);

      return (_.sum(progress) / progress.length).toFixed(2);
    },
    async refresh() {
      let response = await apiModule.getList(this.queryParams);
      if (requestIsSuccess(response)) {
        let modules = JSON.parse(JSON.stringify(response.data));

        this.flatModules = treeArrayToFlatArray(modules);
        this.modules = modules;
      }
    },
    reset() {
      this.queryParams.keywords = null;
      this.refresh();
    },

    add(parentId) {
      this.status = ModalStatus.Add;
      this.parentId = parentId;
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },

    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.parentId = record.parentId;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...objFilterProps(record, formFields) });
      });
    },

    // 详情
    view(id) {
      this.$router.push({
        name: project,
        params: {
          id,
        },
      });
    },

    remove(id) {
      const _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          // 删除角色业务逻辑
          return new Promise(async (resolve, reject) => {
            let response = await apiModule.remove(id);
            _this.refresh();
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },

    // 关闭模态框
    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
      this.parentId = null;
      this.record = {};
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = null;
          let data = {
            ...values,
            order: values.order || 0,
            parentId: this.parentId,
            id: this.record.id,
          };

          // 添加
          if (this.status === ModalStatus.Add) {
            response = await apiModule.create(data);
          } else if (this.status === ModalStatus.Edit) {
            // 编辑
            response = await apiModule.update(data);
          }

          if (requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.$emit('success');
            this.refresh();
          }
        }
      });
    },

    onSelectChange(iSelectedRowKeys) {
      let _selectedRowKeys = [...iSelectedRowKeys];
      // 如果子类选中，父类一定选中
      iSelectedRowKeys.forEach(key => {
        let target = this.flatModules.find(x => x.id == key);
        let parent = this.flatModules.find(x => x.id == target.parentId);
        if (parent && !iSelectedRowKeys.find(x => x == parent.id)) {
          _selectedRowKeys.push(parent.id);
        }
      });

      this.iSelectedRowKeys = _selectedRowKeys;
      let selected = JSON.parse(JSON.stringify(this.flatModules.filter(x => this.iSelectedRowKeys.indexOf(x.id) > -1)));
      selected.map(item => (item.children = null));
      this.$emit('selected', selected);
    },
  },
};
</script>
