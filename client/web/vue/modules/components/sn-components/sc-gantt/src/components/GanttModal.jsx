
import { form as formConfig } from '../../../../_utils/config';
import * as utils from '../../../../_utils/utils';
import moment from 'moment';
import dayjs from 'dayjs'; // 导入日期js
import { ModalStatus } from '../../../../_utils/enum';

// 表单字段
const formFields = [
  'name', // 任务名
  'content', // 工作内容
  'startDate', //  任务开始日期
  'endDate', // 任务结束日期
  'duration', // 任务工期
  'parentId', // 父id Guid
  'isMilestone', // 是否里程碑
  'preTaskIds', // 前置任务ids
  'topLvTreeId', // 顶级树id ,添加第一个顶级任务的时候 用 (类似 planContent 的 planId ,冒泡后记得转换)
];
export default {
  // 甘特图添加编辑 模态框
  name: 'GanttModal',
  props: {
    selectTree: { type: Array, default: () => [] }, // 选择树列表,选择前置的时候用
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象
      parentId: undefined, // pid 添加的时候是undefined ,插入子级的时候 有值
      startDateMark: '', // 开始 时间标记 ,有了这个 好判断 结束时间的范围 (不能小于开始时间)
      earlyDateMark: '', // 最早时间标记,开始时间不能小于这个时间范围
      endDateMark: '', // 结束 时间标记 ,有了这个 好判断 开始时间的范围 (不能大于开始时间)
      editId: undefined, //要 编辑的id
      topLvTreeId: undefined, // 顶级树id ,添加第一个顶级任务的时候 用 (类似 planContent 的 planId ,冒泡后记得转换)
      iSelectTree: [],
    };
  },
  computed: {
    title() {
      return utils.getModalTitle(this.status); // 计算模态框的标题变量
    },
    visible() {
      return this.status !== ModalStatus.Hide; // 计算模态框的显示变量
    },
  },

  async created() {
    this.form = this.$form.createForm(this, {});// 创建表单
  },
  methods: {
    onChange(date, dateString) {
      // console.log(date, dateString);
      // console.log(this.form.getFieldsValue());
    },
    // 添加
    add(topLvTreeId) {
      if (!!topLvTreeId) { // contentId  不为 null 或者 undefined 或者  ''
        this.topLvTreeId = topLvTreeId;
      }
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },
    // 添加子级(插入子任务)
    addChild(parent) {
      // console.log('添加子级(插入子任务)',parentId);
      if (!!parent) { // parentId  不为 null 或者 undefined 或者  ''
        this.parentId = parent.id;
        this.earlyDateMark = parent.startDate;
      }
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.parentId = undefined;
      this.editId = undefined;
      this.startDateMark = '';
      this.endDateMark = '';
      this.status = ModalStatus.Hide;
      this.iSelectTree = [];
    },
    // 编辑
    edit(record) {
      record = {
        ...record,
        startDate: record.startDate ? moment(record.startDate) : null,
        endDate: record.endDate ? moment(record.endDate) : null,
      };
      this.status = ModalStatus.Edit;
      this.record = record;
      this.startDateMark = record.startDate;
      this.endDateMark = record.endDate;
      this.editId = record.id;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          // this.$message.success('操作成功');
          if (this.status === ModalStatus.Add) {
            this.$emit('add', values);
          }
          if (this.status === ModalStatus.Edit) {
            this.$emit('edit', this.editId, values);
            this.record.disabled = false;
          }
          this.close(); // 这个要放在最后 ,不然 就把 前面 设置 的状态 搞空了……
        }
      });
    },
  },
  render() {
    return (
      <a-modal
        title={`任务${this.title}`}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose={true}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label='任务名称'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            {
              this.form.getFieldDecorator('name', {
                initialValue: '',
                rules: [{ required: true, message: '请输入任务名称！', whitespace: true }],
              })(<a-input disabled={this.status === ModalStatus.View} maxLength={20} placeholder={'请输入任务名称(最多20个字符)'} />)
            }
          </a-form-item>

          <a-form-item
            label='开始时间'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-date-picker
              valueFormat='YYYY-MM-DD'
              onChange={(date, dateStr) => {
                // 修改时间后把 开始 时间标记 设置下~
                this.startDateMark = dateStr;
                if (!!this.endDateMark) {
                  let duration = dayjs(this.endDateMark).diff(dayjs(this.startDateMark), 'day');
                  this.form.setFieldsValue({ duration: duration });
                }
              }}
              disabledDate={current => current && current > moment(this.endDateMark).subtract(1, 'days') || current < moment(this.earlyDateMark)}
              placeholder={this.status === ModalStatus.View ? '' : '开始时间'}
              disabled={this.status === ModalStatus.View}
              style='width: 100%'
              v-decorator={[
                'startDate',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请选择开始时间',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>

          <a-form-item
            label='结束时间'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-date-picker
              valueFormat='YYYY-MM-DD'
              onChange={(date, dateStr) => {
                // 修改时间后把 开始 时间标记 设置下~
                this.endDateMark = dateStr;
                if (!!this.startDateMark) {
                  let duration = dayjs(this.endDateMark).diff(dayjs(this.startDateMark), 'day') + 1;
                  this.form.setFieldsValue({ duration: duration });
                }
              }}
              placeholder={this.status === ModalStatus.View ? '' : '计划结束时间'}
              disabled={this.status === ModalStatus.View}
              disabledDate={current => current && current < moment(this.startDateMark).subtract(-1, 'days')}
              style='width: 100%'
              v-decorator={[
                'endDate',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '选择计划结束时间',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>

          <a-form-item
            label='工期(天)'
            label-col={formConfig.labelCol}
            disabled={true}
            wrapper-col={formConfig.wrapperCol}>
            {
              this.form.getFieldDecorator('duration', {
                initialValue: 0,
                rules: [{ required: true, message: '请输入工期!' }],
              })(
                <a-input-number disabled={true}
                  style='width:100%'
                  min={1}
                  precision={0.1}
                  placeholder={'工期'}
                />,
              )
            }
          </a-form-item>
          <a-form-item
            label='顶级树id' // 这个隐藏了
            label-col={formConfig.labelCol}
            style="display:none"
            wrapper-col={formConfig.wrapperCol}
          >
            {
              this.form.getFieldDecorator('topLvTreeId', {
                initialValue: this.topLvTreeId,
                rules: [],
              })(<a-input />)
            }
          </a-form-item>

          <a-form-item
            label='父级id' // 这个隐藏起来
            style="display:none;"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            {
              this.form.getFieldDecorator('parentId', {
                initialValue: this.parentId,
                rules: [],
              })(<a-input placeholder={'请输入父级id'} />)
            }
          </a-form-item>
          <a-form-item
            label='是否里程碑'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-switch
              checked={this.record.isMilestone}
              v-decorator={[
                'isMilestone',
                {
                  initialValue: false,
                  rules: [],
                },
              ]}
              onChange={value => {
                this.record.isMilestone = value;
              }}
            />
          </a-form-item>
          <a-form-item
            label='前置任务'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >

            <a-tree-select
              showSearch={true}
              treeData={this.selectTree}
              replaceFields={{ title: 'name', key: 'id', value: 'id' }}
              style='width: 100%'
              dropdownStyle={{ maxHeight: '400px', overflow: 'auto' }}
              placeholder='选择前置任务'
              allowClear={true}
              multiple={true}
              treeDefaultExpandAll={true}
              v-decorator={[
                'preTaskIds',
                {
                  initialValue: [],
                  rules: [],
                },
              ]}
            />
          </a-form-item>
          <a-form-item label="工作内容"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              placeholder='请输入工作内容'
              v-decorator={[
                'content',
                {
                  initialValue: '',
                },
              ]}
            />
          </a-form-item>

        </a-form>
      </a-modal>
    );
  },
};
