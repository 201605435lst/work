<template>
  <a-card class="project" :title="'项目：' + (record && record.name ? record.name : '')">
    <a-space slot="extra">
      <template v-if="state == ModalStatus.Edit">
        <a-button icon="plus" type="link" @click="add">
          添加模块
        </a-button>

        <a-button type="primary" @click="ok">
          保存
        </a-button>
      </template>

      <template v-else>
        <a-button type="primary" @click="edit">
          编辑
        </a-button>
      </template>

      <SmExport
        ref="smExport"
        url="api/app/projectPriceProject/export"
        default-title="导出"
        :axios="axios"
        template-name="projectPriceProject"
        download-file-name="工程报价"
        :row-index="2"
        @download="downloadFile"
      />,

      <a-button type="primary" @click="back">
        返回
      </a-button>
    </a-space>

    <template v-if="record != null">
      <a-descriptions
        v-if="state == ModalStatus.View"
        class="baseinfo"
        layout="vertical"
        :colon="false"
        title="基本信息"
        bordered
      >
        <a-descriptions-item label="项目名称">
          {{ record.name }}
        </a-descriptions-item>
        <a-descriptions-item label="总价">
          {{ record.sumPrice }}
        </a-descriptions-item>
        <!-- <a-descriptions-item label="创建日期">
          {{ '2021-3-12' }}
        </a-descriptions-item> -->

        <!-- <a-descriptions-item label="状态" :span="3">
          <a-badge status="processing" text="Running" />
        </a-descriptions-item> -->
        <!-- <a-descriptions-item label="Negotiated Amount">
          $80.00
        </a-descriptions-item>
        <a-descriptions-item label="Discount">
          $20.00
        </a-descriptions-item>
        <a-descriptions-item label="Official Receipts">
          $60.00
        </a-descriptions-item> -->
        <!-- <a-descriptions-item label="项目简介" :span="2">
          公伯峡水库灌区工程是指由公伯峡水库引水，黄河南北两岸各建一条干渠及支斗渠等田间配套工程组成，主要解决该地区黄河河谷两岸灌溉及人畜饮水问题，该工程是我省“十五”计划重点水利工程。
          项目区位于青海省境内黄河干流公伯峡水电站下游的两岸阶地上，隶属循化撒拉族自治县、化隆回族自治县等两个少数民族县。该地区降水量为259.4mm，蒸发量2206.4mm，沟河多为季节性河流，气候干旱，素有“干循化”之称。
        </a-descriptions-item> -->
      </a-descriptions>

      <template v-else>
        <div class="ant-descriptions-title">
          基本信息
        </div>
        <a-form :form="form">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item label="名称">
                <a-input
                  v-decorator="[
                    'name',
                    {
                      initialValue: '',
                      rules: [{ required: true, message: '请输入名称', whitespace: true }]
                    }
                  ]"
                  :disabled="state == ModalStatus.View"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item label="排序">
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
            </a-col>

            <a-col :span="24">
              <a-form-item label="简介">
                <a-textarea
                  v-decorator="[
                    'description',
                    {
                      initialValue: '',
                      rules: [{ max: 1000, message: '备注最多输入 1000 字符', whitespace: true }]
                    }
                  ]"
                  style="max-width: none !important "
                  rows="3"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </a-form>
      </template>

      <br>
      <a-descriptions class="modules" title="功能报价" layout="vertical" bordered>
        <a-descriptions-item :span="3">
          <a-table
            :columns="columns"
            row-key="id"
            :data-source="record.projectRltModules"
            default-expand-all-rows
          >
            <span slot="index" slot-scope="text, record, index">
              {{ index + 1 }}
            </span>

            <span slot="name" slot-scope="text, record">
              <a-input
                v-if="record.editable"
                key="name"
                style="margin: -5px 0"
                :value="record.name"
                @change="
                  e => {
                    record.name = e.target.value
                  }
                "
              />
              <span
                v-else
                :class="{
                  change: !!record.name
                }"
              >
                {{ record.name || record.module.name }}
              </span>
            </span>

            <span slot="content" slot-scope="text, record">
              <!-- <a-input
                v-if="record.editable"
                key="content"
                style="margin: -5px 0"
                :value="record.content"
                @change="
                  e => {
                    record.content = e.target.value
                  }
                "
              /> -->
              <span
                :class="{
                  change: !!record.content
                }"
              >
                {{ record.content || record.module.content }}
              </span>
            </span>

            <span slot="workDays" slot-scope="text, record">
              {{ getWorkDays(record) }}
            </span>

            <span slot="progress" slot-scope="text, record">
              {{ getProgress(record) }}
            </span>

            <span slot="remark" slot-scope="text, record">
              <a-input
                v-if="record.editable"
                key="remark"
                style="margin: -5px 0"
                :value="record.remark"
                @change="
                  e => {
                    record.remark = e.target.value
                  }
                "
              />
              <span
                v-else
                :class="{
                  change: !!record.remark
                }"
              >
                {{ record.remark || record.module.remark }}
              </span>
            </span>

            <span slot="operations" slot-scope="text, record">
              <a v-if="state == ModalStatus.Edit" @click="() => (record.editable = !record.editable)">
                {{ record.editable ? '确定' : '编辑' }}
              </a>
              <!-- <a-divider type="vertical" />
              <a
                @click="
                  () => {
                    remove(record)
                  }
                "
              >删除</a> -->
            </span>
          </a-table>
        </a-descriptions-item>
      </a-descriptions>
    </template>

    <a-modal
      v-model="moduleModalVisisble"
      width="50% "
      title="添加模块"
      @ok="onBtnModuleOk"
      @cancel="onBtnModuleCancel"
    >
      <Modules select :selected-row-keys="selectedRowKeys" @selected="onSelectChange" />
    </a-modal>
  </a-card>
