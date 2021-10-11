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

      <template slot="buttons">
        <a-button type="primary" icon="plus" @click="() => add(null)">
          添加
        </a-button>
      </template>
    </sc-table-operator>

    <a-table
      row-key="id"
      :columns="columns"
      :data-source="projects"
      default-expand-all-rows
    >
      <span slot="name" slot-scope="text, record, index">
        {{ `${index + 1}. ${text}` }}
      </span>
      <span slot="operations" slot-scope="text, record">
        <!-- <a @click="() => add(record.parentId)">增加子项</a>
        <a-divider type="vertical" /> -->
        <a @click="() => edit(record)">编辑</a>
        <a-divider type="vertical" />
        <a @click="() => view(record)">查看</a>
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

        <a-form-item label="备注" :label-col="formConfig.labelCol" :wrapper-col="formConfig.wrapperCol">
          <a-textarea
            v-decorator="[
              'remark',
              {
                initialValue: '',
                rules: [{ max: 1000, message: '备注最多输入 1000 字符', whitespace: true }]
              }
            ]"
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
          />
        </a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>
<script>
import * as apiProject from '@/api/project';
import { requestIsSuccess, getModalTitle, objFilterProps } from 'snweb-module/es/_utils/utils';
import { form as formConfig, tips as tipsConfig } from 'snweb-module/es/_utils/config';
import { ModalStatus } from 'snweb-module/es/_utils/enum';

// 定义表单字段常量
const formFields = ['name', 'order', 'content'];

export default {
  name: 'Project',

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

      projects: [],
      status: ModalStatus.Hide,

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
      let col = [
        {
          title: '名称',
          dataIndex: 'name',
          width: '30%',
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '简介',
          dataIndex: 'content',
          key: 'content',
        },
        {
          title: '总价',
          dataIndex: 'sumPrice',
          key: 'sumPrice',
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: '180px',
          scopedSlots: { customRender: 'operations' },
        },
      ];
      return col;
    },
  },
  async created() {
    this.refresh();
  },
  methods: {
    async refresh() {
      console.log(this.queryParams);
      let response = await apiProject.getList(this.queryParams);
      if (requestIsSuccess(response)) {
        this.projects = response.data;
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
      this.$router.push({
        name: 'project_edit',
        params: {
          id: record.id,
          state: ModalStatus.Edit.toString(),
        },
      });
    },
    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = null;
          let data = {
            ...values,
            parentId: this.parentId,
            id: this.record.id,
            order: values.order || 0,
          };

          // 添加
          if (this.status === ModalStatus.Add) {
            response = await apiProject.create(data);
          } else if (this.status === ModalStatus.Edit) {
            // 编辑
            response = await apiProject.update(data);
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

    // 详情
    view(record) {
      this.$router.push({
        name: 'project_view',
        params: {
          id: record.id,
          state: ModalStatus.View.toString(),
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
            let response = await apiProject.remove(id);
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
  },
};
</script>