</template>

<script>
import * as apiProject from '@/api/project';
import { requestIsSuccess, getModalTitle, objFilterProps } from 'snweb-module/es/_utils/utils';
import { treeArrayToFlatArray, treeArrayLoop } from 'snweb-module/es/_utils/tree_array_tools';
import { v4 as uuidv4 } from 'uuid';
import { toTreeArray } from 'tree-array-tools';

import { form as formConfig, tips as tipsConfig } from 'snweb-module/es/_utils/config';
import { ModalStatus } from 'snweb-module/es/_utils/enum';
import Modules from '../module/Modules';
import SmExport from 'snweb-module/es/sm-common/sm-export-module';

// 定义表单字段常量
const formFields = ['name', 'order', 'description'];

export default {
  name: 'Project',
  components: {
    Modules,
    SmExport,
  },
  props: {
    id: { type: String, default: null },
    state: { type: String, default: null },
  },
  data() {
    return {
      moduleModalVisisble: false,
      ModalStatus,
      form: this.$form.createForm(this, {}),
      formConfig,
      record: null,
      flatProjectRltModules: [],
      columns: [
        {
          title: '序号',
          dataIndex: 'index',
          width: '120px',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '功能',
          dataIndex: 'name',
          key: 'name',
          width: '30%',
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '描述',
          dataIndex: 'content',
          key: 'content',
          scopedSlots: { customRender: 'content' },
        },
        {
          title: '工作量',
          dataIndex: 'workDays',
          key: 'workDays',
          scopedSlots: { customRender: 'workDays' },
        },
        {
          title: '进度',
          dataIndex: 'progress',
          key: 'progress',
          scopedSlots: { customRender: 'progress' },
        },
        {
          title: '备注',
          dataIndex: 'remark',
          key: 'remark',
          scopedSlots: { customRender: 'remark' },
        },
        {
          title: '排序',
          dataIndex: 'order',
          key: 'order',
          scopedSlots: { customRender: 'order' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: '60px',
          scopedSlots: { customRender: 'operations' },
        },
      ],
      selectedModules: [],
    };
  },

  computed: {
    expandedRowKeys: function() {
      return this.flatProjectRltModules.map(item => item.id);
    },
    selectedRowKeys: function() {
      return this.flatProjectRltModules.map(item => item.module.id);
    },
  },

  watch: {
    state: function(val, oldVal) {
      this.refresh();
    },
  },
  async created() {
    this.refresh();
  },

  methods: {
    async downloadFile(params) {
      //执行文件下载
      console.log(params);
      await this.$refs.smExport.isCanDownload(params, { id: this.id });
    },

    getWorkDays(item) {
      let sum = 0;
      let loop = array => {
        array.map(item => {
          if (item.children && item.children.length) {
            loop(item.children);
          } else {
            sum += item.module.workDays;
          }
        });
      };

      if (item.children && item.children.length) {
        loop(item.children);
      } else {
        sum = item.module.workDays;
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
            progress.push(item.module.progress);
          }
        });
      };

      if (item.children && item.children.length) {
        loop(item.children);
      } else {
        progress = [item.module.progress];
      }

      return (_.sum(progress) / progress.length).toFixed(2);
    },

    back() {
      this.$router.back();
    },
    async refresh() {
      let response = await apiProject.get(this.id);
      if (requestIsSuccess(response)) {
        treeArrayLoop(response.data.projectRltModules, item => {
          item.editable = false;
        });
        this.record = response.data;

        this.flatProjectRltModules = JSON.parse(JSON.stringify(treeArrayToFlatArray(this.record.projectRltModules)));

        this.$nextTick(() => {
          this.form.setFieldsValue({ ...objFilterProps(this.record, formFields) });
        });
      }
    },
    onSelectChange(selected) {
      this.selectedModules = selected;
    },
    onBtnModuleOk() {
      this.moduleModalVisisble = false;
      let rlts = this.selectedModules.map(item => {
        // 找现有关系
        let target = this.flatProjectRltModules.find(x => x.moduleId == item.id);
        console.log(target);
        let id = uuidv4();
        let rlt = target
          ? {
              id: target.id,
              name: target.name,
              content: target.content,
              remark: target.remark,
              order: target.order,
            }
          : {
              id,
              name: null,
              content: null,
              remark: null,
              order: 0,
            };

        return {
          ...rlt,
          projectId: this.id,
          moduleId: item.id,
          module: item,
          editable: false,
        };
      });

      rlts.map(item => {
        let parent = rlts.find(x => x.module && x.module.id == item.module.parentId);
        if (parent) {
          parent.children = parent.children || [];
          parent.children.push(item);
          item.parentId = parent.id;
        }
      });

      this.flatProjectRltModules = [...rlts];
      rlts = rlts.filter(x => !x.parentId);
      this.record.projectRltModules = rlts;
    },
    onBtnModuleCancel() {
      this.moduleModalVisisble = false;
    },
    edit() {
      this.$router.push({
        name: 'project_edit',
        params: {
          id: this.id,
          state: ModalStatus.Edit.toString(),
        },
      });
    },
    add() {
      this.moduleModalVisisble = true;
      this.selectedModules = this.record.projectRltModules;
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
            projectRltModules: this.record.projectRltModules,
          };

          response = await apiProject.update(data);

          if (requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.refresh();
          }
        }
      });
    },
    remove(rlt) {
      // console.log(rlt);
      // if (rlt.parent) {
      //   let index = rlt.parent.children.findIndex(x => x.id === rlt.id);
      //   if (index > -1) rlt.parent.splice(index, 1);
      // } else {
      //   let index = this.record.projectRltModules.indexOf(rlt);
      //   if (index > -1) {
      //     this.record.projectRltModules.splice(index, 1);
      //   }
      // }
      // let index = this.flatProjectRltModules.indexOf(rlt);
      // if (index > -1) {
      //   this.flatProjectRltModules.splice(index, 1);
      // }
      // 删除
    },
  },
};
</script>

<style lang="less">
.project {
  .baseinfo {
    .ant-descriptions-row {
      display: flex;
      & > * {
        flex: 1;
      }
    }
  }

  .modules {
    .ant-descriptions-row {
      &:first-child {
        display: none;
      }
      .ant-descriptions-item-content {
        padding: 0;
      }
    }
  }

  .change {
    color: rgb(255, 90, 61);
  }
}
</style>
